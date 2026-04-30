using AwningsAPI.Database;

namespace AwningsAPI.Tests.Helpers;

/// <summary>
/// Public xUnit fixture that wraps the internal <see cref="TestWebApplicationFactory"/>.
/// Keeps WebApplicationFactory&lt;Program&gt; (which inherits Program's internal
/// accessibility) hidden behind a public API so test classes can remain public.
/// </summary>
public sealed class WorkflowApiFixture : IDisposable
{
    private readonly TestWebApplicationFactory _factory = new();

    public HttpClient CreateClient() => _factory.CreateClient();

    public AppDbContext GetDbContext() => _factory.GetDbContext();

    public static string GenerateTestJwt(string username = "testuser", string role = "Admin") =>
        TestWebApplicationFactory.GenerateTestJwt(username, role);

    public void Dispose() => _factory.Dispose();
}
