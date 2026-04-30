using AwningsAPI.Dto.Auth;
using AwningsAPI.Model.Auth;
using AwningsAPI.Services.Auth;
using AwningsAPI.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace AwningsAPI.Tests.Unit;

public class AuthServiceTests
{
    // ── Shared helpers ────────────────────────────────────────────────────────

    private const string TestPassword = "TestPass123!";

    private static IConfiguration BuildConfig() =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:SecretKey"]           = "test-secret-key-long-enough-for-hmacsha256-32bytes!",
                ["JwtSettings:Issuer"]              = "AwningsAPI",
                ["JwtSettings:Audience"]            = "AwningsAngularApp",
                ["JwtSettings:ExpirationInHours"]   = "24",
            })
            .Build();

    private static User CreateActiveUser(int id = 1, string username = "testuser", string email = "test@awnings.ie") =>
        new()
        {
            UserId       = id,
            FirstName    = "Test",
            LastName     = "User",
            Email        = email,
            Username     = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(TestPassword),
            IsActive     = true,
            DateCreated  = DateTime.UtcNow,
            CreatedBy    = "System",
        };

    // ── Login ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsAuthResponse()
    {
        using var ctx = DbContextFactory.Create();
        ctx.Users.Add(CreateActiveUser());
        await ctx.SaveChangesAsync();

        var svc = new AuthService(ctx, BuildConfig());

        var result = await svc.LoginAsync(
            new LoginDto { Username = "testuser", Password = TestPassword }, "127.0.0.1");

        result.Should().NotBeNull();
        result.Username.Should().Be("testuser");
        result.Token.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_ThrowsUnauthorizedException()
    {
        using var ctx = DbContextFactory.Create();
        ctx.Users.Add(CreateActiveUser());
        await ctx.SaveChangesAsync();

        var svc = new AuthService(ctx, BuildConfig());

        await svc.Invoking(s => s.LoginAsync(
                new LoginDto { Username = "testuser", Password = "WrongPassword!" }, "127.0.0.1"))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*Invalid username or password*");
    }

    [Fact]
    public async Task LoginAsync_WithInactiveUser_ThrowsUnauthorizedException()
    {
        using var ctx = DbContextFactory.Create();
        var user = CreateActiveUser();
        user.IsActive = false;
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();

        var svc = new AuthService(ctx, BuildConfig());

        await svc.Invoking(s => s.LoginAsync(
                new LoginDto { Username = "testuser", Password = TestPassword }, "127.0.0.1"))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task LoginAsync_WithUnknownUsername_ThrowsUnauthorizedException()
    {
        using var ctx = DbContextFactory.Create();
        var svc = new AuthService(ctx, BuildConfig());

        await svc.Invoking(s => s.LoginAsync(
                new LoginDto { Username = "nobody", Password = TestPassword }, "127.0.0.1"))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    // ── Register ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task RegisterAsync_WithNewUser_ReturnsAuthResponse()
    {
        using var ctx = DbContextFactory.Create();
        var svc = new AuthService(ctx, BuildConfig());

        var result = await svc.RegisterAsync(new RegisterDto
        {
            FirstName       = "Jane",
            LastName        = "Doe",
            Email           = "jane@awnings.ie",
            Username        = "janedoe",
            Password        = TestPassword,
            ConfirmPassword = TestPassword,
        });

        result.Username.Should().Be("janedoe");
        result.Token.Should().NotBeNullOrWhiteSpace();
        ctx.Users.Any(u => u.Username == "janedoe").Should().BeTrue();
    }

    [Fact]
    public async Task RegisterAsync_WithDuplicateUsername_ThrowsInvalidOperationException()
    {
        using var ctx = DbContextFactory.Create();
        ctx.Users.Add(CreateActiveUser(username: "janedoe"));
        await ctx.SaveChangesAsync();

        var svc = new AuthService(ctx, BuildConfig());

        await svc.Invoking(s => s.RegisterAsync(new RegisterDto
            {
                FirstName = "Other", LastName = "User",
                Email = "other@awnings.ie", Username = "janedoe",
                Password = TestPassword, ConfirmPassword = TestPassword,
            }))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Username already exists*");
    }

    [Fact]
    public async Task RegisterAsync_WithDuplicateEmail_ThrowsInvalidOperationException()
    {
        using var ctx = DbContextFactory.Create();
        ctx.Users.Add(CreateActiveUser(email: "shared@awnings.ie"));
        await ctx.SaveChangesAsync();

        var svc = new AuthService(ctx, BuildConfig());

        await svc.Invoking(s => s.RegisterAsync(new RegisterDto
            {
                FirstName = "Other", LastName = "User",
                Email = "shared@awnings.ie", Username = "otherusername",
                Password = TestPassword, ConfirmPassword = TestPassword,
            }))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Email already exists*");
    }

    // ── Refresh Token ─────────────────────────────────────────────────────────

    [Fact]
    public async Task RefreshTokenAsync_WithValidToken_ReturnsNewTokenPair()
    {
        using var ctx = DbContextFactory.Create();
        var user = CreateActiveUser();
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();

        var token = new RefreshToken
        {
            UserId    = user.UserId,
            Token     = "valid-refresh-token",
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            User      = user,
        };
        ctx.RefreshTokens.Add(token);
        await ctx.SaveChangesAsync();

        var svc = new AuthService(ctx, BuildConfig());
        var result = await svc.RefreshTokenAsync("valid-refresh-token", "127.0.0.1");

        result.Token.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBe("valid-refresh-token");
        ctx.RefreshTokens.First(t => t.Token == "valid-refresh-token").IsRevoked.Should().BeTrue();
    }

    [Fact]
    public async Task RefreshTokenAsync_WithRevokedToken_ThrowsUnauthorizedException()
    {
        using var ctx = DbContextFactory.Create();
        var user = CreateActiveUser();
        ctx.Users.Add(user);
        ctx.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.UserId, Token = "revoked-token",
            ExpiresAt = DateTime.UtcNow.AddDays(7), IsRevoked = true, User = user,
        });
        await ctx.SaveChangesAsync();

        var svc = new AuthService(ctx, BuildConfig());

        await svc.Invoking(s => s.RefreshTokenAsync("revoked-token", "127.0.0.1"))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*Invalid or expired refresh token*");
    }

    [Fact]
    public async Task RefreshTokenAsync_WithExpiredToken_ThrowsUnauthorizedException()
    {
        using var ctx = DbContextFactory.Create();
        var user = CreateActiveUser();
        ctx.Users.Add(user);
        ctx.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.UserId, Token = "expired-token",
            ExpiresAt = DateTime.UtcNow.AddDays(-1), IsRevoked = false, User = user,
        });
        await ctx.SaveChangesAsync();

        var svc = new AuthService(ctx, BuildConfig());

        await svc.Invoking(s => s.RefreshTokenAsync("expired-token", "127.0.0.1"))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    // ── Change Password ───────────────────────────────────────────────────────

    [Fact]
    public async Task ChangePasswordAsync_WithCorrectCurrentPassword_UpdatesPasswordHash()
    {
        using var ctx = DbContextFactory.Create();
        var user = CreateActiveUser();
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();

        var svc = new AuthService(ctx, BuildConfig());
        var result = await svc.ChangePasswordAsync(user.UserId, new ChangePasswordDto
        {
            CurrentPassword    = TestPassword,
            NewPassword        = "NewPassword456!",
            ConfirmNewPassword = "NewPassword456!",
        });

        result.Should().BeTrue();
        var updated = ctx.Users.Find(user.UserId)!;
        BCrypt.Net.BCrypt.Verify("NewPassword456!", updated.PasswordHash).Should().BeTrue();
    }

    [Fact]
    public async Task ChangePasswordAsync_WithIncorrectCurrentPassword_ThrowsUnauthorizedException()
    {
        using var ctx = DbContextFactory.Create();
        var user = CreateActiveUser();
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();

        var svc = new AuthService(ctx, BuildConfig());

        await svc.Invoking(s => s.ChangePasswordAsync(user.UserId, new ChangePasswordDto
            {
                CurrentPassword    = "WrongCurrentPass!",
                NewPassword        = "New456!",
                ConfirmNewPassword = "New456!",
            }))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*Current password is incorrect*");
    }
}
