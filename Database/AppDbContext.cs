using AwiningsIreland_WebAPI.Models;
using AwningsAPI.Model.Audit;
using AwningsAPI.Model.Auth;
using AwningsAPI.Model.Customers;
using AwningsAPI.Model.Email;
using AwningsAPI.Model.Products;
using AwningsAPI.Model.Showroom;
using AwningsAPI.Model.SiteVisit;
using AwningsAPI.Model.Suppliers;
using AwningsAPI.Model.Tasks;
using AwningsAPI.Model.Workflow;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System.Text.Json;


namespace AwningsAPI.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerContact> CustomerContacts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<WorkflowStart> WorkflowStarts { get; set; }
        public DbSet<InitialEnquiry> InitialEnquiries { get; set; }
        public DbSet<Projections> Projections { get; set; }
        public DbSet<Brackets> Brackets { get; set; }
        public DbSet<BF> BFs { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<InvoicePayment> InvoicePayments { get; set; }
        public DbSet<Arms> Arms { get; set; }
        public DbSet<Motors> Motors { get; set; }
        public DbSet<ValanceStyle> valanceStyles { get; set; }
        public DbSet<NonStandardRALColours> nonStandardRALColours { get; set; }
        public DbSet<WallSealingProfile> wallSealingProfiles { get; set; }
        public DbSet<Heaters> Heaters { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<QuoteItem> QuoteItems { get; set; }
        public DbSet<SiteVisit> SiteVisits { get; set; }
        public DbSet<SiteVisitValues> SiteVisitValues { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<PaymentSchedule> PaymentSchedules { get; set; }
        public DbSet<ShowroomInvite> ShowroomInvites { get; set; }

        public DbSet<ArmsType> armsTypes { get; set; }
        public DbSet<RadioControlledMotors> radioControlledMotors { get; set; }
        public DbSet<Control> Controls { get; set; }
        public DbSet<ShadePlus> ShadePlus { get; set; }
        public DbSet<LightingCassette> LightingCassettes { get; set; }
        public DbSet<IncomingEmail> IncomingEmails { get; set; }
        public DbSet<EmailAttachment> EmailAttachments { get; set; }
        public DbSet<EmailTask> EmailTasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TaskAttachment> TaskAttachments { get; set; }
        public DbSet<TaskHistory> TaskHistories { get; set; }
        public DbSet<WorkflowFollowUp> WorkflowFollowUps { get; set; }

        public DbSet<UserSignature> UserSignatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Customer Configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.CompanyNumber).HasMaxLength(50);
                entity.Property(e => e.Residential).HasDefaultValue(false);
                entity.Property(e => e.VATNumber).HasMaxLength(50);
                entity.Property(e => e.TaxNumber).HasMaxLength(50);
                entity.Property(e => e.Address1).HasMaxLength(255);
                entity.Property(e => e.Address2).HasMaxLength(255);
                entity.Property(e => e.Address3).HasMaxLength(255);
                entity.Property(e => e.County).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Mobile).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.Eircode).HasMaxLength(10);

                // Salesperson fields
                entity.Property(e => e.AssignedSalespersonId).IsRequired(false);
                entity.Property(e => e.AssignedSalespersonName).HasMaxLength(255).IsRequired(false);

                // Audit fields
                entity.Property(e => e.CreatedBy).HasMaxLength(255);
                entity.Property(e => e.UpdatedBy).HasMaxLength(255);

                // Relationships
                entity.HasMany(c => c.CustomerContacts)
                    .WithOne(cc => cc.Customer)
                    .HasForeignKey(cc => cc.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Email);
                entity.HasIndex(e => e.AssignedSalespersonId);
            });

            // CustomerContact Configuration
            modelBuilder.Entity<CustomerContact>(entity =>
            {
                entity.ToTable("CustomerContacts");
                entity.HasKey(e => e.ContactId);

                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.HasIndex(e => e.CustomerId);
                entity.HasIndex(e => e.Email);
            });

            modelBuilder.Entity<Projections>()
                .Property(p => p.Price)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Brackets>()
                .Property(p => p.Price)
                .HasPrecision(18, 4);

            modelBuilder.Entity<BF>()
                .Property(p => p.Price)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Arms>()
            .Property(p => p.Price)
            .HasPrecision(18, 4);

            modelBuilder.Entity<Motors>()
            .Property(p => p.Price)
            .HasPrecision(18, 4);

            modelBuilder.Entity<ValanceStyle>()
            .Property(p => p.Price)
            .HasPrecision(18, 4);

            modelBuilder.Entity<NonStandardRALColours>()
                .Property(p => p.Price)
                .HasPrecision(18, 4);

            modelBuilder.Entity<WallSealingProfile>()
                .Property(p => p.Price)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Heaters>()
                .Property(p => p.Price)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Heaters>()
                .Property(p => p.PriceNonRALColour)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Control>()
                .Property(p => p.Price)
                .HasPrecision(18, 4);

            modelBuilder.Entity<ShadePlus>()
                .Property(p => p.Price)
                .HasPrecision(18, 4);

            modelBuilder.Entity<LightingCassette>()
                .Property(p => p.Price)
                .HasPrecision(18, 4);

            // Invoice Configuration
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoices");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.InvoiceNumber).IsUnique();
                entity.Property(e => e.Status).HasDefaultValue("Draft");

                entity.HasMany(e => e.InvoiceItems)
                    .WithOne(i => i.Invoice)
                    .HasForeignKey(i => i.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.InvoicePayments)
                    .WithOne(p => p.Invoice)
                    .HasForeignKey(p => p.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // InvoiceItem Configuration
            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.ToTable("InvoiceItems");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
            });

            // InvoicePayment Configuration
            modelBuilder.Entity<InvoicePayment>(entity =>
            {
                entity.ToTable("InvoicePayments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
            });

            //Quote Configuration
            modelBuilder.Entity<Quote>(entity =>
            {
                entity.ToTable("Quotes");
                entity.HasKey(e => e.QuoteId);
                entity.Property(e => e.QuoteNumber).IsRequired();
                entity.HasIndex(e => e.QuoteNumber).IsUnique();

                entity.HasMany(e => e.QuoteItems)
                    .WithOne(i => i.Quote)
                    .HasForeignKey(i => i.QuoteId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // QuoteItem Configuration
            modelBuilder.Entity<QuoteItem>(entity =>
            {
                entity.ToTable("QuoteItems");
                entity.HasKey(e => e.QuoteItemId);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
            });

            // In OnModelCreating method, add:
            modelBuilder.Entity<SiteVisit>(entity =>
            {
                entity.ToTable("SiteVisits");
                entity.HasKey(e => e.SiteVisitId);

                entity.Property(e => e.ProductModelType).IsRequired();
                entity.Property(e => e.CreatedBy).IsRequired();
                entity.Property(e => e.DateCreated).IsRequired();

                entity.HasOne(e => e.Workflow)
                .WithMany()
                .HasForeignKey(e => e.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.WorkflowId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).HasDefaultValue("User");
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasMany(e => e.RefreshTokens)
                    .WithOne(rt => rt.User)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // RefreshToken Configuration
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshTokens");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Token).IsRequired();
                entity.HasIndex(e => e.Token);
                entity.Property(e => e.IsRevoked).HasDefaultValue(false);
            });

            modelBuilder.Entity<SiteVisitValues>(entity =>
            {
                entity.ToTable("SiteVisitValues");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.Category, e.Value }).IsUnique();
                entity.HasIndex(e => e.Category);
            });


            // AuditLog Configuration
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("AuditLogs");
                entity.HasKey(e => e.AuditId);

                entity.Property(e => e.EntityType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.EntityId).IsRequired();
                entity.Property(e => e.Action).IsRequired().HasMaxLength(20);
                entity.Property(e => e.PerformedByName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PerformedAt).IsRequired();

                // Indexes for performance
                entity.HasIndex(e => new { e.EntityType, e.EntityId })
                    .HasDatabaseName("IX_AuditLogs_Entity");

                entity.HasIndex(e => e.PerformedAt)
                    .HasDatabaseName("IX_AuditLogs_PerformedAt")
                    .IsDescending();

                entity.HasIndex(e => e.PerformedBy)
                    .HasDatabaseName("IX_AuditLogs_PerformedBy");

                entity.HasIndex(e => e.Action)
                    .HasDatabaseName("IX_AuditLogs_Action");
            });

            // Payment Schedule Configuration
            modelBuilder.Entity<PaymentSchedule>(entity =>
            {
                entity.ToTable("PaymentSchedules");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Invoice)
                    .WithMany()
                    .HasForeignKey(e => e.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.InvoiceId);
                entity.HasIndex(e => new { e.InvoiceId, e.SortOrder });
            });


            // Seed Admin User
            var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
            modelBuilder.Entity<User>().HasData(
             new User
             {
                 UserId = 1,
                 FirstName = "System",
                 LastName = "Admin",
                 Email = "admin@awnings.ie",
                 Username = "admin",
                 PasswordHash = "$2a$11$lB5zG4AjL5L2gP2V22pquu8HbsUyF6q7Q8HqZQrjO2KQhkXj6nFlO", // ✔ static
                 Role = "Admin",
                 Department = "IT",
                 IsActive = true,
                 DateCreated = new DateTime(2023, 1, 1, 12, 0, 0),
                 CreatedBy = "System"
             }
             );


            // EmailTask Configuration
            modelBuilder.Entity<EmailTask>(entity =>
            {
                entity.ToTable("EmailTasks");
                entity.HasKey(e => e.TaskId);

                // The properties FromName, FromEmail, Subject, and Category already have
                // [Required] and [StringLength] attributes in the model, so we don't need
                // to configure them again here. EF Core will read those attributes.

                // Configure relationships
                entity.HasMany(t => t.TaskComments)
                    .WithOne(c => c.EmailTask)
                    .HasForeignKey(c => c.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.TaskAttachments)
                    .WithOne(a => a.EmailTask)
                    .HasForeignKey(a => a.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.TaskHistories)
                    .WithOne(h => h.EmailTask)
                    .HasForeignKey(h => h.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes for better query performance
                entity.HasIndex(e => e.IncomingEmailId)
                    .HasDatabaseName("IX_EmailTasks_IncomingEmailId");

                entity.HasIndex(e => e.Status)
                    .HasDatabaseName("IX_EmailTasks_Status");

                entity.HasIndex(e => e.Category)
                    .HasDatabaseName("IX_EmailTasks_Category");

                entity.HasIndex(e => e.AssignedToUserId)
                    .HasDatabaseName("IX_EmailTasks_AssignedToUserId");

                entity.HasIndex(e => e.DateAdded)
                    .HasDatabaseName("IX_EmailTasks_DateAdded");

                entity.HasIndex(e => e.CustomerId)
                    .HasDatabaseName("IX_EmailTasks_CustomerId");

                entity.HasIndex(e => new { e.Status, e.DateAdded })
                    .HasDatabaseName("IX_EmailTasks_Status_DateAdded");



            });

            // TaskComment Configuration
            modelBuilder.Entity<TaskComment>(entity =>
            {
                entity.ToTable("TaskComments");
                entity.HasKey(e => e.CommentId);

                entity.Property(e => e.CommentText)
                    .IsRequired()
                    .HasColumnType("nvarchar(MAX)");

                entity.HasIndex(e => e.TaskId)
                    .HasDatabaseName("IX_TaskComments_TaskId");

                entity.HasIndex(e => e.DateCreated)
                    .HasDatabaseName("IX_TaskComments_DateCreated");

                entity.HasIndex(e => e.UserId)
                    .HasDatabaseName("IX_TaskComments_UserId");
            });

            // TaskAttachment Configuration
            modelBuilder.Entity<TaskAttachment>(entity =>
            {
                entity.ToTable("TaskAttachments");
                entity.HasKey(e => e.AttachmentId);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ExtractedText)
                    .HasColumnType("nvarchar(MAX)");

                entity.HasIndex(e => e.TaskId)
                    .HasDatabaseName("IX_TaskAttachments_TaskId");

                entity.HasIndex(e => e.EmailAttachmentId)
                    .HasDatabaseName("IX_TaskAttachments_EmailAttachmentId");
            });

            // TaskHistory Configuration
            modelBuilder.Entity<TaskHistory>(entity =>
            {
                entity.ToTable("TaskHistories");
                entity.HasKey(e => e.HistoryId);

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Details)
                    .HasColumnType("nvarchar(MAX)");

                entity.HasIndex(e => e.TaskId)
                    .HasDatabaseName("IX_TaskHistories_TaskId");

                entity.HasIndex(e => e.DateCreated)
                    .HasDatabaseName("IX_TaskHistories_DateCreated");

                entity.HasIndex(e => new { e.TaskId, e.DateCreated })
                    .HasDatabaseName("IX_TaskHistories_TaskId_DateCreated");
            });

            modelBuilder.Entity<WorkflowFollowUp>(entity =>
            {
                entity.ToTable("WorkflowFollowUps");

                entity.HasKey(e => e.FollowUpId);

                // ── Required fields ─────────────────────────────
                entity.Property(e => e.WorkflowId)
                      .IsRequired();

                entity.Property(e => e.EnquiryId)
                      .IsRequired();

                entity.Property(e => e.LastEnquiryDate)
                      .IsRequired();

                entity.Property(e => e.Subject)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.Category)
                      .HasMaxLength(100)
                      .HasDefaultValue("Inquiry");

                entity.Property(e => e.DateAdded)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.DateCreated)
                      .HasDefaultValueSql("GETUTCDATE()");

                // ── Optional fields ─────────────────────────────
                entity.Property(e => e.CompanyName)
                      .HasMaxLength(255);

                entity.Property(e => e.EnquiryComments)
                      .HasColumnType("nvarchar(max)");

                entity.Property(e => e.EnquiryEmail)
                      .HasMaxLength(255);

                entity.Property(e => e.ResolvedBy)
                      .HasMaxLength(255);

                entity.Property(e => e.DismissReason)
                      .HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(255);

                // ── Indexes (IMPORTANT for performance) ─────────
                entity.HasIndex(e => e.WorkflowId);
                entity.HasIndex(e => e.EnquiryId);
                entity.HasIndex(e => e.IsDismissed);
                entity.HasIndex(e => e.DateAdded);

                // ── Relationships ───────────────────────────────

                // Link to WorkflowStart
                entity.HasOne<WorkflowStart>()
                      .WithMany()
                      .HasForeignKey(e => e.WorkflowId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Optional link to Customer
                entity.HasOne<Customer>()
                      .WithMany()
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.SetNull);

                // Optional link to InitialEnquiry
                entity.HasOne<InitialEnquiry>()
                      .WithMany()
                      .HasForeignKey(e => e.EnquiryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<IncomingEmail>()
                 .HasIndex(e => e.EmailId)
                 .IsUnique()
                 .HasDatabaseName("IX_IncomingEmails_EmailId_Unique");

            // ── UserSignature ─────────────────────────────────────────────────
            modelBuilder.Entity<UserSignature>(entity =>
            {
                entity.ToTable("UserSignatures");
                entity.HasKey(e => e.SignatureId);

                entity.Property(e => e.Username).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Label).IsRequired().HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(150);
                entity.Property(e => e.JobTitle).HasMaxLength(150);
                entity.Property(e => e.Company).HasMaxLength(150);
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.Mobile).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.Website).HasMaxLength(255);

                entity.Property(e => e.GreetingText).HasMaxLength(100).HasDefaultValue("Kindest regards,");
                entity.Property(e => e.SeparatorStyle).HasMaxLength(30).HasDefaultValue("blank_line");
                entity.Property(e => e.LayoutOrder).HasMaxLength(30).HasDefaultValue("name_first");

                entity.Property(e => e.SignatureText).IsRequired();
                entity.Property(e => e.IsDefault).HasDefaultValue(false);

                // Fast per-user lookups
                entity.HasIndex(e => e.Username);
            });

            base.OnModelCreating(modelBuilder);

            var staticCreatedDate = new DateTime(2023, 01, 01, 12, 00, 00);
            var staticUpdatedDate = new DateTime(2023, 01, 02, 12, 00, 00);

            // Seed Customer with Salesperson
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    Name = "Acme Corporation",
                    CompanyNumber = "ACME123",
                    Residential = false,
                    RegistrationNumber = "REG456789",
                    VATNumber = "VAT987654",
                    Address1 = "123 Main Street",
                    Address2 = "Suite 100",
                    Address3 = null,
                    County = "Dublin",
                    CountryId = 1,
                    Phone = "+35312345678",
                    Fax = null,
                    Mobile = "+35387654321",
                    Email = "info@acme.ie",
                    TaxNumber = "TAX123456",
                    Eircode = "D01XY12",
                    AssignedSalespersonId = 1,
                    AssignedSalespersonName = "System Admin",
                    DateCreated = staticCreatedDate,
                    CreatedBy = "System"
                }
            );

            // Seed CustomerContact
            modelBuilder.Entity<CustomerContact>().HasData(
                new CustomerContact
                {
                    ContactId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1985, 5, 20),
                    Mobile = "+35387654322",
                    Phone = "+35312345679",
                    Email = "john.doe@acme.ie",
                    DateCreated = staticCreatedDate,
                    CreatedBy = "System",
                    CustomerId = 1
                }
            );
            // Seed ProductTypes
            modelBuilder.Entity<ProductType>().HasData(
                new ProductType { ProductTypeId = 1, SupplierId = 1, Description = "Folding-arm Cassette Awnings", DateCreated = staticCreatedDate, CreatedBy = "System" },
                new ProductType { ProductTypeId = 2, SupplierId = 1, Description = "Folding-arm Semi-cassette Awnings", DateCreated = staticCreatedDate, CreatedBy = "System" },
                new ProductType { ProductTypeId = 3, SupplierId = 1, Description = "Open Folding-arm Awnings", DateCreated = staticCreatedDate, CreatedBy = "System" },
                new ProductType { ProductTypeId = 4, SupplierId = 1, Description = "Stretch-Awnings", DateCreated = staticCreatedDate, CreatedBy = "System" },
                new ProductType { ProductTypeId = 5, SupplierId = 1, Description = "Wind Protection and Privacy", DateCreated = staticCreatedDate, CreatedBy = "System" },
                new ProductType { ProductTypeId = 6, SupplierId = 1, Description = "Awning Systems", DateCreated = staticCreatedDate, CreatedBy = "System" },
                new ProductType { ProductTypeId = 7, SupplierId = 1, Description = "Free-standing awning stand systems", DateCreated = staticCreatedDate, CreatedBy = "System" },
                new ProductType { ProductTypeId = 8, SupplierId = 1, Description = "Pergola awnings", DateCreated = staticCreatedDate, CreatedBy = "System" },
                new ProductType { ProductTypeId = 9, SupplierId = 1, Description = "Conservatory and Glass Canopy Awnings", DateCreated = staticCreatedDate, CreatedBy = "System" },
                new ProductType { ProductTypeId = 10, SupplierId = 1, Description = "Vertical Roller Blinds and Awnings", DateCreated = staticCreatedDate, CreatedBy = "System" }
            );
            //Suppliers
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { SupplierId = 1, SupplierName = "Markilux", DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Supplier { SupplierId = 2, SupplierName = "Rensen", DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Supplier { SupplierId = 3, SupplierName = "Practic", DateCreated = staticCreatedDate, CreatedBy = "System" }
            );
            //Products  
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Description = "Markilux MX-1 compact", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 2, Description = "Markilux MX-4", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 3, Description = "Markilux MX-2", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 4, Description = "Markilux 6000", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 5, Description = "Markilux MX-3", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 6, Description = "Markilux 990", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 7, Description = "Markilux 970", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 8, Description = "Markilux 5010", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 9, Description = "Markilux 3300", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 10, Description = "Markilux 1710", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 11, Description = "Markilux 900", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 12, Description = "Markilux 3300 Semi", ProductTypeId = 1, SupplierId = 1, DateCreated = new DateTime(2026, 4, 6), CreatedBy = "System" },
                new Product { ProductId = 13, Description = "Markilux 6000", ProductTypeId = 1, SupplierId = 1, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System" },
                new Product { ProductId = 14, Description = "Markilux 779", ProductTypeId = 1, SupplierId = 1, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System" },
                new Product { ProductId = 15, Description = "Markilux 8800", ProductTypeId = 1, SupplierId = 1, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System" },
                new Product { ProductId = 16, Description = "Markilux 6000 XXL", ProductTypeId = 1, SupplierId = 1, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System" }
            );
            //Workflow Start
            modelBuilder.Entity<WorkflowStart>().HasData(
                new WorkflowStart { WorkflowId = 1, CustomerId = 1, Description = "Markilux 990 for outside garden", DateCreated = staticCreatedDate, CreatedBy = "System", SupplierId = 1, ProductTypeId = 1, ProductId = 6 }
            );
            //Projection Data
            modelBuilder.Entity<Projections>().HasData(
                new Projections { ProjectionId = 1, ProductId = 6, ArmTypeId = 1, Width_cm = 250, Projection_cm = 150, Price = 1873, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 2, ProductId = 6, ArmTypeId = 1, Width_cm = 300, Projection_cm = 150, Price = 2023, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 3, ProductId = 6, ArmTypeId = 1, Width_cm = 350, Projection_cm = 150, Price = 2229, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 4, ProductId = 6, ArmTypeId = 1, Width_cm = 400, Projection_cm = 150, Price = 2397, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 5, ProductId = 6, ArmTypeId = 1, Width_cm = 450, Projection_cm = 150, Price = 2554, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 6, ProductId = 6, ArmTypeId = 1, Width_cm = 500, Projection_cm = 150, Price = 2730, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 7, ProductId = 6, ArmTypeId = 1, Width_cm = 250, Projection_cm = 200, Price = 1979, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 8, ProductId = 6, ArmTypeId = 1, Width_cm = 300, Projection_cm = 200, Price = 2145, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 9, ProductId = 6, ArmTypeId = 1, Width_cm = 350, Projection_cm = 200, Price = 2319, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 10, ProductId = 6, ArmTypeId = 1, Width_cm = 400, Projection_cm = 200, Price = 2508, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 11, ProductId = 6, ArmTypeId = 1, Width_cm = 450, Projection_cm = 200, Price = 2664, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 12, ProductId = 6, ArmTypeId = 1, Width_cm = 500, Projection_cm = 200, Price = 2842, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 13, ProductId = 6, ArmTypeId = 1, Width_cm = 300, Projection_cm = 250, Price = 2248, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 14, ProductId = 6, ArmTypeId = 1, Width_cm = 350, Projection_cm = 250, Price = 2431, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 15, ProductId = 6, ArmTypeId = 1, Width_cm = 400, Projection_cm = 250, Price = 2627, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 16, ProductId = 6, ArmTypeId = 1, Width_cm = 450, Projection_cm = 250, Price = 2798, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 17, ProductId = 6, ArmTypeId = 1, Width_cm = 500, Projection_cm = 250, Price = 2970, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 18, ProductId = 6, ArmTypeId = 1, Width_cm = 350, Projection_cm = 300, Price = 2547, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 19, ProductId = 6, ArmTypeId = 1, Width_cm = 400, Projection_cm = 300, Price = 2750, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 20, ProductId = 6, ArmTypeId = 1, Width_cm = 450, Projection_cm = 300, Price = 2904, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 21, ProductId = 6, ArmTypeId = 1, Width_cm = 500, Projection_cm = 300, Price = 3084, DateCreated = staticCreatedDate, CreatedBy = "System" }
            );

            //BF Data
            modelBuilder.Entity<BF>().HasData(
                new BF { BFId = 1, Description = "BF 6", Price = 312.00m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new BF { BFId = 2, Description = "BF 8", Price = 312.00m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new BF { BFId = 3, Description = "BF 16", Price = 312.00m, DateCreated = staticCreatedDate, CreatedBy = "System" }
            );
            //Bracket Data
            modelBuilder.Entity<Brackets>().HasData(
                // ProductId = 6 (Markilux 990) — ArmTypeId NULL for non-arm-specific surcharges/parts
                new Brackets { BracketId = 1, ProductId = 6, ArmTypeId = null, BracketName = "Surcharge for face fixture", PartNumber = "", Price = 86m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 2, ProductId = 6, ArmTypeId = null, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = "", Price = 334m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 3, ProductId = 6, ArmTypeId = null, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = "", Price = 406m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 4, ProductId = 6, ArmTypeId = null, BracketName = "Surcharge for top fixture", PartNumber = "", Price = 86m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 5, ProductId = 6, ArmTypeId = null, BracketName = "Surcharge for eaves fixture", PartNumber = "", Price = 199m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 6, ProductId = 6, ArmTypeId = null, BracketName = "Surcharge for arms with bionic tendon", PartNumber = "", Price = 117m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 7, ProductId = 6, ArmTypeId = null, BracketName = "Surcharge for bespoke arms", PartNumber = "", Price = 177m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 8, ProductId = 6, ArmTypeId = null, BracketName = "Face fixture bracket 150 mm", PartNumber = "71624", Price = 42.70m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 9, ProductId = 6, ArmTypeId = null, BracketName = "Face fixture bracket 300 mm left", PartNumber = "70617", Price = 73.50m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 10, ProductId = 6, ArmTypeId = null, BracketName = "Face fixture bracket 300 mm right", PartNumber = "70600", Price = 73.50m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 11, ProductId = 6, ArmTypeId = null, BracketName = "Stand-off bkt. 80-300 mm for face fixture bracket 300 mm", PartNumber = "77968", Price = 220.50m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 12, ProductId = 6, ArmTypeId = null, BracketName = "Top fixture bracket 150 mm", PartNumber = "71625", Price = 42.70m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 13, ProductId = 6, ArmTypeId = null, BracketName = "Eaves fixture bracket 150mm, complete", PartNumber = "71669", Price = 99.30m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 14, ProductId = 6, ArmTypeId = null, BracketName = "Eaves fixture bracket 270 mm", PartNumber = "71659", Price = 77.00m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 15, ProductId = 6, ArmTypeId = null, BracketName = "Angle and plate for eaves fixture (machine finish)", PartNumber = "716620", Price = 125.20m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 16, ProductId = 6, ArmTypeId = null, BracketName = "Additional eaves fixture plate 60x260x12 mm", PartNumber = "75383", Price = 42.60m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 17, ProductId = 6, ArmTypeId = null, BracketName = "Spreader plate A 430x160x12 mm", PartNumber = "75326", Price = 124.10m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 18, ProductId = 6, ArmTypeId = null, BracketName = "Spreader plate B 300x400x12 mm", PartNumber = "75325", Price = 160.20m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 19, ProductId = 6, ArmTypeId = null, BracketName = "Spacer block face or top fixt. 136x150x20 mm", PartNumber = "716331", Price = 5.50m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 20, ProductId = 6, ArmTypeId = null, BracketName = "Spacer block face or top fixt. 136x150x12 mm", PartNumber = "71644", Price = 3.60m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 21, ProductId = 6, ArmTypeId = null, BracketName = "Cover plate 230x210x2 mm", PartNumber = "71843", Price = 16.50m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 22, ProductId = 6, ArmTypeId = null, BracketName = "Cover plate 290x210x2 mm", PartNumber = "71841", Price = 20.50m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 23, ProductId = 6, ArmTypeId = null, BracketName = "Vertical fixture rail incl. fixing material 624291", PartNumber = "62421", Price = 174.90m, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // ProductId = 7 (Markilux 970) — ArmTypeId = 1 for surcharges
                new Brackets { BracketId = 78, ProductId = 7, ArmTypeId = 1, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 220.00m, DateCreated = new DateTime(2026, 4, 6, 13, 41, 50, 820), CreatedBy = "System" },
                new Brackets { BracketId = 79, ProductId = 7, ArmTypeId = 1, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 592.00m, DateCreated = new DateTime(2026, 4, 6, 13, 41, 50, 820), CreatedBy = "System" },
                new Brackets { BracketId = 80, ProductId = 7, ArmTypeId = 1, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 550.00m, DateCreated = new DateTime(2026, 4, 6, 13, 41, 50, 820), CreatedBy = "System" },
                new Brackets { BracketId = 81, ProductId = 7, ArmTypeId = 1, BracketName = "Surcharge for face fixture incl. spreader plate C", PartNumber = null, Price = 592.00m, DateCreated = new DateTime(2026, 4, 6, 13, 41, 50, 820), CreatedBy = "System" },
                new Brackets { BracketId = 82, ProductId = 7, ArmTypeId = 1, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 278.00m, DateCreated = new DateTime(2026, 4, 6, 13, 41, 50, 820), CreatedBy = "System" },
                new Brackets { BracketId = 83, ProductId = 7, ArmTypeId = 1, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 371.00m, DateCreated = new DateTime(2026, 4, 6, 13, 41, 50, 820), CreatedBy = "System" },

                // ProductId = 1 (Markilux MX-1 compact) — ArmTypeId varies per row
                new Brackets { BracketId = 110, ProductId = 1, ArmTypeId = 1, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 7, 19, 20, 47, 870), CreatedBy = "System" },
                new Brackets { BracketId = 111, ProductId = 1, ArmTypeId = 2, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 7, 19, 20, 47, 870), CreatedBy = "System" },
                new Brackets { BracketId = 112, ProductId = 1, ArmTypeId = 3, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 269.00m, DateCreated = new DateTime(2026, 4, 7, 19, 20, 47, 870), CreatedBy = "System" },
                new Brackets { BracketId = 113, ProductId = 1, ArmTypeId = 1, BracketName = "Surcharge for face fixture A", PartNumber = null, Price = 22.00m, DateCreated = new DateTime(2026, 4, 7, 19, 20, 47, 870), CreatedBy = "System" },
                new Brackets { BracketId = 114, ProductId = 1, ArmTypeId = 2, BracketName = "Surcharge for face fixture A", PartNumber = null, Price = 22.00m, DateCreated = new DateTime(2026, 4, 7, 19, 20, 47, 870), CreatedBy = "System" },
                new Brackets { BracketId = 115, ProductId = 1, ArmTypeId = 3, BracketName = "Surcharge for face fixture A", PartNumber = null, Price = 32.00m, DateCreated = new DateTime(2026, 4, 7, 19, 20, 47, 870), CreatedBy = "System" },
                new Brackets { BracketId = 116, ProductId = 1, ArmTypeId = 1, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 330.00m, DateCreated = new DateTime(2026, 4, 7, 19, 20, 47, 870), CreatedBy = "System" },
                new Brackets { BracketId = 117, ProductId = 1, ArmTypeId = 2, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 339.00m, DateCreated = new DateTime(2026, 4, 7, 19, 20, 47, 870), CreatedBy = "System" },
                new Brackets { BracketId = 118, ProductId = 1, ArmTypeId = 3, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 504.00m, DateCreated = new DateTime(2026, 4, 7, 19, 20, 47, 870), CreatedBy = "System" },

                // ProductId = 2 (Markilux MX-4)
                new Brackets { BracketId = 119, ProductId = 2, ArmTypeId = 1, BracketName = "Surcharge face fixture bracket A 300 mm", PartNumber = null, Price = 44.00m, DateCreated = new DateTime(2026, 4, 7, 20, 18, 15, 206), CreatedBy = "System" },
                new Brackets { BracketId = 120, ProductId = 2, ArmTypeId = 2, BracketName = "Surcharge face fixture bracket A 300 mm", PartNumber = null, Price = 44.00m, DateCreated = new DateTime(2026, 4, 7, 20, 18, 15, 206), CreatedBy = "System" },
                new Brackets { BracketId = 121, ProductId = 2, ArmTypeId = 3, BracketName = "Surcharge face fixture bracket A 300 mm", PartNumber = null, Price = 66.00m, DateCreated = new DateTime(2026, 4, 7, 20, 18, 15, 206), CreatedBy = "System" },
                new Brackets { BracketId = 122, ProductId = 2, ArmTypeId = 1, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 330.00m, DateCreated = new DateTime(2026, 4, 7, 20, 18, 15, 206), CreatedBy = "System" },
                new Brackets { BracketId = 123, ProductId = 2, ArmTypeId = 2, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 339.00m, DateCreated = new DateTime(2026, 4, 7, 20, 18, 15, 206), CreatedBy = "System" },
                new Brackets { BracketId = 124, ProductId = 2, ArmTypeId = 3, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 504.00m, DateCreated = new DateTime(2026, 4, 7, 20, 18, 15, 206), CreatedBy = "System" },
                new Brackets { BracketId = 125, ProductId = 2, ArmTypeId = 1, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 7, 20, 18, 15, 206), CreatedBy = "System" },
                new Brackets { BracketId = 126, ProductId = 2, ArmTypeId = 2, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 7, 20, 18, 15, 206), CreatedBy = "System" },
                new Brackets { BracketId = 127, ProductId = 2, ArmTypeId = 3, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 269.00m, DateCreated = new DateTime(2026, 4, 7, 20, 18, 15, 206), CreatedBy = "System" },

                // ProductId = 13 (Markilux 6000)
                new Brackets { BracketId = 135, ProductId = 13, ArmTypeId = 4, BracketName = "Surcharge face fixture bracket A 300 mm", PartNumber = null, Price = 88.00m, DateCreated = new DateTime(2026, 4, 7, 20, 49, 38, 263), CreatedBy = "System" },
                new Brackets { BracketId = 136, ProductId = 13, ArmTypeId = 6, BracketName = "Surcharge face fixture bracket A 300 mm", PartNumber = null, Price = 88.00m, DateCreated = new DateTime(2026, 4, 7, 20, 49, 38, 263), CreatedBy = "System" },
                new Brackets { BracketId = 137, ProductId = 13, ArmTypeId = 7, BracketName = "Surcharge face fixture bracket A 300 mm", PartNumber = null, Price = 132.00m, DateCreated = new DateTime(2026, 4, 7, 20, 49, 38, 263), CreatedBy = "System" },
                new Brackets { BracketId = 138, ProductId = 13, ArmTypeId = 4, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 659.00m, DateCreated = new DateTime(2026, 4, 7, 20, 49, 38, 263), CreatedBy = "System" },
                new Brackets { BracketId = 139, ProductId = 13, ArmTypeId = 6, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 677.00m, DateCreated = new DateTime(2026, 4, 7, 20, 49, 38, 263), CreatedBy = "System" },
                new Brackets { BracketId = 140, ProductId = 13, ArmTypeId = 7, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 1007.00m, DateCreated = new DateTime(2026, 4, 7, 20, 49, 38, 263), CreatedBy = "System" },
                new Brackets { BracketId = 141, ProductId = 13, ArmTypeId = 4, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 361.00m, DateCreated = new DateTime(2026, 4, 7, 20, 49, 38, 263), CreatedBy = "System" },
                new Brackets { BracketId = 142, ProductId = 13, ArmTypeId = 6, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 361.00m, DateCreated = new DateTime(2026, 4, 7, 20, 49, 38, 263), CreatedBy = "System" },
                new Brackets { BracketId = 143, ProductId = 13, ArmTypeId = 7, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 536.00m, DateCreated = new DateTime(2026, 4, 7, 20, 49, 38, 263), CreatedBy = "System" },

                // ProductId = 3 (Markilux MX-2)
                new Brackets { BracketId = 146, ProductId = 3, ArmTypeId = 1, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 220.00m, DateCreated = new DateTime(2026, 4, 7, 21, 7, 50, 610), CreatedBy = "System" },
                new Brackets { BracketId = 147, ProductId = 3, ArmTypeId = 1, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 592.00m, DateCreated = new DateTime(2026, 4, 7, 21, 7, 50, 610), CreatedBy = "System" },
                new Brackets { BracketId = 148, ProductId = 3, ArmTypeId = 1, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 550.00m, DateCreated = new DateTime(2026, 4, 7, 21, 7, 50, 610), CreatedBy = "System" },
                new Brackets { BracketId = 149, ProductId = 3, ArmTypeId = 1, BracketName = "Surcharge for face fixture incl. spreader plate C", PartNumber = null, Price = 592.00m, DateCreated = new DateTime(2026, 4, 7, 21, 7, 50, 610), CreatedBy = "System" },
                new Brackets { BracketId = 150, ProductId = 3, ArmTypeId = 1, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 287.00m, DateCreated = new DateTime(2026, 4, 7, 21, 7, 50, 610), CreatedBy = "System" },
                new Brackets { BracketId = 151, ProductId = 3, ArmTypeId = 1, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 371.00m, DateCreated = new DateTime(2026, 4, 7, 21, 7, 50, 610), CreatedBy = "System" },
                new Brackets { BracketId = 152, ProductId = 3, ArmTypeId = 1, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 7, 21, 7, 50, 610), CreatedBy = "System" },
                new Brackets { BracketId = 153, ProductId = 3, ArmTypeId = 1, BracketName = "Surcharge for the two-tone housing, markilux \"MX- colour\" in the colour combinations 1-10", PartNumber = null, Price = 300.00m, DateCreated = new DateTime(2026, 4, 7, 21, 7, 50, 610), CreatedBy = "System" },
                new Brackets { BracketId = 154, ProductId = 3, ArmTypeId = 1, BracketName = "Face fixture bracket left / 3", PartNumber = "72826", Price = 109.80m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 155, ProductId = 3, ArmTypeId = 1, BracketName = "Face fixture bracket right / 3", PartNumber = "72827", Price = 109.80m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 156, ProductId = 3, ArmTypeId = 1, BracketName = "Stand-off bkt. 80-300 mm for face fixture bkt. / 4", PartNumber = "72872", Price = 253.50m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 157, ProductId = 3, ArmTypeId = 1, BracketName = "Top fixture bracket left / 4", PartNumber = "60523", Price = 143.20m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 158, ProductId = 3, ArmTypeId = 1, BracketName = "Top fixture bracket right / 4", PartNumber = "60524", Price = 143.20m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 159, ProductId = 3, ArmTypeId = 1, BracketName = "Eaves fixture bracket left 150 mm, complete / 4", PartNumber = "60603", Price = 185.30m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 160, ProductId = 3, ArmTypeId = 1, BracketName = "Eaves fixture bracket right 150 mm, complete / 4", PartNumber = "60604", Price = 185.30m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 161, ProductId = 3, ArmTypeId = 1, BracketName = "Eaves fixture bracket 270 mm / 4", PartNumber = "71659", Price = 79.30m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 162, ProductId = 3, ArmTypeId = 1, BracketName = "Angle and plate for eaves fixture (machine finish) / 4", PartNumber = "716620", Price = 128.90m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 163, ProductId = 3, ArmTypeId = 1, BracketName = "Additional eaves fixture plate 60x260x12 mm / 2", PartNumber = "75383", Price = 43.90m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 164, ProductId = 3, ArmTypeId = 1, BracketName = "Spreader plate A 430x160x12 mm / 8", PartNumber = "72870", Price = 186.00m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 165, ProductId = 3, ArmTypeId = 1, BracketName = "Spreader plate B 300x400x12 mm / 4", PartNumber = "73465", Price = 164.90m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 166, ProductId = 3, ArmTypeId = 1, BracketName = "Spreader Plate C 310x130x12 mm / 6", PartNumber = "72526", Price = 186.00m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 167, ProductId = 3, ArmTypeId = 1, BracketName = "Spacer block face fixture 100x120x20 mm / 3", PartNumber = "718581", Price = 14.70m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 168, ProductId = 3, ArmTypeId = 1, BracketName = "Spacer block face fixture 100x120x12 mm / 3", PartNumber = "718571", Price = 14.30m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 169, ProductId = 3, ArmTypeId = 1, BracketName = "Spacer block for top fixture 90x140x20 mm / 4", PartNumber = "716311", Price = 4.40m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 170, ProductId = 3, ArmTypeId = 1, BracketName = "Spacer block for top fixture 90x140x12 mm / 4", PartNumber = "716411", Price = 5.00m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 171, ProductId = 3, ArmTypeId = 1, BracketName = "Cover plate 230x210x2 mm", PartNumber = "71843", Price = 17.00m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },
                new Brackets { BracketId = 172, ProductId = 3, ArmTypeId = 1, BracketName = "Vertical fixture rail incl. fixing material 624291", PartNumber = "62421", Price = 180.00m, DateCreated = new DateTime(2026, 4, 7, 21, 19, 27, 960), CreatedBy = "System" },

                // ProductId = 4 (Markilux 6000)
                new Brackets { BracketId = 173, ProductId = 4, ArmTypeId = 4, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 596.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 174, ProductId = 4, ArmTypeId = 5, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 894.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 175, ProductId = 4, ArmTypeId = 7, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 1192.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 176, ProductId = 4, ArmTypeId = 4, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 1107.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 177, ProductId = 4, ArmTypeId = 5, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 1421.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 178, ProductId = 4, ArmTypeId = 7, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 1975.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 179, ProductId = 4, ArmTypeId = 4, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 1256.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 180, ProductId = 4, ArmTypeId = 5, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 1570.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 181, ProductId = 4, ArmTypeId = 7, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 2198.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 182, ProductId = 4, ArmTypeId = 4, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 794.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 183, ProductId = 4, ArmTypeId = 5, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 1191.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 184, ProductId = 4, ArmTypeId = 7, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 1588.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 185, ProductId = 4, ArmTypeId = 4, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 950.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 186, ProductId = 4, ArmTypeId = 5, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 1424.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 187, ProductId = 4, ArmTypeId = 7, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 1899.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 188, ProductId = 4, ArmTypeId = 4, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 361.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 189, ProductId = 4, ArmTypeId = 5, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 361.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },
                new Brackets { BracketId = 190, ProductId = 4, ArmTypeId = 7, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 536.00m, DateCreated = new DateTime(2026, 4, 8, 17, 43, 59, 706), CreatedBy = "System" },

                // ProductId = 14 (Markilux 779)
                new Brackets { BracketId = 193, ProductId = 14, ArmTypeId = 1, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 298.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 194, ProductId = 14, ArmTypeId = 8, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 447.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 195, ProductId = 14, ArmTypeId = 3, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 596.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 196, ProductId = 14, ArmTypeId = 1, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 554.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 197, ProductId = 14, ArmTypeId = 8, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 711.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 198, ProductId = 14, ArmTypeId = 3, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 988.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 199, ProductId = 14, ArmTypeId = 1, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 628.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 200, ProductId = 14, ArmTypeId = 8, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 785.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 201, ProductId = 14, ArmTypeId = 3, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 1099.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 202, ProductId = 14, ArmTypeId = 1, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 397.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 203, ProductId = 14, ArmTypeId = 8, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 596.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 204, ProductId = 14, ArmTypeId = 3, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 794.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 205, ProductId = 14, ArmTypeId = 1, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 475.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 206, ProductId = 14, ArmTypeId = 8, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 712.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 207, ProductId = 14, ArmTypeId = 3, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 950.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 208, ProductId = 14, ArmTypeId = 1, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 209, ProductId = 14, ArmTypeId = 8, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },
                new Brackets { BracketId = 210, ProductId = 14, ArmTypeId = 3, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 269.00m, DateCreated = new DateTime(2026, 4, 8, 19, 48, 32, 840), CreatedBy = "System" },

                // ProductId = 8 (Markilux 5010)
                new Brackets { BracketId = 226, ProductId = 8, ArmTypeId = 1, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 263.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 227, ProductId = 8, ArmTypeId = 8, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 394.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 228, ProductId = 8, ArmTypeId = 3, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 525.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 229, ProductId = 8, ArmTypeId = 1, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 518.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 230, ProductId = 8, ArmTypeId = 8, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 653.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 231, ProductId = 8, ArmTypeId = 3, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 912.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 232, ProductId = 8, ArmTypeId = 1, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 593.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 233, ProductId = 8, ArmTypeId = 8, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 728.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 234, ProductId = 8, ArmTypeId = 3, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 1024.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 235, ProductId = 8, ArmTypeId = 1, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 326.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 236, ProductId = 8, ArmTypeId = 8, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 489.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 237, ProductId = 8, ArmTypeId = 3, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 652.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 238, ProductId = 8, ArmTypeId = 1, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 404.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 239, ProductId = 8, ArmTypeId = 8, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 605.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 240, ProductId = 8, ArmTypeId = 3, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 807.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 241, ProductId = 8, ArmTypeId = 1, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 242, ProductId = 8, ArmTypeId = 8, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 243, ProductId = 8, ArmTypeId = 3, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 269.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 244, ProductId = 8, ArmTypeId = 1, BracketName = "Surcharge for arms with bionic tendon", PartNumber = null, Price = 121.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 245, ProductId = 8, ArmTypeId = 8, BracketName = "Surcharge for arms with bionic tendon", PartNumber = null, Price = 121.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },
                new Brackets { BracketId = 246, ProductId = 8, ArmTypeId = 3, BracketName = "Surcharge for arms with bionic tendon", PartNumber = null, Price = 177.00m, DateCreated = new DateTime(2026, 4, 9, 8, 23, 24, 643), CreatedBy = "System" },

                // ProductId = 15 (Markilux 8800)
                new Brackets { BracketId = 262, ProductId = 15, ArmTypeId = 4, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 525.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 263, ProductId = 15, ArmTypeId = 5, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 788.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 264, ProductId = 15, ArmTypeId = 7, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 1050.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 265, ProductId = 15, ArmTypeId = 4, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 1036.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 266, ProductId = 15, ArmTypeId = 5, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 1306.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 267, ProductId = 15, ArmTypeId = 7, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 1824.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 268, ProductId = 15, ArmTypeId = 4, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 1185.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 269, ProductId = 15, ArmTypeId = 5, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 1455.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 270, ProductId = 15, ArmTypeId = 7, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 2047.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 271, ProductId = 15, ArmTypeId = 4, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 652.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 272, ProductId = 15, ArmTypeId = 5, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 978.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 273, ProductId = 15, ArmTypeId = 7, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 1304.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 274, ProductId = 15, ArmTypeId = 4, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 807.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 275, ProductId = 15, ArmTypeId = 5, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 1210.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 276, ProductId = 15, ArmTypeId = 7, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 1613.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 277, ProductId = 15, ArmTypeId = 4, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 361.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 278, ProductId = 15, ArmTypeId = 5, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 361.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 279, ProductId = 15, ArmTypeId = 7, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 536.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 280, ProductId = 15, ArmTypeId = 4, BracketName = "Surcharge for arms with bionic tendon", PartNumber = null, Price = 234.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 281, ProductId = 15, ArmTypeId = 5, BracketName = "Surcharge for arms with bionic tendon", PartNumber = null, Price = 234.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 282, ProductId = 15, ArmTypeId = 7, BracketName = "Surcharge for arms with bionic tendon", PartNumber = null, Price = 348.00m, DateCreated = new DateTime(2026, 4, 9, 9, 15, 44, 326), CreatedBy = "System" },
                new Brackets { BracketId = 305, ProductId = 15, ArmTypeId = 1, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 9, 9, 51, 21, 433), CreatedBy = "System" },
                new Brackets { BracketId = 306, ProductId = 15, ArmTypeId = 9, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 9, 9, 51, 21, 433), CreatedBy = "System" },
                new Brackets { BracketId = 307, ProductId = 15, ArmTypeId = 11, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 9, 9, 51, 21, 433), CreatedBy = "System" },
                new Brackets { BracketId = 308, ProductId = 15, ArmTypeId = 10, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 269.00m, DateCreated = new DateTime(2026, 4, 9, 9, 51, 21, 433), CreatedBy = "System" },

                // ProductId = 9 (Markilux 3300)
                new Brackets { BracketId = 309, ProductId = 9, ArmTypeId = 1, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 79.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 310, ProductId = 9, ArmTypeId = 9, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 130.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 311, ProductId = 9, ArmTypeId = 11, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 156.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 312, ProductId = 9, ArmTypeId = 10, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 195.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 313, ProductId = 9, ArmTypeId = 1, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 335.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 314, ProductId = 9, ArmTypeId = 9, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 394.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 315, ProductId = 9, ArmTypeId = 11, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 424.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 316, ProductId = 9, ArmTypeId = 10, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 591.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 317, ProductId = 9, ArmTypeId = 1, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 409.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 318, ProductId = 9, ArmTypeId = 9, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 468.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 319, ProductId = 9, ArmTypeId = 11, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 498.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 320, ProductId = 9, ArmTypeId = 10, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 702.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 321, ProductId = 9, ArmTypeId = 1, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 140.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 322, ProductId = 9, ArmTypeId = 9, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 279.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 323, ProductId = 9, ArmTypeId = 11, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 348.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 324, ProductId = 9, ArmTypeId = 10, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 418.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 325, ProductId = 9, ArmTypeId = 1, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 246.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 326, ProductId = 9, ArmTypeId = 9, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 492.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 327, ProductId = 9, ArmTypeId = 11, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 615.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 328, ProductId = 9, ArmTypeId = 10, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 738.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 329, ProductId = 9, ArmTypeId = 1, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 330, ProductId = 9, ArmTypeId = 9, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 331, ProductId = 9, ArmTypeId = 11, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },
                new Brackets { BracketId = 332, ProductId = 9, ArmTypeId = 10, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 269.00m, DateCreated = new DateTime(2026, 4, 9, 9, 52, 58, 963), CreatedBy = "System" },

                // ProductId = 16 (Markilux 6000 XXL)
                new Brackets { BracketId = 351, ProductId = 16, ArmTypeId = 12, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 158.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 352, ProductId = 16, ArmTypeId = 13, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 260.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 353, ProductId = 16, ArmTypeId = 14, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 311.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 354, ProductId = 16, ArmTypeId = 15, BracketName = "Surcharge for face fixture", PartNumber = null, Price = 390.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 355, ProductId = 16, ArmTypeId = 12, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 669.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 356, ProductId = 16, ArmTypeId = 13, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 788.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 357, ProductId = 16, ArmTypeId = 14, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 847.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 358, ProductId = 16, ArmTypeId = 15, BracketName = "Surcharge for face fixture incl. spreader plate A", PartNumber = null, Price = 1181.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 359, ProductId = 16, ArmTypeId = 12, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 818.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 360, ProductId = 16, ArmTypeId = 13, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 936.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 361, ProductId = 16, ArmTypeId = 14, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 996.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 362, ProductId = 16, ArmTypeId = 15, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 1404.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 363, ProductId = 16, ArmTypeId = 12, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 279.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 364, ProductId = 16, ArmTypeId = 13, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 557.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 365, ProductId = 16, ArmTypeId = 14, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 696.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 366, ProductId = 16, ArmTypeId = 15, BracketName = "Surcharge for top fixture", PartNumber = null, Price = 836.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 367, ProductId = 16, ArmTypeId = 12, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 492.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 368, ProductId = 16, ArmTypeId = 13, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 984.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 369, ProductId = 16, ArmTypeId = 14, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 1230.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 370, ProductId = 16, ArmTypeId = 15, BracketName = "Surcharge for eaves fixture", PartNumber = null, Price = 1476.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 371, ProductId = 16, ArmTypeId = 12, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 361.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 372, ProductId = 16, ArmTypeId = 13, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 361.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 373, ProductId = 16, ArmTypeId = 14, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 361.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" },
                new Brackets { BracketId = 374, ProductId = 16, ArmTypeId = 15, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 536.00m, DateCreated = new DateTime(2026, 4, 9, 10, 25, 31, 153), CreatedBy = "System" }
            );
            //Arms Data — values moved to Brackets above; Arms table left empty
            modelBuilder.Entity<Arms>().HasData(
            );
            //Motors Data
            modelBuilder.Entity<Motors>().HasData(
                new Motors { MotorId = 1, ProductId = 6, Description = "Surcharge for servo-assisted gear", Price = 72m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Motors { MotorId = 2, ProductId = 6, Description = "Surcharge for hard-wired motor", Price = 470m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Motors { MotorId = 3, ProductId = 6, Description = "Surcharge for radio-contr. motor io/RTS + 1 ch. transmitter", Price = 700m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Motors { MotorId = 4, ProductId = 6, Description = "Surcharge for radio-contr. motor io/RTS w/o transmitter", Price = 586m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Motors { MotorId = 5, ProductId = 6, Description = "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", Price = 1082m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Motors { MotorId = 6, ProductId = 6, Description = "Surcharge for radio-contr. motor io with manual override w/o transmitter", Price = 968m, DateCreated = staticCreatedDate, CreatedBy = "System" }
             );
            //ValanceStyle Data
            modelBuilder.Entity<ValanceStyle>().HasData(
                new ValanceStyle { ValanceStyleId = 1, ProductId = 6, WidthCm = 250, Price = 76m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new ValanceStyle { ValanceStyleId = 2, ProductId = 6, WidthCm = 300, Price = 84m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new ValanceStyle { ValanceStyleId = 3, ProductId = 6, WidthCm = 350, Price = 93m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new ValanceStyle { ValanceStyleId = 4, ProductId = 6, WidthCm = 400, Price = 105m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new ValanceStyle { ValanceStyleId = 5, ProductId = 6, WidthCm = 450, Price = 118m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new ValanceStyle { ValanceStyleId = 6, ProductId = 6, WidthCm = 500, Price = 130m, DateCreated = staticCreatedDate, CreatedBy = "System" }
             );
            //NonStandardRALColours Data
            modelBuilder.Entity<NonStandardRALColours>().HasData(
                new NonStandardRALColours { RALColourId = 1, ProductId = 6, WidthCm = 250, Price = 319m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new NonStandardRALColours { RALColourId = 2, ProductId = 6, WidthCm = 300, Price = 342m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new NonStandardRALColours { RALColourId = 3, ProductId = 6, WidthCm = 350, Price = 360m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new NonStandardRALColours { RALColourId = 4, ProductId = 6, WidthCm = 400, Price = 377m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new NonStandardRALColours { RALColourId = 5, ProductId = 6, WidthCm = 450, Price = 394m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new NonStandardRALColours { RALColourId = 6, ProductId = 6, WidthCm = 500, Price = 411m, DateCreated = staticCreatedDate, CreatedBy = "System" }
             );
            //WallSealingProfile Data
            modelBuilder.Entity<WallSealingProfile>().HasData(
                new WallSealingProfile { WallSealingProfileId = 1, ProductId = 6, WidthCm = 250, Price = 87m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new WallSealingProfile { WallSealingProfileId = 2, ProductId = 6, WidthCm = 300, Price = 101m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new WallSealingProfile { WallSealingProfileId = 3, ProductId = 6, WidthCm = 350, Price = 110m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new WallSealingProfile { WallSealingProfileId = 4, ProductId = 6, WidthCm = 400, Price = 126m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new WallSealingProfile { WallSealingProfileId = 5, ProductId = 6, WidthCm = 450, Price = 141m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new WallSealingProfile { WallSealingProfileId = 6, ProductId = 6, WidthCm = 500, Price = 157m, DateCreated = staticCreatedDate, CreatedBy = "System" }
             );
            //Heaters Data
            modelBuilder.Entity<Heaters>().HasData(
                new Heaters { HeaterId = 1, ProductId = 6, Description = "Markilux Infrared Heater 2500 watt Dimmable", Price = 1393m, PriceNonRALColour = 1635m, DateCreated = staticCreatedDate, CreatedBy = "System" }
             );


            // Seed SiteVisitValues
            modelBuilder.Entity<SiteVisitValues>().HasData(
                // Model
                new SiteVisitValues { Id = 1, Category = "Model", Value = "Renson", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 2, Category = "Model", Value = "Practic", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 3, Category = "Model", Value = "Markilux", DisplayOrder = 3, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Structure
                new SiteVisitValues { Id = 4, Category = "Structure", Value = "Free Standing", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 5, Category = "Structure", Value = "Mounted to Building", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Wall Type
                new SiteVisitValues { Id = 6, Category = "WallType", Value = "Red Brick", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 7, Category = "WallType", Value = "Block", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 8, Category = "WallType", Value = "External Insulation", DisplayOrder = 3, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // External Insulation
                new SiteVisitValues { Id = 9, Category = "ExternalInsulation", Value = "Yes", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 10, Category = "ExternalInsulation", Value = "No", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Wall Finish
                new SiteVisitValues { Id = 11, Category = "WallFinish", Value = "Rendered", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 12, Category = "WallFinish", Value = "Smooth", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 13, Category = "WallFinish", Value = "Pebbledash", DisplayOrder = 3, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Flashing Required
                new SiteVisitValues { Id = 14, Category = "FlashingRequired", Value = "Yes", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 15, Category = "FlashingRequired", Value = "No", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Stand of Brackets
                new SiteVisitValues { Id = 16, Category = "StandOfBrackets", Value = "Yes", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 17, Category = "StandOfBrackets", Value = "No", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Electrician
                new SiteVisitValues { Id = 18, Category = "Electrician", Value = "Ours", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 19, Category = "Electrician", Value = "Own", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Electrical Connection
                new SiteVisitValues { Id = 20, Category = "ElectricalConnection", Value = "Plug in", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 21, Category = "ElectricalConnection", Value = "Hard Wired", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Fixture Type
                new SiteVisitValues { Id = 22, Category = "FixtureType", Value = "Face Fix", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 23, Category = "FixtureType", Value = "Top Fix", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 24, Category = "FixtureType", Value = "Recess", DisplayOrder = 3, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Operation
                new SiteVisitValues { Id = 25, Category = "Operation", Value = "Manual", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 26, Category = "Operation", Value = "Motorised", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Operation Side
                new SiteVisitValues { Id = 27, Category = "OperationSide", Value = "Right", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 28, Category = "OperationSide", Value = "Left", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Valance Choice
                new SiteVisitValues { Id = 29, Category = "ValanceChoice", Value = "Yes", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 30, Category = "ValanceChoice", Value = "No", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Wind Sensor
                new SiteVisitValues { Id = 31, Category = "WindSensor", Value = "Vibrabox", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 32, Category = "WindSensor", Value = "Anemometer", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // ShadePlus Required
                new SiteVisitValues { Id = 33, Category = "ShadePlusRequired", Value = "Yes", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 34, Category = "ShadePlusRequired", Value = "No", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Shade Type
                new SiteVisitValues { Id = 35, Category = "ShadeType", Value = "Manual", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 36, Category = "ShadeType", Value = "Motorised", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Lights
                new SiteVisitValues { Id = 37, Category = "Lights", Value = "Yes", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 38, Category = "Lights", Value = "No", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Lights Type
                new SiteVisitValues { Id = 39, Category = "LightsType", Value = "Spot Lights", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 40, Category = "LightsType", Value = "LED Line", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 41, Category = "LightsType", Value = "Other", DisplayOrder = 3, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Heater
                new SiteVisitValues { Id = 42, Category = "Heater", Value = "Yes", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 43, Category = "Heater", Value = "No", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Heater Manufacturer
                new SiteVisitValues { Id = 44, Category = "HeaterManufacturer", Value = "Markilux", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 45, Category = "HeaterManufacturer", Value = "Bromic", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 46, Category = "HeaterManufacturer", Value = "Other", DisplayOrder = 3, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Heater Output
                new SiteVisitValues { Id = 47, Category = "HeaterOutput", Value = "2kw", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 48, Category = "HeaterOutput", Value = "2.5kw", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 49, Category = "HeaterOutput", Value = "3kw", DisplayOrder = 3, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 50, Category = "HeaterOutput", Value = "4kw", DisplayOrder = 4, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 51, Category = "HeaterOutput", Value = "6kw", DisplayOrder = 5, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 52, Category = "HeaterOutput", Value = "Other", DisplayOrder = 6, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Remote Control
                new SiteVisitValues { Id = 53, Category = "RemoteControl", Value = "Handheld", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 54, Category = "RemoteControl", Value = "Wall Mounted", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Controller Box (MODIFIED - "On" changed to "On Off", "Off" removed, DisplayOrder adjusted)
                new SiteVisitValues { Id = 55, Category = "ControllerBox", Value = "On Off", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 57, Category = "ControllerBox", Value = "Dimmable", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Side Infills (NEW CATEGORY)
                new SiteVisitValues { Id = 58, Category = "SideInfills", Value = "P1", DisplayOrder = 1, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 59, Category = "SideInfills", Value = "P2", DisplayOrder = 2, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 60, Category = "SideInfills", Value = "S1", DisplayOrder = 3, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new SiteVisitValues { Id = 61, Category = "SideInfills", Value = "S2", DisplayOrder = 4, IsActive = true, DateCreated = staticCreatedDate, CreatedBy = "System" }
            );

            //Arms Type
            modelBuilder.Entity<ArmsType>().HasData(
                 new ArmsType { ArmTypeId = 1, Description = "2-0-2", DateCreated = staticCreatedDate, CreatedBy = "System" },
                 new ArmsType { ArmTypeId = 2, Description = "2-1-3", DateCreated = staticCreatedDate, CreatedBy = "System" },
                 new ArmsType { ArmTypeId = 3, Description = "3-2-4", DateCreated = staticCreatedDate, CreatedBy = "System" },
                 new ArmsType { ArmTypeId = 4, Description = "4-0-4", DateCreated = new DateTime(2026, 4, 6, 13, 0, 30), CreatedBy = "System" },
                 new ArmsType { ArmTypeId = 5, Description = "4-0-6", DateCreated = new DateTime(2026, 4, 6, 13, 0, 30), CreatedBy = "System" },
                 new ArmsType { ArmTypeId = 6, Description = "4-2-6", DateCreated = new DateTime(2026, 4, 6, 13, 0, 30), CreatedBy = "System" },
                 new ArmsType { ArmTypeId = 7, Description = "6-4-8", DateCreated = new DateTime(2026, 4, 6, 13, 0, 30), CreatedBy = "System" },
                 new ArmsType { ArmTypeId = 8, Description = "2-0-3", DateCreated = new DateTime(2026, 4, 8, 19, 35, 51), CreatedBy = "System" },
                 new ArmsType { ArmTypeId = 9, Description = "2-0-4", DateCreated = new DateTime(2026, 4, 9, 9, 27, 27), CreatedBy = "System" },
                 new ArmsType { ArmTypeId = 10, Description = "3-2-6", DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System" },
                 new ArmsType { ArmTypeId = 11, Description = "2-1-5", DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System" },
                 new ArmsType { ArmTypeId = 12, Description = "4-0-5", DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System" },
                 new ArmsType { ArmTypeId = 13, Description = "4-0-7", DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System" },
                 new ArmsType { ArmTypeId = 14, Description = "4-2-9", DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System" },
                 new ArmsType { ArmTypeId = 15, Description = "6-4-11", DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System" }
            );
            //RadioControlled Motors
            modelBuilder.Entity<RadioControlledMotors>().HasData(
                 new RadioControlledMotors { RadioMotorId = 1, Description = "RadioControlled Motor", Width_cm = 250, Price = 1547, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                 new RadioControlledMotors { RadioMotorId = 2, Description = "RadioControlled Motor", Width_cm = 300, Price = 1603, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                 new RadioControlledMotors { RadioMotorId = 3, Description = "RadioControlled Motor", Width_cm = 350, Price = 1678, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                 new RadioControlledMotors { RadioMotorId = 4, Description = "RadioControlled Motor", Width_cm = 400, Price = 1763, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                 new RadioControlledMotors { RadioMotorId = 5, Description = "RadioControlled Motor", Width_cm = 450, Price = 1822, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                 new RadioControlledMotors { RadioMotorId = 6, Description = "RadioControlled Motor", Width_cm = 500, Price = 1896, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                 new RadioControlledMotors { RadioMotorId = 7, Description = "RadioControlled Motor", Width_cm = 550, Price = 1986, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                 new RadioControlledMotors { RadioMotorId = 8, Description = "RadioControlled Motor", Width_cm = 600, Price = 2068, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                 new RadioControlledMotors { RadioMotorId = 9, Description = "RadioControlled Motor", Width_cm = 650, Price = 2143, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                 new RadioControlledMotors { RadioMotorId = 10, Description = "RadioControlled Motor", Width_cm = 700, Price = 2219, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 }
            );

            //Control Data
            modelBuilder.Entity<Control>().HasData(
                new Control { ControlId = 1, Description = "markilux io-5 designcontrol transmitter - 5 channel", PartNumber = "8272099", Price = 154.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 2 },
                new Control { ControlId = 2, Description = "Somfy TaHoma Switch", PartNumber = "8272377", Price = 362.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 2 },
                new Control { ControlId = 3, Description = "markilux io-1 designcontrol transmitter - 1 channel", PartNumber = "8272087", Price = 118.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new Control { ControlId = 4, Description = "markilux io-5 designcontrol transmitter - 5 channel", PartNumber = "8272099", Price = 154.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 }
            );

            //ShadePlus Data
            modelBuilder.Entity<ShadePlus>().HasData(
                new ShadePlus { ShadePlusId = 1, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 250, Price = 1592.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 2, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 300, Price = 1650.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 3, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 350, Price = 1727.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 4, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 400, Price = 1815.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 5, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 450, Price = 1875.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 6, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 500, Price = 1951.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 7, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 550, Price = 2044.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 8, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 600, Price = 2128.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 9, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 650, Price = 2206.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 10, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 700, Price = 2284.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 11, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 500, Price = 1424.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 12, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 600, Price = 1581.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 13, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 700, Price = 1726.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 14, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 800, Price = 1878.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 15, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 900, Price = 2027.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 16, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 1000, Price = 2179.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 17, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 1100, Price = 2329.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 18, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 1200, Price = 2479.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 19, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 1300, Price = 2628.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 20, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 1390, Price = 2779.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 21, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 500, Price = 3060.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 22, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 600, Price = 3183.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 23, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 700, Price = 3374.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 24, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 800, Price = 3546.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 25, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 900, Price = 3649.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 26, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 1000, Price = 3787.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 27, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 1100, Price = 3959.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 28, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 1200, Price = 4128.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 29, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 1300, Price = 4318.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 30, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 1390, Price = 4511.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 31, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 500, Price = 3325.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 32, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 600, Price = 3449.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 33, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 700, Price = 3638.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 34, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 800, Price = 3809.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 35, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 900, Price = 3913.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 36, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 1000, Price = 4053.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 37, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 1100, Price = 4225.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 38, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 1200, Price = 4392.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 39, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 1300, Price = 4585.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 40, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 1390, Price = 4776.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 4 },
                new ShadePlus { ShadePlusId = 41, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 250, Price = 719.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 42, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 300, Price = 798.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 43, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 350, Price = 865.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 44, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 400, Price = 945.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 45, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 450, Price = 1019.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 46, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 500, Price = 1096.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 47, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 550, Price = 1173.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 48, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 600, Price = 1248.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 49, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 650, Price = 1319.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 50, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 700, Price = 1396.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 51, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 250, Price = 1538.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 52, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 300, Price = 1596.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 53, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 350, Price = 1693.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 54, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 400, Price = 1778.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 55, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 450, Price = 1830.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 56, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 500, Price = 1898.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 57, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 550, Price = 1986.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 58, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 600, Price = 2070.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 59, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 650, Price = 2163.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 60, Description = "Surcharge for height 210 cm with hard-wired motor", WidthCm = 700, Price = 2262.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 61, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 250, Price = 1669.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 62, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 300, Price = 1729.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 63, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 350, Price = 1828.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 64, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 400, Price = 1911.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 65, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 450, Price = 1961.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 66, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 500, Price = 2032.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 67, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 550, Price = 2119.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 68, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 600, Price = 2204.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 69, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 650, Price = 2297.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 70, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", WidthCm = 700, Price = 2395.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 14 },
                new ShadePlus { ShadePlusId = 71, Description = "Surcharge for height 170 cm with gearbox", WidthCm = 250, Price = 718.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 72, Description = "Surcharge for height 170 cm with gearbox", WidthCm = 300, Price = 797.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 73, Description = "Surcharge for height 170 cm with gearbox", WidthCm = 350, Price = 865.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 74, Description = "Surcharge for height 170 cm with gearbox", WidthCm = 400, Price = 944.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 75, Description = "Surcharge for height 170 cm with gearbox", WidthCm = 450, Price = 1018.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 76, Description = "Surcharge for height 170 cm with gearbox", WidthCm = 500, Price = 1095.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 77, Description = "Surcharge for height 170 cm with gearbox", WidthCm = 550, Price = 1170.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 78, Description = "Surcharge for height 170 cm with gearbox", WidthCm = 600, Price = 1247.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 79, Description = "Surcharge for height 170 cm with hard-wired motor", WidthCm = 250, Price = 1538.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 80, Description = "Surcharge for height 170 cm with hard-wired motor", WidthCm = 300, Price = 1596.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 81, Description = "Surcharge for height 170 cm with hard-wired motor", WidthCm = 350, Price = 1693.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 82, Description = "Surcharge for height 170 cm with hard-wired motor", WidthCm = 400, Price = 1777.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 83, Description = "Surcharge for height 170 cm with hard-wired motor", WidthCm = 450, Price = 1829.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 84, Description = "Surcharge for height 170 cm with hard-wired motor", WidthCm = 500, Price = 1898.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 85, Description = "Surcharge for height 170 cm with hard-wired motor", WidthCm = 550, Price = 1986.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 86, Description = "Surcharge for height 170 cm with hard-wired motor", WidthCm = 600, Price = 2069.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 87, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", WidthCm = 250, Price = 1667.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 88, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", WidthCm = 300, Price = 1729.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 89, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", WidthCm = 350, Price = 1827.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 90, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", WidthCm = 400, Price = 1911.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 91, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", WidthCm = 450, Price = 1960.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 92, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", WidthCm = 500, Price = 2032.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 93, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", WidthCm = 550, Price = 2119.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 94, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", WidthCm = 600, Price = 2204.00m, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System", ProductId = 7 },
                new ShadePlus { ShadePlusId = 95, Description = null, WidthCm = 250, Price = 719.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 8 },
                new ShadePlus { ShadePlusId = 96, Description = null, WidthCm = 300, Price = 798.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 8 },
                new ShadePlus { ShadePlusId = 97, Description = null, WidthCm = 350, Price = 865.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 8 },
                new ShadePlus { ShadePlusId = 98, Description = null, WidthCm = 400, Price = 945.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 8 },
                new ShadePlus { ShadePlusId = 99, Description = null, WidthCm = 450, Price = 1019.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 8 },
                new ShadePlus { ShadePlusId = 100, Description = null, WidthCm = 500, Price = 1096.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 8 },
                new ShadePlus { ShadePlusId = 101, Description = null, WidthCm = 550, Price = 1173.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 8 },
                new ShadePlus { ShadePlusId = 102, Description = null, WidthCm = 600, Price = 1248.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 8 },
                new ShadePlus { ShadePlusId = 103, Description = null, WidthCm = 650, Price = 1319.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 8 },
                new ShadePlus { ShadePlusId = 104, Description = null, WidthCm = 700, Price = 1396.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 8 },
                new ShadePlus { ShadePlusId = 105, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 500, Price = 1424.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 15 },
                new ShadePlus { ShadePlusId = 106, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 600, Price = 1581.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 15 },
                new ShadePlus { ShadePlusId = 107, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 700, Price = 1726.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 15 },
                new ShadePlus { ShadePlusId = 108, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 800, Price = 1878.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 15 },
                new ShadePlus { ShadePlusId = 109, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 900, Price = 2027.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 15 },
                new ShadePlus { ShadePlusId = 110, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 1000, Price = 2179.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 15 },
                new ShadePlus { ShadePlusId = 111, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 1100, Price = 2329.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 15 },
                new ShadePlus { ShadePlusId = 112, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 1200, Price = 2479.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 15 },
                new ShadePlus { ShadePlusId = 113, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 1300, Price = 2628.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 15 },
                new ShadePlus { ShadePlusId = 114, Description = "Surcharge for height 210 cm with gearbox", WidthCm = 1390, Price = 2779.00m, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System", ProductId = 15 }
            );

            //LightingCassette Data
            modelBuilder.Entity<LightingCassette>().HasData(
                new LightingCassette { LightingId = 1, Description = "Surcharge for LED Line RGB-WW Radio-controlled io - dimmable (without remote control)", Price = 1555.00m, DateCreated = new DateTime(2026, 4, 6), CreatedBy = "System", ProductId = 5 },
                new LightingCassette { LightingId = 2, Description = "Surcharge for LED Line RGB-WW Zigbee radio control - dimmable (without transmitter)", Price = 1386.00m, DateCreated = new DateTime(2026, 4, 6), CreatedBy = "System", ProductId = 5 },
                new LightingCassette { LightingId = 3, Description = "Surcharge for LED Line RGB-WW Radio-controlled io - dimmable (without remote control)", Price = 1555.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 3 },
                new LightingCassette { LightingId = 4, Description = "Surcharge for LED Line RGB-WW Zigbee radio control - dimmable (without transmitter)", Price = 1386.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 3 }
            );
        }
    }
}