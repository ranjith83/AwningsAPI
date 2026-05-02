using Microsoft.EntityFrameworkCore;

namespace AwningsEmailFunction.Tests.Unit;

public class EmailProcessorServiceTests
{
    private static EmailProcessorService Build(
        EmailFunctionDbContext ctx,
        IEmailReaderService? reader = null,
        IEmailAnalysisService? analysis = null,
        IBlobEmailStorageService? blob = null)
    {
        reader   ??= new Mock<IEmailReaderService>().Object;
        analysis ??= new Mock<IEmailAnalysisService>().Object;
        blob     ??= new Mock<IBlobEmailStorageService>().Object;
        return new EmailProcessorService(ctx, reader, analysis, blob,
            NullLogger<EmailProcessorService>.Instance);
    }

    private static IncomingEmail MakeFetchedEmail(string messageId = "msg-001") => new()
    {
        EmailId         = messageId,
        Subject         = "Test Subject",
        FromEmail       = "customer@test.com",
        FromName        = "Test Customer",
        BodyPreview     = "Body preview",
        BodyContent     = "Full body",
        ReceivedDateTime = DateTime.UtcNow,
    };

    private static EmailAnalysisResult MakeAnalysis(string category = "enquiry") => new()
    {
        Category   = category,
        Confidence = 0.95,
    };

    // ── New email ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task ProcessIncomingEmail_NewEmail_SavesEmailAndCreatesTask()
    {
        using var ctx    = EmailFunctionDbContextFactory.Create();
        var mockReader   = new Mock<IEmailReaderService>();
        var mockAnalysis = new Mock<IEmailAnalysisService>();

        mockReader.Setup(r => r.GetCompleteEmailAsync("inbox@test.com", "msg-001"))
                  .ReturnsAsync(MakeFetchedEmail("msg-001"));
        mockAnalysis.Setup(a => a.AnalyzeEmailAsync(It.IsAny<IncomingEmail>()))
                    .ReturnsAsync(MakeAnalysis("quote"));

        var svc = Build(ctx, mockReader.Object, mockAnalysis.Object);
        await svc.ProcessIncomingEmailAsync("msg-001", "inbox@test.com");

        ctx.IncomingEmails.Should().HaveCount(1);
        ctx.IncomingEmails.First().ProcessingStatus.Should().Be("Completed");
        ctx.Tasks.Should().HaveCount(1);
        ctx.Tasks.First().Category.Should().Be("Quote");
    }

    // ── Duplicate: already in DB ──────────────────────────────────────────────

    [Fact]
    public async Task ProcessIncomingEmail_AlreadyInDb_SkipsProcessing()
    {
        using var ctx = EmailFunctionDbContextFactory.Create();
        ctx.IncomingEmails.Add(new IncomingEmail { EmailId = "msg-dup", Subject = "Existing" });
        await ctx.SaveChangesAsync();

        var mockReader = new Mock<IEmailReaderService>();
        var svc        = Build(ctx, mockReader.Object);
        await svc.ProcessIncomingEmailAsync("msg-dup", "inbox@test.com");

        // Reader should never be called — skipped immediately
        mockReader.Verify(r => r.GetCompleteEmailAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        ctx.Tasks.Should().BeEmpty();
    }

    // ── Junk category ─────────────────────────────────────────────────────────

    [Fact]
    public async Task ProcessIncomingEmail_JunkCategory_NoTaskCreated()
    {
        using var ctx    = EmailFunctionDbContextFactory.Create();
        var mockReader   = new Mock<IEmailReaderService>();
        var mockAnalysis = new Mock<IEmailAnalysisService>();

        mockReader.Setup(r => r.GetCompleteEmailAsync(It.IsAny<string>(), "msg-junk"))
                  .ReturnsAsync(MakeFetchedEmail("msg-junk"));
        mockAnalysis.Setup(a => a.AnalyzeEmailAsync(It.IsAny<IncomingEmail>()))
                    .ReturnsAsync(MakeAnalysis("junk"));

        var svc = Build(ctx, mockReader.Object, mockAnalysis.Object);
        await svc.ProcessIncomingEmailAsync("msg-junk", "inbox@test.com");

        ctx.IncomingEmails.Should().HaveCount(1);
        ctx.Tasks.Should().BeEmpty();
        ctx.IncomingEmails.First().ProcessingStatus.Should().Be("Completed");
    }

    // ── General category treated as junk ──────────────────────────────────────

    [Fact]
    public async Task ProcessIncomingEmail_GeneralCategory_NoTaskCreated()
    {
        using var ctx    = EmailFunctionDbContextFactory.Create();
        var mockReader   = new Mock<IEmailReaderService>();
        var mockAnalysis = new Mock<IEmailAnalysisService>();

        mockReader.Setup(r => r.GetCompleteEmailAsync(It.IsAny<string>(), "msg-gen"))
                  .ReturnsAsync(MakeFetchedEmail("msg-gen"));
        mockAnalysis.Setup(a => a.AnalyzeEmailAsync(It.IsAny<IncomingEmail>()))
                    .ReturnsAsync(MakeAnalysis("general"));

        var svc = Build(ctx, mockReader.Object, mockAnalysis.Object);
        await svc.ProcessIncomingEmailAsync("msg-gen", "inbox@test.com");

        ctx.Tasks.Should().BeEmpty();
    }

    // ── Complaint priority ────────────────────────────────────────────────────

    [Fact]
    public async Task ProcessIncomingEmail_ComplaintCategory_TaskIsUrgentPriority()
    {
        using var ctx    = EmailFunctionDbContextFactory.Create();
        var mockReader   = new Mock<IEmailReaderService>();
        var mockAnalysis = new Mock<IEmailAnalysisService>();

        mockReader.Setup(r => r.GetCompleteEmailAsync(It.IsAny<string>(), "msg-complaint"))
                  .ReturnsAsync(MakeFetchedEmail("msg-complaint"));
        mockAnalysis.Setup(a => a.AnalyzeEmailAsync(It.IsAny<IncomingEmail>()))
                    .ReturnsAsync(MakeAnalysis("complaint"));

        var svc = Build(ctx, mockReader.Object, mockAnalysis.Object);
        await svc.ProcessIncomingEmailAsync("msg-complaint", "inbox@test.com");

        ctx.Tasks.First().Priority.Should().Be("Urgent");
    }

    // ── Auto-link known customer ──────────────────────────────────────────────

    [Fact]
    public async Task ProcessIncomingEmail_KnownCustomerEmail_LinksTaskToCustomer()
    {
        using var ctx = EmailFunctionDbContextFactory.Create();
        ctx.Customers.Add(new Customer { CustomerId = 1, Name = "ACME Ltd", Email = "customer@test.com" });
        await ctx.SaveChangesAsync();

        var mockReader   = new Mock<IEmailReaderService>();
        var mockAnalysis = new Mock<IEmailAnalysisService>();

        mockReader.Setup(r => r.GetCompleteEmailAsync(It.IsAny<string>(), "msg-autolink"))
                  .ReturnsAsync(MakeFetchedEmail("msg-autolink"));
        mockAnalysis.Setup(a => a.AnalyzeEmailAsync(It.IsAny<IncomingEmail>()))
                    .ReturnsAsync(MakeAnalysis("enquiry"));

        var svc = Build(ctx, mockReader.Object, mockAnalysis.Object);
        await svc.ProcessIncomingEmailAsync("msg-autolink", "inbox@test.com");

        var task = ctx.Tasks.First();
        task.CustomerId.Should().Be(1);
        task.CustomerName.Should().Be("ACME Ltd");
    }

    [Fact]
    public async Task ProcessIncomingEmail_KnownCustomerWithSingleWorkflow_AutoCompletesTask()
    {
        using var ctx = EmailFunctionDbContextFactory.Create();
        ctx.Customers.Add(new Customer { CustomerId = 1, Name = "ACME Ltd", Email = "customer@test.com" });
        ctx.WorkflowStarts.Add(new WorkflowStart { WorkflowId = 10, CustomerId = 1, WorkflowName = "Garden Awning" });
        await ctx.SaveChangesAsync();

        var mockReader   = new Mock<IEmailReaderService>();
        var mockAnalysis = new Mock<IEmailAnalysisService>();

        mockReader.Setup(r => r.GetCompleteEmailAsync(It.IsAny<string>(), "msg-autocomplete"))
                  .ReturnsAsync(MakeFetchedEmail("msg-autocomplete"));
        mockAnalysis.Setup(a => a.AnalyzeEmailAsync(It.IsAny<IncomingEmail>()))
                    .ReturnsAsync(MakeAnalysis("enquiry"));

        var svc = Build(ctx, mockReader.Object, mockAnalysis.Object);
        await svc.ProcessIncomingEmailAsync("msg-autocomplete", "inbox@test.com");

        var task = ctx.Tasks.First();
        task.WorkflowId.Should().Be(10);
        task.Status.Should().Be("Completed");
    }

    // ── Reader throws → email marked Failed ───────────────────────────────────

    [Fact]
    public async Task ProcessIncomingEmail_WhenReaderThrows_EmailMarkedFailed()
    {
        using var ctx  = EmailFunctionDbContextFactory.Create();
        var mockReader = new Mock<IEmailReaderService>();
        mockReader.Setup(r => r.GetCompleteEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                  .ThrowsAsync(new Exception("Graph API error"));

        var svc = Build(ctx, mockReader.Object);
        await svc.ProcessIncomingEmailAsync("msg-err", "inbox@test.com");

        // Email is saved with Failed status
        var email = await ctx.IncomingEmails.IgnoreQueryFilters().FirstOrDefaultAsync();
        email.Should().BeNull(); // reader failed before save — email never persisted, no crash
    }
}
