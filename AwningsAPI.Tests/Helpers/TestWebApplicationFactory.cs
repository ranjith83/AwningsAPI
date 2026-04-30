using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AwningsAPI.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AwningsAPI.Tests.Helpers;

/// <summary>
/// Spins up the real ASP.NET Core pipeline with:
///   - InMemory database (no SQL Server required)
///   - Dummy Azure AD config (no real Graph credentials)
///   - Background hosted services removed (no email watcher)
///   - Fixed JWT secret matching <see cref="GenerateTestJwt"/>
/// </summary>
internal class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();
    private readonly InMemoryDatabaseRoot _dbRoot = new();

    internal const string JwtSecret   = "test-secret-key-long-enough-for-hmacsha256-32bytes!";
    internal const string JwtIssuer   = "AwningsAPI";
    internal const string JwtAudience = "AwningsAngularApp";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"]       = "InMemory",
                ["JwtSettings:SecretKey"]                     = JwtSecret,
                ["JwtSettings:Issuer"]                        = JwtIssuer,
                ["JwtSettings:Audience"]                      = JwtAudience,
                ["JwtSettings:ExpirationInHours"]             = "24",
                ["AzureAd:TenantId"]                          = "00000000-0000-0000-0000-000000000000",
                ["AzureAd:ClientId"]                          = "00000000-0000-0000-0000-000000000000",
                ["AzureAd:ClientSecret"]                      = "test",
                ["GraphSubscription:NotificationUrl"]         = "https://localhost/webhook",
                ["EmailConfiguration:IsProd"]                 = "false",
                ["EmailConfiguration:TestMailAddress"]        = "test@test.com",
                ["Serilog:MinimumLevel:Default"]              = "Warning",
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remove every EF Core registration that references SQL Server so the
            // InMemory provider does not conflict with the existing SQL Server one.
            // The (sp, options) => lambda form of AddDbContext also stores its
            // configuration in IDbContextOptionsConfiguration<T>, so remove that too.
            var toRemove = services
                .Where(d =>
                    d.ServiceType == typeof(AppDbContext) ||
                    d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    d.ServiceType.IsGenericType &&
                    d.ServiceType.GetGenericTypeDefinition().FullName ==
                        "Microsoft.EntityFrameworkCore.Infrastructure.IDbContextOptionsConfiguration`1" &&
                    d.ServiceType.GenericTypeArguments[0] == typeof(AppDbContext))
                .ToList();

            foreach (var d in toRemove)
                services.Remove(d);

            // Register a clean InMemory DbContext.
            // EnableServiceProviderCaching(false) prevents the shared EF Core
            // internal provider from carrying over the SQL Server registration.
            // _dbRoot ensures all DbContext instances (HTTP request scopes and
            // GetDbContext() calls) share the same in-memory database storage.
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(_dbName, _dbRoot)
                       .EnableServiceProviderCaching(false));

            // Suppress the email background service — it needs live Graph credentials
            services.RemoveAll(typeof(IHostedService));

            // Override JWT validation parameters AFTER they have been bound from
            // configuration.  PostConfigure runs last, so this wins regardless of
            // when configuration was first read in Program.cs.
            services.PostConfigure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret));
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey         = key,
                        ValidateIssuer           = true,
                        ValidIssuer              = JwtIssuer,
                        ValidateAudience         = true,
                        ValidAudience            = JwtAudience,
                        ValidateLifetime         = true,
                        ClockSkew                = TimeSpan.Zero,
                    };
                });
        });
    }

    /// <summary>Returns a scoped DbContext pointed at the test database.</summary>
    public AppDbContext GetDbContext()
    {
        var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    /// <summary>Generates a signed JWT accepted by the test server.</summary>
    public static string GenerateTestJwt(string username = "testuser", string role = "Admin")
    {
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: JwtIssuer,
            audience: JwtAudience,
            claims: new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, $"{username}@test.com"),
                new Claim(ClaimTypes.Role, role),
            },
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
