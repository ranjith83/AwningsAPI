using Anthropic;
using AwningsAPI.Database;
using AwningsAPI.Hubs;
using AwningsAPI.Interceptors;
using AwningsAPI.Interfaces;
using AwningsAPI.Middleware;
using AwningsAPI.Services;
using AwningsAPI.Services.AuditLogService;
using AwningsAPI.Services.Auth;
using AwningsAPI.Services.ConfigurationService;
using AwningsAPI.Services.CustomerService;
using AwningsAPI.Services.Email;
using AwningsAPI.Services.OutlookService;
using AwningsAPI.Services.QuoteService;
using AwningsAPI.Services.SiteVisitService;
using AwningsAPI.Services.Suppliers;
using AwningsAPI.Services.Tasks;
using AwningsAPI.Services.WorkflowService;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<IQuoteService, QuoteService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISiteVisitService, SiteVisitService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuditInterceptor>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IPaymentScheduleService, PaymentScheduleService>();
builder.Services.AddScoped<IOutlookService, OutlookService>();
//builder.Services.AddScoped<IFollowUpService, FollowUpService>();
builder.Services.AddScoped<FollowUpService>();

builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<IOptionLookupService, OptionLookupService>();
builder.Services.AddScoped<IEmailAutoReplyService, EmailAutoReplyService>();

// Configure HttpClient
builder.Services.AddHttpClient();

// Anthropic SDK client for Claude AI
builder.Services.AddSingleton<AnthropicClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new AnthropicClient { ApiKey = configuration["Claude:ApiKey"] ?? "" };
});


// Register Microsoft Graph Service Client as Singleton
builder.Services.AddSingleton<GraphServiceClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var tenantId = configuration["AzureAd:TenantId"];
    var clientId = configuration["AzureAd:ClientId"];
    var clientSecret = configuration["AzureAd:ClientSecret"];

    var options = new ClientSecretCredentialOptions
    {
        AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
    };

    var clientSecretCredential = new ClientSecretCredential(
        tenantId, clientId, clientSecret, options);

    return new GraphServiceClient(clientSecretCredential);
});

// Email Services
builder.Services.AddScoped<IEmailReaderService, EmailReaderService>();
builder.Services.AddScoped<IEmailAnalysisService, EmailAnalysisService>();
builder.Services.AddScoped<IEmailProcessorService, EmailProcessorService>();
builder.Services.AddScoped<IGraphSubscriptionService, GraphSubscriptionService>();
//builder.Services.AddHostedService<EmailWatcherBackgroundService>();
// Task Service
builder.Services.AddScoped<ITaskService, TaskService>();

// JWT Authentication Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey!)),
        ClockSkew = TimeSpan.Zero
    };
    // SignalR WebSocket connections pass the JWT via query string
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(token) &&
                context.HttpContext.Request.Path.StartsWithSegments("/hubs"))
                context.Token = token;
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddSignalR();
builder.Services.AddAuthorization();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 1000;
    options.CompactionPercentage = 0.25;
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddResponseCompression(options => options.EnableForHttps = true);
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    options.UseSqlServer(
               builder.Configuration.GetConnectionString("DefaultConnection"),
               sql => sql.EnableRetryOnFailure())
           .AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Awnings Of Ireland API", Version = "v1" });
});

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? ["http://localhost:4200"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        policy => policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()           // includes OPTIONS
            .AllowCredentials());       // REQUIRED for [Authorize] endpoints
});



var app = builder.Build();

// Migrations are applied by the CI pipeline (ci-dev.yml / ci-prod.yml)
// using 'dotnet ef database update' before deployment.
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     db.Database.Migrate();
// }

app.UseCors("AllowAngularDev");
app.UseResponseCompression();
app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");
app.MapHealthChecks("/health");

app.Run();
