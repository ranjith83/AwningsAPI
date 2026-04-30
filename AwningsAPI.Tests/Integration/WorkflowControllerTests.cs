using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Model.Workflow;
using AwningsAPI.Tests.Helpers;
using FluentAssertions;

namespace AwningsAPI.Tests.Integration;

/// <summary>
/// Spins up the real ASP.NET Core pipeline with InMemory DB and verifies HTTP-level
/// behaviour for the InitialEnquiry endpoints: auth guards, 200/201/404 responses,
/// and that the soft-delete query filter is respected.
/// </summary>
public class WorkflowControllerTests : IClassFixture<WorkflowApiFixture>
{
    private readonly WorkflowApiFixture _fixture;
    private readonly HttpClient _client;

    public WorkflowControllerTests(WorkflowApiFixture fixture)
    {
        _fixture = fixture;
        _client  = fixture.CreateClient();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void Authenticate(string username = "testuser") =>
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", WorkflowApiFixture.GenerateTestJwt(username));

    private void ClearAuth() =>
        _client.DefaultRequestHeaders.Authorization = null;

    private async Task<int> SeedEnquiryAsync(int workflowId = 1)
    {
        using var ctx = _fixture.GetDbContext();
        var enquiry = new InitialEnquiry
        {
            WorkflowId  = workflowId,
            Comments    = "Integration test enquiry",
            Email       = "it@test.com",
            DateCreated = DateTime.UtcNow,
            CreatedBy   = "seed",
        };
        ctx.InitialEnquiries.Add(enquiry);
        await ctx.SaveChangesAsync();
        return enquiry.EnquiryId;
    }

    // ── Auth guard: unauthenticated requests ──────────────────────────────────

    [Fact]
    public async Task GetEnquiries_WithoutAuth_Returns401()
    {
        ClearAuth();
        var response = await _client.GetAsync("/api/workflow/GeInitialEnquiryForWorkflow?WorkflowId=1");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task AddEnquiry_WithoutAuth_Returns401()
    {
        ClearAuth();
        var response = await _client.PostAsJsonAsync("/api/workflow/AddInitialEnquiry",
            new InitialEnquiryDto { WorkflowId = 1, Comments = "test", Email = "t@t.com" });
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteEnquiry_WithoutAuth_Returns401()
    {
        ClearAuth();
        var response = await _client.DeleteAsync("/api/workflow/DeleteInitialEnquiry/1");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ── GET enquiries ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetEnquiries_WithAuth_ReturnsActiveEnquiriesOnly()
    {
        Authenticate();
        using var ctx = _fixture.GetDbContext();

        // Seed an active enquiry and a soft-deleted one for the same workflow
        ctx.InitialEnquiries.AddRange(
            new InitialEnquiry { WorkflowId = 100, Comments = "Active",  Email = "a@t.com", DateCreated = DateTime.UtcNow, CreatedBy = "seed" },
            new InitialEnquiry { WorkflowId = 100, Comments = "Deleted", Email = "d@t.com", DateCreated = DateTime.UtcNow, CreatedBy = "seed", IsDeleted = true, DeletedAt = DateTime.UtcNow, DeletedBy = "seed" });
        await ctx.SaveChangesAsync();

        var response = await _client.GetAsync("/api/workflow/GeInitialEnquiryForWorkflow?WorkflowId=100");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<List<InitialEnquiryDto>>();
        body.Should().HaveCount(1);
        body![0].Comments.Should().Be("Active");
    }

    [Fact]
    public async Task GetEnquiries_ForWorkflowWithNoEnquiries_ReturnsEmptyArray()
    {
        Authenticate();
        var response = await _client.GetAsync("/api/workflow/GeInitialEnquiryForWorkflow?WorkflowId=99999");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<List<InitialEnquiryDto>>();
        body.Should().BeEmpty();
    }

    // ── POST AddInitialEnquiry ────────────────────────────────────────────────

    [Fact]
    public async Task AddEnquiry_WithAuth_Returns201AndPersists()
    {
        Authenticate();
        var dto = new InitialEnquiryDto { WorkflowId = 200, Comments = "New enquiry via API", Email = "new@test.com" };

        var response = await _client.PostAsJsonAsync("/api/workflow/AddInitialEnquiry", dto);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        using var ctx = _fixture.GetDbContext();
        ctx.InitialEnquiries.Any(e => e.WorkflowId == 200).Should().BeTrue();
    }

    // ── PUT UpdateInitialEnquiry ──────────────────────────────────────────────

    [Fact]
    public async Task UpdateEnquiry_WithAuth_WhenExists_Returns200()
    {
        Authenticate();
        var enquiryId = await SeedEnquiryAsync(workflowId: 300);

        var dto = new InitialEnquiryDto
        {
            EnquiryId  = enquiryId,
            WorkflowId = 300,
            Comments   = "Updated via API",
            Email      = "upd@test.com",
        };

        var response = await _client.PutAsJsonAsync("/api/workflow/UpdateInitialEnquiry", dto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── DELETE DeleteInitialEnquiry ───────────────────────────────────────────

    [Fact]
    public async Task DeleteEnquiry_WithAuth_WhenExists_Returns200AndSoftDeletes()
    {
        Authenticate();
        var enquiryId = await SeedEnquiryAsync(workflowId: 400);

        var response = await _client.DeleteAsync($"/api/workflow/DeleteInitialEnquiry/{enquiryId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify the record still exists in DB but is flagged as deleted
        using var ctx = _fixture.GetDbContext();
        var row = ctx.InitialEnquiries
            .IgnoreQueryFilters()
            .Single(e => e.EnquiryId == enquiryId);
        row.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteEnquiry_WithAuth_WhenNotExists_Returns404()
    {
        Authenticate();
        var response = await _client.DeleteAsync("/api/workflow/DeleteInitialEnquiry/999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteEnquiry_WhenAlreadyDeleted_Returns404()
    {
        Authenticate();
        using var ctx = _fixture.GetDbContext();
        var enquiry = new InitialEnquiry
        {
            WorkflowId  = 500,
            Comments    = "Already gone",
            Email       = "gone@test.com",
            DateCreated = DateTime.UtcNow,
            CreatedBy   = "seed",
            IsDeleted   = true,
            DeletedAt   = DateTime.UtcNow,
            DeletedBy   = "seed",
        };
        ctx.InitialEnquiries.Add(enquiry);
        await ctx.SaveChangesAsync();

        // The query filter means FindAsync won't find it → 404
        var response = await _client.DeleteAsync($"/api/workflow/DeleteInitialEnquiry/{enquiry.EnquiryId}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
