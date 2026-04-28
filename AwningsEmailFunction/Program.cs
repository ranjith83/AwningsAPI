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
        services.AddScoped<IEmailWatchService, EmailWatchService>();

        services.AddHttpClient();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

await host.RunAsync();
