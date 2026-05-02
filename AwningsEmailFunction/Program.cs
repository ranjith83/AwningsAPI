using AwningsEmailFunction.Database;
using AwningsEmailFunction.Interfaces;
using AwningsEmailFunction.Services;
using Azure.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;

        // Microsoft Graph client — uses same AzureAd credentials as AwningsAPI
        services.AddSingleton<GraphServiceClient>(_ =>
        {
            var credential = new ClientSecretCredential(
                config["AzureAd:TenantId"],
                config["AzureAd:ClientId"],
                config["AzureAd:ClientSecret"]);
            return new GraphServiceClient(credential);
        });

        services.AddDbContext<EmailFunctionDbContext>(options =>
            options.UseSqlServer(config["ConnectionStrings:DefaultConnection"]));

        services.AddScoped<IGraphSubscriptionService, GraphSubscriptionService>();
        services.AddScoped<IEmailReaderService, EmailReaderService>();
        services.AddScoped<IEmailAnalysisService, EmailAnalysisService>();
        services.AddScoped<IEmailProcessorService, EmailProcessorService>();
        services.AddScoped<IEmailWatchService, EmailWatchService>();
        services.AddSingleton<IBlobEmailStorageService, BlobEmailStorageService>();

        services.AddHttpClient();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<EmailFunctionDbContext>();
        db.Database.EnsureCreated();

        db.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE name='GraphSubscriptions' AND xtype='U')
            CREATE TABLE GraphSubscriptions (
                Id          INT IDENTITY(1,1) PRIMARY KEY,
                SubscriptionId NVARCHAR(255) NOT NULL,
                ExpiryDateTime DATETIMEOFFSET NOT NULL,
                UpdatedAt   DATETIME2 NOT NULL
            )");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[STARTUP ERROR] Database initialization failed: {ex.Message}");
    }
}

await host.RunAsync();
