using AwningsAPI.Database;
using AwningsAPI.Interceptors;
using AwningsAPI.Interfaces;
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
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()          // Enables ASP.NET Core pipeline inside Functions
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        // ── Controllers — scan the existing AwningsAPI assembly ──────────────
        services.AddControllers()
            .AddApplicationPart(typeof(AwningsAPI.Controllers.SiteVisitController).Assembly);

        // ── Business services (identical to existing Program.cs) ─────────────
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IWorkflowService, WorkflowService>();
        services.AddScoped<IQuoteService, QuoteService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISiteVisitService, SiteVisitService>();
        services.AddHttpContextAccessor();
        services.AddScoped<AuditInterceptor>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<IPaymentScheduleService, PaymentScheduleService>();
        services.AddScoped<IOutlookService, OutlookService>();
        services.AddScoped<FollowUpService>();
        services.AddScoped<IConfigurationService, ConfigurationService>();
        services.AddScoped<ITaskService, TaskService>();

        // ── HttpClient ───────────────────────────────────────────────────────
        services.AddHttpClient();

        // ── Microsoft Graph ──────────────────────────────────────────────────
        services.AddSingleton<GraphServiceClient>(sp =>
        {
            var tenantId     = configuration["AzureAd:TenantId"];
            var clientId     = configuration["AzureAd:ClientId"];
            var clientSecret = configuration["AzureAd:ClientSecret"];

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret,
                new ClientSecretCredentialOptions
                {
                    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
                });

            return new GraphServiceClient(credential);
        });

        // ── Email services ───────────────────────────────────────────────────
        services.AddScoped<IEmailReaderService, EmailReaderService>();
        services.AddScoped<IEmailAnalysisService, EmailAnalysisService>();
        services.AddScoped<IEmailProcessorService, EmailProcessorService>();
        services.AddScoped<IGraphSubscriptionService, GraphSubscriptionService>();
        services.AddHostedService<EmailWatcherBackgroundService>();

        // ── Database ─────────────────────────────────────────────────────────
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"])
                   .AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
        });

        // ── JWT Authentication ────────────────────────────────────────────────
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey   = jwtSettings["SecretKey"];

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer              = jwtSettings["Issuer"],
                ValidAudience            = jwtSettings["Audience"],
                IssuerSigningKey         = new SymmetricSecurityKey(
                                               System.Text.Encoding.UTF8.GetBytes(secretKey!)),
                ClockSkew                = TimeSpan.Zero
            };
        });

        services.AddAuthorization();

        // ── Swagger ───────────────────────────────────────────────────────────
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Awnings API", Version = "v1" });

            // Allow JWT testing from Swagger UI
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter: Bearer {your token}"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
        });

        // ── Application Insights ─────────────────────────────────────────────
        //services.ConfigureFunctionsApplicationInsights();

        // ── CORS (Angular dev) ────────────────────────────────────────────────
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularDev",
                policy => policy
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
        });
        
    })
   
    .Build();

host.Run();
