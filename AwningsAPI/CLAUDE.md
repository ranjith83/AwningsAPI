# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build

# Run (HTTP)
dotnet run --launch-profile http   # http://localhost:5281

# Run (HTTPS)
dotnet run --launch-profile https  # https://localhost:7184

# Apply EF Core migrations
dotnet ef database update

# Add a new migration
dotnet ef migrations add <MigrationName>
```

Swagger UI is available at `/swagger` when running in Development mode.

## Architecture

This is a .NET 9 ASP.NET Core Web API for managing the sales and operations workflow of an awnings business (Awnings of Ireland). The Angular frontend runs on `localhost:4200`.

### Request Flow

```
Controllers → Services (via injected interfaces) → AppDbContext → SQL Server
                                                 ↑
                                         AuditInterceptor (SaveChanges hook)
```

### Domain & Workflow

The core business process is a multi-stage sales pipeline:

**InitialEnquiry → Quote → SiteVisit → Invoice → Payment**

Each stage is managed by its corresponding service. The `WorkflowService` orchestrates transitions between stages and handles product pricing. `FollowUpService` manages scheduled follow-ups within a workflow.

### Key Layers

- **Controllers/** — Thin controllers; authentication via JWT `[Authorize]` attributes
- **Interfaces/** — Service contracts (ICustomerService, IQuoteService, etc.)
- **Services/** — All business logic; services are registered as **scoped** in `Program.cs`
- **Model/** — EF Core entities (35+ models)
- **Dto/** — API request/response shapes; services map between models and DTOs
- **Database/AppDbContext.cs** — Single DbContext with 30+ DbSets; acts as unit of work
- **Interceptor/AuditInterceptor.cs** — `SaveChangesInterceptor` that automatically creates field-level audit log entries for Customer, Contact, Workflow, Quote, Invoice, and SiteVisit changes

### Email Integration (Microsoft Graph)

`EmailWatcherBackgroundService` (IHostedService) polls every hour to maintain active Microsoft Graph webhook subscriptions. When emails arrive:

1. `GraphSubscriptionService` manages subscription lifecycle (72-hour expiry)
2. `EmailReaderService` fetches emails via Graph API
3. `EmailAnalysisService` runs AI analysis — provider is configurable (`OpenAI`, `Claude`, `AzureOpenAI`) in `appsettings.json`
4. `EmailProcessorService` routes the result into the workflow

### Authentication

JWT with 24-hour access tokens and 7-day rotating refresh tokens. Issuer: `AwningsAPI`, Audience: `AwningsAngularApp`. Passwords hashed with BCrypt. `HttpContextAccessor` is injected into `AuditInterceptor` to record the acting user on audit logs.

### Configuration Keys (appsettings.json)

| Section | Purpose |
|---|---|
| `ConnectionStrings:DefaultConnection` | Primary SQL Server database |
| `JwtSettings` | SecretKey, Issuer, Audience, ExpirationInHours |
| `AzureAd` | TenantId, ClientId, ClientSecret for Microsoft Graph |
| `AI Provider` | `OpenAI` / `Claude` / `AzureOpenAI` |
| `GraphSubscription:NotificationUrl` | Webhook endpoint for incoming email events |
| `EmailConfiguration:IsProd` | When false, redirects emails to `TestMailAddress` |
