# Awnings of Ireland — System Architecture

## Overview

A full-stack business operations platform for managing the sales pipeline of an awnings company. The system handles inbound email triage, customer management, quoting, invoicing, site visits, and payments through a multi-stage workflow.

---

## High-Level Architecture

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                          CLIENT LAYER                                       │
│                                                                             │
│   Angular SPA (localhost:4200 / prod domain)                                │
│   ├── Auth (JWT)           ├── Dashboard         ├── Customers              │
│   ├── Email Tasks          ├── Workflow Pipeline  ├── Quotes                │
│   ├── Invoices             ├── Site Visits        ├── Reports               │
│   ├── Follow-ups           ├── Audit History      └── Configuration         │
└──────────────────────────────┬──────────────────────────────────────────────┘
                               │ HTTPS + JWT Bearer
                               ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                          API LAYER                                          │
│                                                                             │
│   AwningsAPI — .NET 9 ASP.NET Core Web API                                  │
│   Azure Container Apps (ghcr.io image)                                      │
│                                                                             │
│   Controllers → Interfaces → Services → AppDbContext → SQL Server           │
│                                       ↑                                     │
│                              AuditInterceptor (SaveChanges hook)            │
└───────────┬──────────────────────────────────────────┬──────────────────────┘
            │ EF Core / SQL                            │ Graph SDK / Blob SDK
            ▼                                          ▼
┌───────────────────────┐              ┌──────────────────────────────────────┐
│   Azure SQL Database  │              │           AZURE SERVICES             │
│                       │              │                                      │
│  Shared by both API   │              │  ┌──────────────────────────────┐    │
│  and Email Function   │              │  │  Azure Blob Storage          │    │
│  (same connection     │              │  │  Container: awnings-emails   │    │
│   string)             │              │  │  Auth: Managed Identity      │    │
└───────────────────────┘              │  │  • bodies/{emailId}.html     │    │
                                       │  │  • attachments/{eId}/{aId}/  │    │
                                       │  └──────────────────────────────┘    │
                                       │                                      │
                                       │  ┌──────────────────────────────┐    │
                                       │  │  Microsoft Graph API         │    │
                                       │  │  • Read mailbox emails       │    │
                                       │  │  • Webhook subscriptions     │    │
                                       │  │  • Send emails               │    │
                                       │  └──────────────────────────────┘    │
                                       │                                      │
                                       │  ┌──────────────────────────────┐    │
                                       │  │  AI Analysis                 │    │
                                       │  │  Provider: OpenAI / Claude / │    │
                                       │  │  AzureOpenAI (configurable)  │    │
                                       │  └──────────────────────────────┘    │
                                       └──────────────────────────────────────┘
                                                        ▲
┌─────────────────────────────────────────────────────────────────────────────┐
│                       EMAIL PROCESSING LAYER                                │
│                                                                             │
│   AwningsEmailFunction — Azure Functions v4 (.NET 8 isolated worker)        │
│                                                                             │
│   ┌─────────────────┐   ┌──────────────────┐   ┌────────────────────────┐  │
│   │ GraphWebhook    │   │ EmailProcessor   │   │ SubscriptionRenewal    │  │
│   │ Function        │──▶│ Function         │   │ Function               │  │
│   │ HTTP Trigger    │   │ Queue Trigger    │   │ Timer (every 12 hrs)   │  │
│   │ GET/POST        │   │ email-processing │   │                        │  │
│   │ /EmailWatch/    │   │ queue            │   │ Renews Graph webhook   │  │
│   │ notify          │   │                  │   │ before 3-day expiry    │  │
│   └─────────────────┘   └──────────────────┘   └────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## Component Detail

### 1. Angular Frontend

**Location:** `awnings-ireland/src`  
**Runtime:** Browser SPA, Angular with standalone components

#### Routes

| Path | Component | Purpose |
|---|---|---|
| `/login` | LoginComponent | Public — JWT login |
| `/dashboard` | DashboardComponent | KPI overview |
| `/customers` | CustomerDetails | CRM — customer list & detail |
| `/workflow/list` | WorkflowListComponent | All active workflows |
| `/workflow/initial-enquiry` | InitialEnquiryComponent | Stage 1 of pipeline |
| `/workflow/create-quote` | CreateQuoteComponent | Stage 2 — quotation |
| `/workflow/invite-showroom` | InviteShowroomComponent | Showroom booking |
| `/workflow/setup-site-visit` | SetupSiteVisitComponent | Stage 3 — site visit |
| `/workflow/final-quote` | FinalQuoteComponent | Revised quote |
| `/workflow/invoice` | InvoiceComponent | Stage 4 — invoice |
| `/workflow/payment` | PaymentComponent | Stage 5 — payment |
| `/task` | TaskComponent | Email task inbox (all tabs) |
| `/reports` | ReportsComponent | Business reports |
| `/audit` | AuditHistoryComponent | Field-level audit log |
| `/followups` | FollowUpListComponent | Scheduled follow-ups |
| `/configuration` | ConfigurationComponent | System configuration |

#### Key Angular Services

| Service | API Target | Purpose |
|---|---|---|
| `AuthService` | `/api/Auth` | Login, refresh token, user info |
| `EmailTaskService` | `/api/EmailTask` | Task CRUD, search, assign, body/attachment proxy |
| `WorkflowService` | `/api/Workflow` | Workflow lifecycle |
| `CustomerService` | `/api/Customer` | Customer CRUD |
| `CreateQuoteService` | `/api/Quote` | Quote creation & pricing |
| `InvoiceService` | `/api/Invoice` | Invoice management |
| `SetupSiteVisitService` | `/api/SiteVisit` | Site visit scheduling |
| `PaymentScheduleService` | `/api/PaymentSchedule` | Payment tracking |
| `AuditTrailService` | `/api/AuditLog` | Audit log queries |
| `FollowUpService` | `/api/Followup` | Follow-up management |
| `OutlookCalendarService` | `/api/Outlook` | Calendar integration |
| `ConfigurationService` | `/api/Configuration` | App config |
| `ReportsService` | `/api/Reports` | Report data |
| `DashboardService` | `/api/Dashboard` | Dashboard KPIs |

#### Email Task Component Flow

```
Double-click row
      │
      ▼
_openEmailViewer(task)
      │
      ├── Show modal immediately with summary data
      ├── GET /api/EmailTask/{taskId}       → full task (attachments, history)
      ├── GET /api/EmailTask/{taskId}/body  → HTML body (blob proxy)
      └── POST /api/EmailTask/{taskId}/read → log read to TaskHistories
```

**Email body rendering:**  
HTML is fetched from backend proxy → wrapped in a `Blob` → set as `<iframe src="blob:...">` via `DomSanitizer.bypassSecurityTrustResourceUrl`. This prevents Angular from stripping inline `data:image` URIs that represent embedded images.

---

### 2. AwningsAPI (.NET 9)

**Location:** `AwningsAPI/AwningsAPI`  
**Hosting:** Azure Container Apps, Docker image pushed to `ghcr.io`

#### Request Pipeline

```
Request
  → CORS (AllowAngularDev)
  → UseAuthentication (JWT Bearer)
  → UseAuthorization
  → Controller
  → Service (via injected interface)
  → AppDbContext + AuditInterceptor
  → SQL Server
```

#### Controllers & Responsibilities

| Controller | Route | Responsibility |
|---|---|---|
| `AuthController` | `/api/Auth` | Login, refresh token, register, user list |
| `CustomerController` | `/api/Customer` | Customer & contact CRUD |
| `WorkflowController` | `/api/Workflow` | Workflow create/update, stage transitions |
| `QuoteController` | `/api/Quote` | Quote CRUD, product pricing |
| `InvoiceController` | `/api/Invoice` | Invoice generation |
| `SiteVisitController` | `/api/SiteVisit` | Site visit scheduling & images |
| `PaymentScheduleController` | `/api/PaymentSchedule` | Payment milestones |
| `EmailTaskController` | `/api/EmailTask` | Task search, assign, body/attachment proxy, send email |
| `EmailProcessingController` | `/api/EmailProcessing` | Manual email trigger |
| `EmailWatchController` | `/api/EmailWatch` | Graph webhook (relay to Function) |
| `FollowupController` | `/api/Followup` | Follow-up CRUD |
| `OutlookController` | `/api/Outlook` | Outlook calendar events |
| `AuditLogController` | `/api/AuditLog` | Audit log queries |
| `ProductItemsController` | `/api/ProductItems` | Product catalogue |
| `SupplierController` | `/api/Supplier` | Supplier management |
| `ConfigurationController` | `/api/Configuration` | App settings |

#### Services Layer

```
IAuthService           → JWT generation, BCrypt password hashing, refresh tokens
ICustomerService       → Customer + contact CRUD, eircode lookup
IWorkflowService       → Workflow stages, pricing, status transitions
IQuoteService          → Quote builder, product line items
IInvoiceService        → Invoice creation, payment tracking
ISiteVisitService      → Site visit records, image upload
ITaskService           → Task CRUD, search/filter (ExcludeCategories), audit history
IEmailReaderService    → Microsoft Graph email read/send, CID→data-URI replacement
IEmailAnalysisService  → AI provider routing (OpenAI/Claude/AzureOpenAI)
IEmailProcessorService → Orchestrates fetch→analyse→task→link pipeline
IGraphSubscriptionService → Graph webhook subscription lifecycle
IAuditLogService       → Field-level change log queries
IFollowUpService       → Scheduled follow-up dismiss/create
IPaymentScheduleService → Payment milestone tracking
IOutlookService        → Outlook calendar event management
IConfigurationService  → Dynamic app configuration
```

#### AuditInterceptor

Hooks `SaveChangesInterceptor`. On every save, detects changed properties on:
`Customer, CustomerContact, WorkflowStart, Quote, Invoice, SiteVisit`
and writes a row per changed field to `AuditLogs`.

#### Blob Storage (Email body & attachments)

```
CreateBlobClient(blobUrl)
  │
  ├── BlobStorage:ConnectionString set? → BlobClient(connectionString, …)  [local Azurite]
  └── BlobStorage:AccountUrl set?       → BlobClient(uri, DefaultAzureCredential)  [production]

Production auth: Container App Managed Identity + Storage Blob Data Reader role
```

---

### 3. AwningsEmailFunction (Azure Functions v4, .NET 8)

**Location:** `AwningsAPI/AwningsEmailFunction`  
**Hosting:** Azure Function App, deployed via zip-deploy with ARM bearer token

#### Functions

```
┌──────────────────────────────────────────────────────────────────┐
│  GraphWebhookFunction                                            │
│  ┌────────────────────────────────────────────────────────────┐  │
│  │ GET  /api/EmailWatch/notify  → validation handshake        │  │
│  │ POST /api/EmailWatch/notify  → extract messageIds          │  │
│  │                                → enqueue to               │  │
│  │                                  "email-processing" queue  │  │
│  │ GET  /api/EmailWatch/status  → subscription status         │  │
│  │ POST /api/EmailWatch/subscribe   → manual subscribe        │  │
│  │ DELETE /api/EmailWatch/subscribe → unsubscribe             │  │
│  └────────────────────────────────────────────────────────────┘  │
│                                                                   │
│  EmailProcessorFunction                                          │
│  ┌────────────────────────────────────────────────────────────┐  │
│  │ QueueTrigger: "email-processing"                           │  │
│  │ → EmailWatchService.SaveEmailAsync(messageId)              │  │
│  │   → EmailProcessorService.ProcessIncomingEmailAsync()      │  │
│  │     1. Fetch email from Graph + download attachments       │  │
│  │     2. Replace cid: → data: URIs in HTML body              │  │
│  │     3. Upload body HTML to Blob Storage                    │  │
│  │     4. Upload attachments to Blob Storage                  │  │
│  │     5. Save IncomingEmail + EmailAttachments to DB         │  │
│  │     6. AI analysis → category + confidence                 │  │
│  │     7. Create AppTask                                      │  │
│  │     8. Auto-link Customer + Workflow (by sender email)     │  │
│  │     9. If initial_enquiry → create InitialEnquiry record   │  │
│  └────────────────────────────────────────────────────────────┘  │
│                                                                   │
│  SubscriptionRenewalFunction                                     │
│  ┌────────────────────────────────────────────────────────────┐  │
│  │ TimerTrigger: every 12 hours (0 0 */12 * * *)              │  │
│  │ → GraphSubscriptionService.EnsureSubscriptionAsync()       │  │
│  │   Renews subscription if within 12 hrs of 3-day expiry     │  │
│  └────────────────────────────────────────────────────────────┘  │
└──────────────────────────────────────────────────────────────────┘
```

#### Startup Sequence

```
Program.cs host.RunAsync()
    └── IHostApplicationLifetime.ApplicationStarted.Register(...)
            └── GraphSubscriptionService.EnsureSubscriptionAsync()
                    (deferred until HTTP triggers are live — prevents validation timeout)
```

#### AI Analysis Flow

```
EmailAnalysisService.AnalyzeEmailAsync(email)
    │
    ├── Provider = "OpenAI"      → OpenAI chat completion
    ├── Provider = "Claude"      → Anthropic Claude API
    └── Provider = "AzureOpenAI" → Azure OpenAI endpoint
    
    Returns: { Category, Confidence, ExtractedData }
    
    Categories: enquiry | site_visit | invoice | quote |
                showroom | complaint | junk | general
```

---

### 4. Shared Database (Azure SQL)

Both the API and the Email Function connect to the **same SQL Server database**.

#### Core Tables

```
── Identity ──────────────────────────────────────────────────────
  Users               JWT users, BCrypt passwords, roles
  RefreshTokens       7-day rotating refresh tokens

── CRM ───────────────────────────────────────────────────────────
  Customers           Company/residential customer records
  CustomerContacts    Contact persons per customer

── Sales Pipeline ────────────────────────────────────────────────
  WorkflowStarts      Root of the pipeline per customer
  InitialEnquiries    Stage 1 — enquiry details
  Quotes              Stage 2 — quote header
  QuoteItems          Line items per quote
  ShowroomInvites     Optional showroom visit
  SiteVisits          Stage 3 — site visit records
  SiteVisitValues     Measurements taken on site
  SiteVisitImages     Photos from site visit
  Invoices            Stage 4 — invoice header
  InvoiceItems        Line items per invoice
  InvoicePayments     Payment receipts
  PaymentSchedules    Payment milestone tracking

── Products & Suppliers ──────────────────────────────────────────
  Suppliers           Supplier catalogue
  ProductTypes        Product categories
  Products            Specific products per supplier
  ProductItems        Items in a quote/workflow
  Arms, Brackets, Motors, Controls, Projections, ...
  (product configuration lookup tables)

── Email Processing ──────────────────────────────────────────────
  IncomingEmails      Raw email metadata + BodyBlobUrl
  EmailAttachments    Base64 or BlobStorageUrl per attachment
  GraphSubscriptions  Active Graph webhook subscription record

── Task Management ───────────────────────────────────────────────
  Tasks (AppTask)     Unified task record (Email/SiteVisit/Manual)
  TaskAttachments     Attachment references linked to task
  TaskComments        User comments per task
  TaskHistories       Full action log (Created/Assigned/Read/etc.)

── Workflow Support ──────────────────────────────────────────────
  WorkflowFollowUps   Scheduled follow-up reminders
  UserSignatures      Email signature per user

── Audit ─────────────────────────────────────────────────────────
  AuditLogs           Field-level change log (entity + field + old/new)
```

---

### 5. Email Processing Pipeline (End to End)

```
Outlook Mailbox (hello@awningsofireland.com)
        │ new email arrives
        ▼
Microsoft Graph  ──── webhook notification (POST) ────▶  GraphWebhookFunction
        │                                                        │
        │                                                        │ enqueue messageId
        │                                                        ▼
        │                                             Azure Storage Queue
        │                                             "email-processing"
        │                                                        │
        │                                                        │ dequeue
        │                                                        ▼
        │                                             EmailProcessorFunction
        │                                                        │
        ◀─── GET message + attachments ─────────────────────────┘
        │
        │  BodyContent (HTML with cid: refs)
        │  FileAttachments (IsInline, ContentId, Base64)
        ▼
ReplaceCidReferences()
  cid:{ContentId} → data:{ContentType};base64,{Base64}
        │
        ▼
BlobEmailStorageService
  Upload body HTML  → awnings-emails/bodies/{emailId}.html
  Upload attachments → awnings-emails/attachments/{eId}/{aId}/{filename}
        │
        ▼
  IncomingEmail saved to DB (BodyBlobUrl, attachments)
        │
        ▼
EmailAnalysisService (AI)
  → Category + Confidence + ExtractedData
        │
        ▼
CreateTaskAsync
  AppTask { SourceType=Email, Category, Status=New, ... }
  TaskAttachments copied from EmailAttachments
        │
        ▼
AutoLinkCustomerAndWorkflowAsync
  Match sender email → Customer → WorkflowStart
  If single workflow found → auto-complete task
        │
        ▼
  Task visible in Angular email inbox
```

---

### 6. Sales Workflow Pipeline

```
Customer Contact
      │
      ▼
InitialEnquiry  (WorkflowStart created)
      │
      ▼
Quote           (product selection, pricing, PDF generation)
      │
      ├──▶ ShowroomInvite (optional)
      │
      ▼
SiteVisit       (measurements, photos, values)
      │
      ▼
FinalQuote      (revised after site visit)
      │
      ▼
Invoice         (line items, VAT, totals)
      │
      ▼
PaymentSchedule (milestones — deposit, balance, etc.)
      │
      ▼
Completed
```

---

### 7. Authentication & Security

```
Login → AuthService
  │  BCrypt.Verify(password, hash)
  │  Generate JWT (24hr expiry, HS256)
  │  Generate RefreshToken (7-day, stored in DB)
  │
  ▼
JWT stored in Angular (localStorage / memory)
  │
  └── Authorization: Bearer <token>  on every API request

Token refresh:
  Angular intercepts 401 → POST /api/Auth/refresh
  → new access token + rotating refresh token

Roles:
  Admin  → sees all tasks/users, can assign to anyone
  User   → sees only their own assigned tasks
```

---

### 8. CI/CD Pipelines

#### `ci.yml` — AwningsAPI

```
Trigger: push to any branch (AwningsAPI/** paths only)

Jobs:
  build-and-test
    → dotnet restore + build (Release)
    → [tests commented out]

  deploy  (master push only)
    → docker build + push to ghcr.io/ranjith83/awningsapi:sha
    → azure/container-apps-deploy-action
    → Auth: AZURE_CREDENTIALS secret
```

#### `email-function.yml` — AwningsEmailFunction

```
Trigger: push to any branch
         (AwningsEmailFunction/**, AwningsEmailFunction.Tests/**, 
          .github/workflows/email-function.yml paths)

Jobs:
  build-and-test
    → dotnet restore + publish (Release, linux-x64)

  deploy  (master push only)
    → cd ./publish && zip -r ../publish.zip .
    → azure/login (OIDC — client-id, tenant-id, subscription-id)
    → curl POST https://{APP_NAME}.scm.azurewebsites.net/api/zipdeploy
         Authorization: Bearer {ARM token}
    → Auth: Federated Identity (no long-lived secret)
```

---

### 9. Configuration Reference

#### AwningsAPI (`appsettings.json`)

| Key | Purpose |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server |
| `JwtSettings:SecretKey / Issuer / Audience / ExpirationInHours` | JWT config |
| `AzureAd:TenantId / ClientId / ClientSecret` | Graph API client credentials |
| `AzureAd:MonitoredMailbox` | Mailbox to watch (`hello@awningsofireland.com`) |
| `AI:Provider` | `OpenAI` / `Claude` / `AzureOpenAI` |
| `OpenAI:ApiKey / Model` | OpenAI credentials |
| `GraphSubscription:NotificationUrl` | Webhook endpoint URL |
| `GraphSubscription:ClientState` | Webhook validation secret |
| `BlobStorage:AccountUrl` | Blob service URL (production — Managed Identity) |
| `BlobStorage:ConnectionString` | Azurite connection string (local dev only) |
| `BlobStorage:ContainerName` | `awnings-emails` |
| `EmailConfiguration:IsProd` | `false` → redirects outbound email to TestMailAddress |
| `EmailConfiguration:TestMailAddress` | Dev email redirect target |
| `AllowedOrigins` | CORS origins for Angular app |

#### AwningsEmailFunction (`local.settings.json` / Azure config)

| Key | Purpose |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server (same DB as API) |
| `AzureWebJobsStorage` | Azure Storage for queues/timers |
| `BlobStorageConnectionString` | Blob storage (Azurite locally) |
| `BlobStorageContainerName` | `awnings-emails` |
| `AzureAd:TenantId / ClientId / ClientSecret` | Graph API credentials |
| `AzureAd:OrganizerEmail` | Monitored mailbox |
| `GraphSubscription:NotificationUrl` | This function's webhook URL |
| `GraphSubscription:ClientState` | Validation secret (must match API) |
| `GraphSubscription:LifetimeMinutes` | `4230` (~3 days, Graph max) |
| `GraphSubscription:RenewalBufferHours` | `12` |
| `AwningsAPI:BaseUrl` | API base URL for internal calls |
| `EmailReader:MaxEmailsPerBatch` | Max emails fetched per poll |

---

### 10. Key Design Decisions

| Decision | Rationale |
|---|---|
| Email body stored in Blob, not DB | HTML with inline images can be 1–5 MB; keeps DB lean |
| CID → data-URI replacement at ingest time | Avoids needing Graph auth at render time; images self-contained in blob |
| Email body served via API proxy, not direct blob URL | Blob container is private; Angular can't authenticate directly |
| Email body rendered in `<iframe src="blob:...">` | Angular's `[innerHTML]` sanitizer strips `data:` URIs; iframe isolates the content |
| Queue between webhook and processor | Graph requires 202 within seconds; heavy processing decoupled with retry guarantee |
| `IHostApplicationLifetime.ApplicationStarted` for subscription | HTTP triggers must be live before Graph validation handshake |
| `db.Database.Migrate()` on startup | Removes dependency on CI migration job; single deployment step |
| Managed Identity for blob auth in production | No stored credentials; eliminates secret rotation risk |
| Two separate CI/CD pipelines | Path-filtered triggers prevent API changes from deploying the function and vice versa |
| `ExcludeCategories` filter | Tasks tab hides junk/general without a separate API; Junk tab uses `Categories` filter |
| `downloadableAttachments` filters `isInline=false` | Inline images (embedded in body) excluded from the Attachments tab count and list |
