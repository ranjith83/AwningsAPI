using AwningsAPI.Dto.Customers;
using AwningsAPI.Model.Customers;
using AwningsAPI.Services.CustomerService;
using AwningsAPI.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace AwningsAPI.Tests.Unit;

public class CustomerServiceTests
{
    // ── Shared helpers ────────────────────────────────────────────────────────

    private static CustomerService Build(AwningsAPI.Database.AppDbContext ctx)
    {
        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory
            .Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(new HttpClient());

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["TomTom:ApiKey"] = "test" })
            .Build();

        return new CustomerService(ctx, httpClientFactory.Object, config, NullLogger<CustomerService>.Instance);
    }

    private static Customer MakeCustomer(int id, string name = "Acme Ltd") =>
        new()
        {
            CustomerId  = id,
            Name        = name,
            Email       = $"info{id}@test.com",
            DateCreated = DateTime.UtcNow,
            CreatedBy   = "tester",
            CustomerContacts = new List<CustomerContact>
            {
                new() { FirstName = "Alice", LastName = "Smith", Email = $"alice{id}@test.com", DateCreated = DateTime.UtcNow, CreatedBy = "tester" }
            },
        };

    // ── GetAllCustomersWithContacts ───────────────────────────────────────────

    [Fact]
    public async Task GetAllCustomersWithContactsAsync_ReturnsAllCustomersWithContacts()
    {
        using var ctx = DbContextFactory.Create();
        ctx.Customers.AddRange(MakeCustomer(1, "Acme"), MakeCustomer(2, "Beta Corp"));
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        var result = (await svc.GetAllCustomersWithContactsAsync()).ToList();

        result.Should().HaveCount(2);
        result.Should().AllSatisfy(c => c.CustomerContacts.Should().NotBeEmpty());
    }

    // ── GetCustomerById ───────────────────────────────────────────────────────

    [Fact]
    public async Task GetCustomerByIdAsync_WhenFound_ReturnsCustomerWithContacts()
    {
        using var ctx = DbContextFactory.Create();
        ctx.Customers.Add(MakeCustomer(1));
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        var result = await svc.GetCustomerByIdAsync(1);

        result.CustomerId.Should().Be(1);
        result.CustomerContacts.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WhenNotFound_ThrowsKeyNotFoundException()
    {
        using var ctx = DbContextFactory.Create();
        var svc = Build(ctx);

        await svc.Invoking(s => s.GetCustomerByIdAsync(999))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*999*");
    }

    // ── SaveCompanyWithContact ────────────────────────────────────────────────

    [Fact]
    public async Task SaveCompanyWithContact_PersistsCustomerAndContacts()
    {
        using var ctx = DbContextFactory.Create();
        var svc = Build(ctx);

        var dto = new CompanyWithContactDto
        {
            Name  = "New Corp",
            Email = "contact@new.com",
            Contacts = new List<CustomerContactDto>
            {
                new() { FirstName = "Bob", LastName = "Jones", Email = "bob@new.com", Phone = "01234" },
            },
        };

        var result = await svc.SaveCompanyWithContact(dto, "tester");

        result.CustomerId.Should().BeGreaterThan(0);
        result.Name.Should().Be("New Corp");
        result.CustomerContacts.Should().HaveCount(1);
        result.CreatedBy.Should().Be("tester");
    }

    [Fact]
    public async Task SaveCompanyWithContact_WithNoContacts_PersistsCustomerOnly()
    {
        using var ctx = DbContextFactory.Create();
        var svc = Build(ctx);

        var dto = new CompanyWithContactDto
        {
            Name  = "Solo Corp",
            Email = "solo@corp.com",
            Contacts = new List<CustomerContactDto>(),
        };

        var result = await svc.SaveCompanyWithContact(dto, "tester");

        result.CustomerContacts.Should().BeEmpty();
    }

    // ── SaveContactToCompany ──────────────────────────────────────────────────

    [Fact]
    public async Task SaveContactToCompany_AddsContactWithAuditFields()
    {
        using var ctx = DbContextFactory.Create();
        ctx.Customers.Add(MakeCustomer(1));
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        var dto = new ContactDto
        {
            CompanyId = 1,
            FirstName = "Carol",
            LastName  = "White",
            Email     = "carol@test.com",
            Phone     = "0871234567",
        };

        var result = await svc.SaveContactToCompany(dto, "tester");

        result.CustomerId.Should().Be(1);
        result.FirstName.Should().Be("Carol");
        result.CreatedBy.Should().Be("tester");
    }

    // ── UpdateCompany ─────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateCompany_WhenFound_UpdatesNameAndFields()
    {
        using var ctx = DbContextFactory.Create();
        ctx.Customers.Add(MakeCustomer(1, "Old Name"));
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        var dto = new CompanyDto { Name = "New Name", Email = "new@corp.com" };

        var result = await svc.UpdateCompany(1, dto, "editor");

        result.Name.Should().Be("New Name");
        result.UpdatedBy.Should().Be("editor");
    }

    [Fact]
    public async Task UpdateCompany_WhenNotFound_ThrowsKeyNotFoundException()
    {
        using var ctx = DbContextFactory.Create();
        var svc = Build(ctx);

        await svc.Invoking(s => s.UpdateCompany(999, new CompanyDto { Name = "X" }, "user"))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*999*");
    }

    // ── DeleteCompanyWithContact ──────────────────────────────────────────────

    [Fact]
    public async Task DeleteCompanyWithContact_WhenFound_RemovesCustomerAndReturnsTrue()
    {
        using var ctx = DbContextFactory.Create();
        ctx.Customers.Add(MakeCustomer(1));
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        var result = await svc.DeleteCompanyWithContact(1, "admin");

        result.Should().BeTrue();
        ctx.Customers.Any(c => c.CustomerId == 1).Should().BeFalse();
    }

    [Fact]
    public async Task DeleteCompanyWithContact_WhenNotFound_ThrowsKeyNotFoundException()
    {
        using var ctx = DbContextFactory.Create();
        var svc = Build(ctx);

        await svc.Invoking(s => s.DeleteCompanyWithContact(999, "admin"))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*999*");
    }
}
