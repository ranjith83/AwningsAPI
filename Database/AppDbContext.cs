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
                new Product { ProductId = 2, Description = "Markilux MX-4 Single", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 3, Description = "Markilux MX-4 Coupler", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 4, Description = "Markilux MX-2", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 5, Description = "Markilux 6000 Single", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 6, Description = "Markilux 6000 Coupler", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 7, Description = "Markilux MX-3", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 8, Description = "Markilux 990", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 9, Description = "Markilux 970", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 10, Description = "Markilux 5010 Single", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 11, Description = "Markilux 5010 Coupler", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Product { ProductId = 12, Description = "Markilux 3300 Single", ProductTypeId = 1, SupplierId = 1, DateCreated = new DateTime(2026, 4, 6), CreatedBy = "System" },
                new Product { ProductId = 13, Description = "Markilux 3300 Coupler", ProductTypeId = 1, SupplierId = 1, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System" },
                new Product { ProductId = 14, Description = "Markilux 1710", ProductTypeId = 1, SupplierId = 1, DateCreated = new DateTime(2026, 4, 8), CreatedBy = "System" },
                new Product { ProductId = 15, Description = "Markilux 900", ProductTypeId = 1, SupplierId = 1, DateCreated = new DateTime(2026, 4, 9), CreatedBy = "System" }
            );
            //Workflow Start
            modelBuilder.Entity<WorkflowStart>().HasData(
                new WorkflowStart { WorkflowId = 1, CustomerId = 1, Description = "Markilux 990 for outside garden", DateCreated = staticCreatedDate, CreatedBy = "System", SupplierId = 1, ProductTypeId = 1, ProductId = 6 }
            );
            //Projection Data
            // Projections for Markilux MX-1 compact (ProductId = 1)
            // Note: property names use Width_cm and Projection_cm as requested.
            // Ensure staticCreatedDate is defined earlier in OnModelCreating.
            modelBuilder.Entity<Projections>().HasData(
                new Projections { ProjectionId = 1, Width_cm = 250, Projection_cm = 165, Price = 4639m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 2 },
                new Projections { ProjectionId = 2, Width_cm = 300, Projection_cm = 165, Price = 4821m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 2 },
                new Projections { ProjectionId = 3, Width_cm = 350, Projection_cm = 165, Price = 5050m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 2 },
                new Projections { ProjectionId = 4, Width_cm = 400, Projection_cm = 165, Price = 5278m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 2 },
                new Projections { ProjectionId = 5, Width_cm = 450, Projection_cm = 165, Price = 5525m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 6, Width_cm = 500, Projection_cm = 165, Price = 6111m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 7, Width_cm = 550, Projection_cm = 165, Price = 6414m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 8, Width_cm = 600, Projection_cm = 165, Price = 6806m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 9, Width_cm = 650, Projection_cm = 165, Price = 7430m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 10, Width_cm = 700, Projection_cm = 165, Price = 8247m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 8 },

                new Projections { ProjectionId = 11, Width_cm = 300, Projection_cm = 215, Price = 5037m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 2 },
                new Projections { ProjectionId = 12, Width_cm = 350, Projection_cm = 215, Price = 5282m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 2 },
                new Projections { ProjectionId = 13, Width_cm = 400, Projection_cm = 215, Price = 5524m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 2 },
                new Projections { ProjectionId = 14, Width_cm = 450, Projection_cm = 215, Price = 5775m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 15, Width_cm = 500, Projection_cm = 215, Price = 6379m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 16, Width_cm = 550, Projection_cm = 215, Price = 6683m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 17, Width_cm = 600, Projection_cm = 215, Price = 7058m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 18, Width_cm = 650, Projection_cm = 215, Price = 7733m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 19, Width_cm = 700, Projection_cm = 215, Price = 8551m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 8 },

                new Projections { ProjectionId = 20, Width_cm = 350, Projection_cm = 265, Price = 5506m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 2 },
                new Projections { ProjectionId = 21, Width_cm = 400, Projection_cm = 265, Price = 5747m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 2 },
                new Projections { ProjectionId = 22, Width_cm = 450, Projection_cm = 265, Price = 6025m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 23, Width_cm = 500, Projection_cm = 265, Price = 6686m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 24, Width_cm = 550, Projection_cm = 265, Price = 7023m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 25, Width_cm = 600, Projection_cm = 265, Price = 7346m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 26, Width_cm = 650, Projection_cm = 265, Price = 8029m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 27, Width_cm = 700, Projection_cm = 265, Price = 8883m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 8 },

                new Projections { ProjectionId = 28, Width_cm = 400, Projection_cm = 315, Price = 5961m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 2 },
                new Projections { ProjectionId = 29, Width_cm = 450, Projection_cm = 315, Price = 6249m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 30, Width_cm = 500, Projection_cm = 315, Price = 6939m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 31, Width_cm = 550, Projection_cm = 315, Price = 7278m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 32, Width_cm = 600, Projection_cm = 315, Price = 7581m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 33, Width_cm = 650, Projection_cm = 315, Price = 8318m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 34, Width_cm = 700, Projection_cm = 315, Price = 9181m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 8 },

                new Projections { ProjectionId = 35, Width_cm = 450, Projection_cm = 365, Price = 6872m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 36, Width_cm = 500, Projection_cm = 365, Price = 7383m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 37, Width_cm = 550, Projection_cm = 365, Price = 7744m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 38, Width_cm = 600, Projection_cm = 365, Price = 8144m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 39, Width_cm = 650, Projection_cm = 365, Price = 9007m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 8 },
                new Projections { ProjectionId = 40, Width_cm = 700, Projection_cm = 365, Price = 9442m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 8 },

                new Projections { ProjectionId = 41, Width_cm = 500, Projection_cm = 415, Price = 8009m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 42, Width_cm = 550, Projection_cm = 415, Price = 8362m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 5 },
                new Projections { ProjectionId = 43, Width_cm = 700, Projection_cm = 415, Price = 9712m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, ArmTypeId = 8 },

                new Projections { ProjectionId = 44, Width_cm = 250, Projection_cm = 150, Price = 4931m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 45, Width_cm = 300, Projection_cm = 150, Price = 5132m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 46, Width_cm = 350, Projection_cm = 150, Price = 5382m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 47, Width_cm = 400, Projection_cm = 150, Price = 5634m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 48, Width_cm = 450, Projection_cm = 150, Price = 5840m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 49, Width_cm = 500, Projection_cm = 150, Price = 6484m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 50, Width_cm = 550, Projection_cm = 150, Price = 6885m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 51, Width_cm = 600, Projection_cm = 150, Price = 7315m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 52, Width_cm = 650, Projection_cm = 150, Price = 8002m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 53, Width_cm = 700, Projection_cm = 150, Price = 8899m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 8 },

                new Projections { ProjectionId = 54, Width_cm = 300, Projection_cm = 200, Price = 5369m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 55, Width_cm = 350, Projection_cm = 200, Price = 5638m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 56, Width_cm = 400, Projection_cm = 200, Price = 5904m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 57, Width_cm = 450, Projection_cm = 200, Price = 6116m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 58, Width_cm = 500, Projection_cm = 200, Price = 6780m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 59, Width_cm = 550, Projection_cm = 200, Price = 7179m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 60, Width_cm = 600, Projection_cm = 200, Price = 7591m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 61, Width_cm = 650, Projection_cm = 200, Price = 8336m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 62, Width_cm = 700, Projection_cm = 200, Price = 9235m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 8 },

                new Projections { ProjectionId = 63, Width_cm = 350, Projection_cm = 250, Price = 5884m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 64, Width_cm = 400, Projection_cm = 250, Price = 6150m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 65, Width_cm = 450, Projection_cm = 250, Price = 6390m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 66, Width_cm = 500, Projection_cm = 250, Price = 7116m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 67, Width_cm = 550, Projection_cm = 250, Price = 7553m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 68, Width_cm = 600, Projection_cm = 250, Price = 7909m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 69, Width_cm = 650, Projection_cm = 250, Price = 8661m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 70, Width_cm = 700, Projection_cm = 250, Price = 9599m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 8 },

                new Projections { ProjectionId = 71, Width_cm = 400, Projection_cm = 300, Price = 6386m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 72, Width_cm = 450, Projection_cm = 300, Price = 6635m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 73, Width_cm = 500, Projection_cm = 300, Price = 7396m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 74, Width_cm = 550, Projection_cm = 300, Price = 7834m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 75, Width_cm = 600, Projection_cm = 300, Price = 8168m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 76, Width_cm = 650, Projection_cm = 300, Price = 8979m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 77, Width_cm = 700, Projection_cm = 300, Price = 9928m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 8 },

                new Projections { ProjectionId = 78, Width_cm = 450, Projection_cm = 350, Price = 7322m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 79, Width_cm = 500, Projection_cm = 350, Price = 7883m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 80, Width_cm = 550, Projection_cm = 350, Price = 8347m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 81, Width_cm = 600, Projection_cm = 350, Price = 8787m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 82, Width_cm = 700, Projection_cm = 350, Price = 10215m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 8 },

                new Projections { ProjectionId = 83, Width_cm = 500, Projection_cm = 400, Price = 8586m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 2 },
                new Projections { ProjectionId = 84, Width_cm = 550, Projection_cm = 400, Price = 9027m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                new Projections { ProjectionId = 85, Width_cm = 600, Projection_cm = 400, Price = 9431m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, ArmTypeId = 5 },
                 new { ProjectionId = 86, Width_cm = 500, Projection_cm = 150, Price = 9884m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 87, Width_cm = 600, Projection_cm = 150, Price = 10284m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 88, Width_cm = 700, Projection_cm = 150, Price = 10784m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 89, Width_cm = 800, Projection_cm = 150, Price = 11290m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 90, Width_cm = 900, Projection_cm = 150, Price = 11701m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 91, Width_cm = 1000, Projection_cm = 150, Price = 12990m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 92, Width_cm = 1100, Projection_cm = 150, Price = 13790m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 93, Width_cm = 1200, Projection_cm = 150, Price = 14650m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 94, Width_cm = 1300, Projection_cm = 150, Price = 16025m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 95, Width_cm = 1400, Projection_cm = 150, Price = 17821m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 18 },

                // 200 projection
                new { ProjectionId = 96, Width_cm = 600, Projection_cm = 200, Price = 10760m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 97, Width_cm = 700, Projection_cm = 200, Price = 11298m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 98, Width_cm = 800, Projection_cm = 200, Price = 11829m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 99, Width_cm = 900, Projection_cm = 200, Price = 12253m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 100, Width_cm = 1000, Projection_cm = 200, Price = 13579m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 101, Width_cm = 1100, Projection_cm = 200, Price = 14379m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 102, Width_cm = 1200, Projection_cm = 200, Price = 15205m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 103, Width_cm = 1300, Projection_cm = 200, Price = 16694m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 104, Width_cm = 1400, Projection_cm = 200, Price = 18492m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 18 },

                // 250 projection
                new { ProjectionId = 105, Width_cm = 700, Projection_cm = 250, Price = 11790m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 106, Width_cm = 800, Projection_cm = 250, Price = 12321m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 107, Width_cm = 900, Projection_cm = 250, Price = 12800m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 108, Width_cm = 1000, Projection_cm = 250, Price = 14252m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 109, Width_cm = 1100, Projection_cm = 250, Price = 15128m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 110, Width_cm = 1200, Projection_cm = 250, Price = 15840m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 111, Width_cm = 1300, Projection_cm = 250, Price = 17341m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 112, Width_cm = 1400, Projection_cm = 250, Price = 19220m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 18 },

                // 300 projection
                new { ProjectionId = 113, Width_cm = 800, Projection_cm = 300, Price = 12793m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 114, Width_cm = 900, Projection_cm = 300, Price = 13292m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 115, Width_cm = 1000, Projection_cm = 300, Price = 14813m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 116, Width_cm = 1100, Projection_cm = 300, Price = 15691m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 117, Width_cm = 1200, Projection_cm = 300, Price = 16357m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 118, Width_cm = 1300, Projection_cm = 300, Price = 17978m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 119, Width_cm = 1400, Projection_cm = 300, Price = 19878m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 18 },

                // 350 projection
                new { ProjectionId = 120, Width_cm = 900, Projection_cm = 350, Price = 14664m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 121, Width_cm = 1000, Projection_cm = 350, Price = 15786m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 122, Width_cm = 1100, Projection_cm = 350, Price = 16715m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 123, Width_cm = 1200, Projection_cm = 350, Price = 17594m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 124, Width_cm = 1400, Projection_cm = 350, Price = 20940m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 18 },

                // 400 projection
                new { ProjectionId = 125, Width_cm = 1000, Projection_cm = 400, Price = 17248m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 10 },
                new { ProjectionId = 126, Width_cm = 1100, Projection_cm = 400, Price = 18127m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                new { ProjectionId = 127, Width_cm = 1200, Projection_cm = 400, Price = 18884m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3, ArmTypeId = 15 },
                //Markilux1-MX-2
                // 150 projection
                new { ProjectionId = 128, Width_cm = 250, Projection_cm = 150, Price = 3949m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 129, Width_cm = 300, Projection_cm = 150, Price = 4198m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 130, Width_cm = 350, Projection_cm = 150, Price = 4537m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 131, Width_cm = 400, Projection_cm = 150, Price = 4819m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 132, Width_cm = 450, Projection_cm = 150, Price = 5078m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 133, Width_cm = 500, Projection_cm = 150, Price = 5367m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 134, Width_cm = 550, Projection_cm = 150, Price = 5689m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 135, Width_cm = 600, Projection_cm = 150, Price = 6007m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },

                // 200 projection
                new { ProjectionId = 136, Width_cm = 250, Projection_cm = 200, Price = 4070m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 137, Width_cm = 300, Projection_cm = 200, Price = 4399m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 138, Width_cm = 350, Projection_cm = 200, Price = 4694m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 139, Width_cm = 400, Projection_cm = 200, Price = 5000m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 140, Width_cm = 450, Projection_cm = 200, Price = 5259m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 141, Width_cm = 500, Projection_cm = 200, Price = 5557m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 142, Width_cm = 550, Projection_cm = 200, Price = 5884m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 143, Width_cm = 600, Projection_cm = 200, Price = 6214m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },

                // 250 projection
                new { ProjectionId = 144, Width_cm = 300, Projection_cm = 250, Price = 4492m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 145, Width_cm = 350, Projection_cm = 250, Price = 4942m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 146, Width_cm = 400, Projection_cm = 250, Price = 5223m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 147, Width_cm = 450, Projection_cm = 250, Price = 5483m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 148, Width_cm = 500, Projection_cm = 250, Price = 5769m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 149, Width_cm = 550, Projection_cm = 250, Price = 6109m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 150, Width_cm = 600, Projection_cm = 250, Price = 6445m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },

                // 300 projection
                new { ProjectionId = 151, Width_cm = 350, Projection_cm = 300, Price = 5071m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 152, Width_cm = 400, Projection_cm = 300, Price = 5400m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 153, Width_cm = 450, Projection_cm = 300, Price = 5659m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 154, Width_cm = 500, Projection_cm = 300, Price = 5955m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 155, Width_cm = 550, Projection_cm = 300, Price = 6303m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 156, Width_cm = 600, Projection_cm = 300, Price = 6654m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },

                // 350 projection
                new { ProjectionId = 157, Width_cm = 400, Projection_cm = 350, Price = 5606m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 158, Width_cm = 450, Projection_cm = 350, Price = 5873m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },
                new { ProjectionId = 159, Width_cm = 500, Projection_cm = 350, Price = 6187m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4, ArmTypeId = 2 },

                 new { ProjectionId = 160, Width_cm = 250, Projection_cm = 150, Price = 2826m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
            new { ProjectionId = 161, Width_cm = 350, Projection_cm = 150, Price = 2979m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
            new { ProjectionId = 162, Width_cm = 400, Projection_cm = 150, Price = 3174m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
            new { ProjectionId = 163, Width_cm = 450, Projection_cm = 150, Price = 3372m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
            new { ProjectionId = 164, Width_cm = 500, Projection_cm = 150, Price = 3579m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 165, Width_cm = 550, Projection_cm = 150, Price = 4039m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 166, Width_cm = 600, Projection_cm = 150, Price = 4516m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 167, Width_cm = 650, Projection_cm = 150, Price = 5050m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 168, Width_cm = 700, Projection_cm = 150, Price = 5838m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 8 },

            // Block 2
            new { ProjectionId = 169, Width_cm = 300, Projection_cm = 150, Price = 3165m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
            new { ProjectionId = 170, Width_cm = 350, Projection_cm = 150, Price = 3375m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
            new { ProjectionId = 171, Width_cm = 400, Projection_cm = 150, Price = 3577m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
            new { ProjectionId = 172, Width_cm = 450, Projection_cm = 150, Price = 3793m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
            new { ProjectionId = 173, Width_cm = 500, Projection_cm = 150, Price = 4012m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 174, Width_cm = 550, Projection_cm = 150, Price = 4260m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 175, Width_cm = 600, Projection_cm = 150, Price = 4733m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 176, Width_cm = 650, Projection_cm = 150, Price = 5304m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 177, Width_cm = 700, Projection_cm = 150, Price = 6110m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 8 },

            // Block 3
            new { ProjectionId = 178, Width_cm = 350, Projection_cm = 150, Price = 3564m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
            new { ProjectionId = 179, Width_cm = 400, Projection_cm = 150, Price = 3769m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
            new { ProjectionId = 180, Width_cm = 450, Projection_cm = 150, Price = 4005m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
            new { ProjectionId = 181, Width_cm = 500, Projection_cm = 150, Price = 4263m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 182, Width_cm = 550, Projection_cm = 150, Price = 4543m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 183, Width_cm = 600, Projection_cm = 150, Price = 4980m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 184, Width_cm = 650, Projection_cm = 150, Price = 5624m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 185, Width_cm = 700, Projection_cm = 150, Price = 6404m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 8 },

            // Block 4
            new { ProjectionId = 186, Width_cm = 400, Projection_cm = 150, Price = 3952m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
            new { ProjectionId = 187, Width_cm = 450, Projection_cm = 150, Price = 4196m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
            new { ProjectionId = 188, Width_cm = 500, Projection_cm = 150, Price = 4474m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 189, Width_cm = 550, Projection_cm = 150, Price = 4922m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 190, Width_cm = 600, Projection_cm = 150, Price = 5237m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 191, Width_cm = 650, Projection_cm = 150, Price = 5870m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 192, Width_cm = 700, Projection_cm = 150, Price = 6673m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 8 },

            // Block 5
            new { ProjectionId = 193, Width_cm = 450, Projection_cm = 150, Price = 4581m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 194, Width_cm = 500, Projection_cm = 150, Price = 4895m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 195, Width_cm = 550, Projection_cm = 150, Price = 5316m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 196, Width_cm = 600, Projection_cm = 150, Price = 5656m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 197, Width_cm = 650, Projection_cm = 150, Price = 6439m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 8 },
            new { ProjectionId = 198, Width_cm = 700, Projection_cm = 150, Price = 6822m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 8 },

            // Block 6
            new { ProjectionId = 199, Width_cm = 500, Projection_cm = 150, Price = 5541m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 200, Width_cm = 550, Projection_cm = 150, Price = 5978m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 201, Width_cm = 600, Projection_cm = 150, Price = 6231m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
            new { ProjectionId = 202, Width_cm = 700, Projection_cm = 150, Price = 7146m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 8 }
            );


            //BF Data
            modelBuilder.Entity<BF>().HasData(
                new BF { BFId = 1, Description = "BF 6", Price = 312.00m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new BF { BFId = 2, Description = "BF 8", Price = 312.00m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new BF { BFId = 3, Description = "BF 16", Price = 312.00m, DateCreated = staticCreatedDate, CreatedBy = "System" }
            );
            //Bracket Data
            // Brackets (PKs start at 1; PartNumber stored as string)
            modelBuilder.Entity<Brackets>().HasData(
                new Brackets { BracketId = 1, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183m, ProductId = 1, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new Brackets { BracketId = 2, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183m, ProductId = 1, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 5 },
                new Brackets { BracketId = 3, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 269m, ProductId = 1, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 8 },

                new Brackets { BracketId = 4, BracketName = "Surcharge for face fixture A", PartNumber = null, Price = 22m, ProductId = 1, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new Brackets { BracketId = 5, BracketName = "Surcharge for face fixture A", PartNumber = null, Price = 22m, ProductId = 1, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 5 },
                new Brackets { BracketId = 6, BracketName = "Surcharge for face fixture A", PartNumber = null, Price = 32m, ProductId = 1, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 8 },

                new Brackets { BracketId = 7, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 330m, ProductId = 1, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new Brackets { BracketId = 8, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 339m, ProductId = 1, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 5 },
                new Brackets { BracketId = 9, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 504m, ProductId = 1, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 8 },

                new Brackets { BracketId = 10, BracketName = "Face fixture 280 x 180 mm", PartNumber = "72611", Price = 78.40m, ProductId = 1, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },
                new Brackets { BracketId = 11, BracketName = "Face fixture bracket A 300 x 180 mm", PartNumber = "72714", Price = 89.00m, ProductId = 1, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },
                new Brackets { BracketId = 12, BracketName = "Spreader plate B 300 x 400 x 12 mm", PartNumber = "75327", Price = 164.90m, ProductId = 1, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },
                new Brackets { BracketId = 13, BracketName = "Stand-off bkt. 80-300 mm for face fixture", PartNumber = "77970", Price = 243.90m, ProductId = 1, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },

                new Brackets { BracketId = 14, BracketName = "Surcharge face fixture bracket A 300 mm", PartNumber = null, Price = 44m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new Brackets { BracketId = 15, BracketName = "Surcharge face fixture bracket A 300 mm", PartNumber = null, Price = 44m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 5 },
                new Brackets { BracketId = 16, BracketName = "Surcharge face fixture bracket A 300 mm", PartNumber = null, Price = 66m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 8 },

                new Brackets { BracketId = 17, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 330m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new Brackets { BracketId = 18, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 339m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 5 },
                new Brackets { BracketId = 19, BracketName = "Surcharge for face fixture incl. spreader plate B", PartNumber = null, Price = 504m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 8 },

                new Brackets { BracketId = 20, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new Brackets { BracketId = 21, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 183m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 5 },
                new Brackets { BracketId = 22, BracketName = "Surcharge for bespoke arms", PartNumber = null, Price = 269m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 8 },

                // Part-numbered items (PartNumber as string)
                new Brackets { BracketId = 23, BracketName = "Face fixture bracket 200 mm", PartNumber = "62143", Price = 66.40m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },
                new Brackets { BracketId = 24, BracketName = "Face fixture bracket A 300 mm", PartNumber = "60775", Price = 88.40m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },
                new Brackets { BracketId = 25, BracketName = "Spreader plate B 250 x 23 x 49 mm", PartNumber = "75327", Price = 164.90m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },
                new Brackets { BracketId = 26, BracketName = "Stand-off bkt. 80-300 mm for face fixture bkt. / 4", PartNumber = "77970", Price = 243.90m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },
                new Brackets { BracketId = 27, BracketName = "Spacer block face fixture 180x150x12 mm / 4", PartNumber = "74989", Price = 8.40m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },
                new Brackets { BracketId = 28, BracketName = "Spacer block face fixture 180x150x20 mm / 4", PartNumber = "749881", Price = 11.20m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },
                new Brackets { BracketId = 29, BracketName = "Cover plate 320x210x2 mm", PartNumber = "71842", Price = 21.90m, ProductId = 2, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },
                new { BracketId = 30, ProductId = 3, BracketName = "Surcharge face fixture bracket A 300 mm", Price = 88m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 10 },
                new { BracketId = 31, ProductId = 3, BracketName = "Surcharge face fixture bracket A 300 mm", Price = 88m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 15 },
                new { BracketId = 32, ProductId = 3, BracketName = "Surcharge face fixture bracket A 300 mm", Price = 132m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 18 },

                new { BracketId = 33, ProductId = 3, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 659m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 10 },
                new { BracketId = 34, ProductId = 3, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 677m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 15 },
                new { BracketId = 35, ProductId = 3, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 1007m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 18 },

                new { BracketId = 36, ProductId = 3, BracketName = "Surcharge for bespoke arms", Price = 361m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 10 },
                new { BracketId = 37, ProductId = 3, BracketName = "Surcharge for bespoke arms", Price = 361m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 15 },
                new { BracketId = 38, ProductId = 3, BracketName = "Surcharge for bespoke arms", Price = 536m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 18 },

                new { BracketId = 39, ProductId = 3, BracketName = "Surcharge for junction roller", Price = 291m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },
                new { BracketId = 40, ProductId = 3, BracketName = "Surcharge for one-piece cover", Price = 291m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },

                // Markilux MX‑2
                new { BracketId = 41, ProductId = 4, BracketName = "Surcharge for face fixture", Price = 220m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 42, ProductId = 4, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 592m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 43, ProductId = 4, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 550m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 44, ProductId = 4, BracketName = "Surcharge for face fixture incl. spreader plate C", Price = 592m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 45, ProductId = 4, BracketName = "Surcharge for top fixture", Price = 287m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 46, ProductId = 4, BracketName = "Surcharge for eaves fixture", Price = 371m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 47, ProductId = 4, BracketName = "Surcharge for bespoke arms", Price = 183m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 48, ProductId = 4, BracketName = "Surcharge for the two-tone housing, markilux \"MX- colour\" in the colour combinations 1-10", Price = 300m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },


                new { BracketId = 49, ProductId = 4, BracketName = "Face fixture bracket left / 3", PartNumber = "72826", Price = 109.80m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 50, ProductId = 4, BracketName = "Face fixture bracket right / 3", PartNumber = "72827", Price = 109.80m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 51, ProductId = 4, BracketName = "Stand-off bkt. 80-300 mm for face fixture bkt. / 4", PartNumber = "72872", Price = 253.50m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 52, ProductId = 4, BracketName = "Top fixture bracket left / 4", PartNumber = "60523", Price = 143.20m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 53, ProductId = 4, BracketName = "Top fixture bracket right / 4", PartNumber = "60524", Price = 143.20m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 54, ProductId = 4, BracketName = "Eaves fixture bracket left 150 mm, complete / 4", PartNumber = "60603", Price = 185.30m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 55, ProductId = 4, BracketName = "Eaves fixture bracket right 150 mm, complete / 4", PartNumber = "60604", Price = 185.30m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 56, ProductId = 4, BracketName = "Eaves fixture bracket 270 mm / 4", PartNumber = "71659", Price = 79.30m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 57, ProductId = 4, BracketName = "Angle and plate for eaves fixture (machine finish) / 4", PartNumber = "716620", Price = 128.90m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 58, ProductId = 4, BracketName = "Additional eaves fixture plate 60x260x12 mm / 2", PartNumber = "75383", Price = 43.90m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 59, ProductId = 4, BracketName = "Spreader plate A 430x160x12 mm / 8", PartNumber = "72870", Price = 186m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 60, ProductId = 4, BracketName = "Spreader plate B 300x400x12 mm / 4", PartNumber = "73465", Price = 164.90m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 61, ProductId = 4, BracketName = "Spreader Plate C 310x130x12 mm / 6", PartNumber = "72526", Price = 186m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 62, ProductId = 4, BracketName = "Spacer block face fixture 100x120x20 mm / 3", PartNumber = "718581", Price = 14.70m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 63, ProductId = 4, BracketName = "Spacer block face fixture 100x120x12 mm / 3", PartNumber = "718571", Price = 14.30m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 64, ProductId = 4, BracketName = "Spacer block for top fixture 90x140x20 mm / 4", PartNumber = "716311", Price = 4.40m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 65, ProductId = 4, BracketName = "Spacer block for top fixture 90x140x12 mm / 4", PartNumber = "716411", Price = 5m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 66, ProductId = 4, BracketName = "Cover plate 230x210x2 mm", PartNumber = "71843", Price = 17m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 67, ProductId = 4, BracketName = "Vertical fixture rail incl. fixing material 624291", PartNumber = "62421", Price = 180m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },

                //MARKILUX 6000 Single
                new { BracketId = 68, ProductId = 5, BracketName = "Surcharge for face fixture", Price = 298m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 69, ProductId = 5, BracketName = "Surcharge for face fixture", Price = 447m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 3 },
                new { BracketId = 70, ProductId = 5, BracketName = "Surcharge for face fixture", Price = 596m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 8 },

                new { BracketId = 71, ProductId = 5, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 554m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 72, ProductId = 5, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 711m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 3 },
                new { BracketId = 73, ProductId = 5, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 988m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 8 },

                new { BracketId = 74, ProductId = 5, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 628m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 75, ProductId = 5, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 785m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 3 },
                new { BracketId = 76, ProductId = 5, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 1099m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 8 },

                new { BracketId = 77, ProductId = 5, BracketName = "Surcharge for top fixture", Price = 397m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 78, ProductId = 5, BracketName = "Surcharge for top fixture", Price = 596m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 3 },
                new { BracketId = 79, ProductId = 5, BracketName = "Surcharge for top fixture", Price = 794m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 8 },

                new { BracketId = 80, ProductId = 5, BracketName = "Surcharge for eaves fixture", Price = 475m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 81, ProductId = 5, BracketName = "Surcharge for eaves fixture", Price = 712m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 3 },
                new { BracketId = 82, ProductId = 5, BracketName = "Surcharge for eaves fixture", Price = 950m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 8 },

                new { BracketId = 83, ProductId = 5, BracketName = "Surcharge for bespoke arms", Price = 183m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 2 },
                new { BracketId = 84, ProductId = 5, BracketName = "Surcharge for bespoke arms", Price = 183m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 3 },
                new { BracketId = 85, ProductId = 5, BracketName = "Surcharge for bespoke arms", Price = 269m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 8 },

                // Part-numbered brackets
                new { BracketId = 86, ProductId = 5, BracketName = "Face fixture bracket assembly 5 - 35° / 4", PartNumber = "74909", Price = 148.90m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                new { BracketId = 87, ProductId = 5, BracketName = "Face fixture bracket assembly 36 - 70° / 4", PartNumber = "74928", Price = 148.90m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                new { BracketId = 88, ProductId = 5, BracketName = "Stand-off bkt. 80-300 mm for face fixture bkt. / 4", PartNumber = "77970", Price = 243.90m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                new { BracketId = 89, ProductId = 5, BracketName = "Top fixture bracket 5 - 35° / 4", PartNumber = "74903", Price = 198.40m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                new { BracketId = 90, ProductId = 5, BracketName = "Top fixture bracket 36 - 70° / 4", PartNumber = "74905", Price = 198.40m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                new { BracketId = 91, ProductId = 5, BracketName = "Eaves fixture bracket 150mm, complete / 4", PartNumber = "74944", Price = 237.30m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                new { BracketId = 92, ProductId = 5, BracketName = "Eaves fixture bracket 270 mm, complete / 4", PartNumber = "74970", Price = 276.40m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                new { BracketId = 93, ProductId = 5, BracketName = "Angle and plate for eaves fixture (machine finish) / 4", PartNumber = "741290", Price = 142.80m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                new { BracketId = 94, ProductId = 5, BracketName = "Additional eaves fixture plate 60x260x12 mm / 2", PartNumber = "75383", Price = 43.90m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                new { BracketId = 95, ProductId = 5, BracketName = "Spreader plate A 430x160x12 mm / 8", PartNumber = "75328", Price = 127.70m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                new { BracketId = 96, ProductId = 5, BracketName = "Spreader plate B 300x400x12 mm / 4", PartNumber = "75327", Price = 164.90m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                new { BracketId = 97, ProductId = 5, BracketName = "Spacer block face fixture 180x150x20 mm / 4", PartNumber = "749881", Price = 11.20m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                new { BracketId = 98, ProductId = 5, BracketName = "Spacer block for top fixture 136x150x20 mm /4", PartNumber = "716331", Price = 5.70m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                new { BracketId = 99, ProductId = 5, BracketName = "Spacer block face fixture 180x150x12 mm / 4", PartNumber = "74989", Price = 8.40m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                new { BracketId = 100, ProductId = 5, BracketName = "Cover plate 320x210x2 mm", PartNumber = "71842", Price = 21.90m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null }
            );

            //Arms Data — values moved to Brackets above; Arms table left empty
            modelBuilder.Entity<Arms>().HasData(
            );
            //Motors Data
            modelBuilder.Entity<Motors>().HasData(
                new { MotorId = 1, Description = "Surcharge for servo-assisted gear", Price = 75m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { MotorId = 2, Description = "Surcharge for servo-assisted gear", Price = 75m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { MotorId = 3, Description = "Surcharge for servo-assisted gear", Price = 0m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },

                new { MotorId = 4, Description = "Surcharge for hard-wired motor", Price = 484m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { MotorId = 5, Description = "Surcharge for hard-wired motor", Price = 484m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { MotorId = 6, Description = "Surcharge for hard-wired motor", Price = 574m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },

                new { MotorId = 7, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { MotorId = 8, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { MotorId = 9, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 809m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },

                new { MotorId = 10, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { MotorId = 11, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { MotorId = 12, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 691m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },

                new { MotorId = 13, Description = "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", Price = 1115m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { MotorId = 14, Description = "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", Price = 1115m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { MotorId = 15, Description = "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", Price = 0m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },

                new { MotorId = 16, Description = "Surcharge for radio-contr. motor io with manual override w/o transmitter", Price = 997m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { MotorId = 17, Description = "Surcharge for radio-contr. motor io with manual override w/o transmitter", Price = 997m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { MotorId = 18, Description = "Surcharge for radio-contr. motor io with manual override w/o transmitter", Price = 0m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 }
             );
            //ValanceStyle Data
            modelBuilder.Entity<ValanceStyle>().HasData(
                 new { ValanceStyleId = 1, WidthCm = 250, Price = 79m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { ValanceStyleId = 2, WidthCm = 300, Price = 87m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { ValanceStyleId = 3, WidthCm = 350, Price = 96m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { ValanceStyleId = 4, WidthCm = 400, Price = 109m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { ValanceStyleId = 5, WidthCm = 450, Price = 122m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { ValanceStyleId = 6, WidthCm = 500, Price = 134m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { ValanceStyleId = 7, WidthCm = 550, Price = 145m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { ValanceStyleId = 8, WidthCm = 600, Price = 158m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { ValanceStyleId = 9, WidthCm = 650, Price = 166m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { ValanceStyleId = 10, WidthCm = 700, Price = 177m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 }
             );

            //NonStandardRALColours Data
            modelBuilder.Entity<NonStandardRALColours>().HasData(
             new NonStandardRALColours { RALColourId = 1, WidthCm = 250, Price = 316m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 2, WidthCm = 300, Price = 329m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 3, WidthCm = 350, Price = 352m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 4, WidthCm = 400, Price = 371m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 5, WidthCm = 450, Price = 396m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 6, WidthCm = 500, Price = 445m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 7, WidthCm = 550, Price = 477m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 8, WidthCm = 600, Price = 509m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 9, WidthCm = 650, Price = 539m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 10, WidthCm = 700, Price = 640m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1, MultiplyBy = 2.5m },
                 new NonStandardRALColours { RALColourId = 11, WidthCm = 250, Price = 315m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 12, WidthCm = 300, Price = 328m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 13, WidthCm = 350, Price = 351m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 14, WidthCm = 400, Price = 371m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 15, WidthCm = 450, Price = 396m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 16, WidthCm = 500, Price = 444m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 17, WidthCm = 550, Price = 476m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 18, WidthCm = 600, Price = 508m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 19, WidthCm = 650, Price = 539m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, MultiplyBy = 2.5m },
                new NonStandardRALColours { RALColourId = 20, WidthCm = 700, Price = 640m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 2, MultiplyBy = 2.5m },
                new { RALColourId = 21, WidthCm = 500, Price = 625m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3 },
                new { RALColourId = 22, WidthCm = 600, Price = 650m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3 },
                new { RALColourId = 23, WidthCm = 700, Price = 692m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3 },
                new { RALColourId = 24, WidthCm = 800, Price = 730m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3 },
                new { RALColourId = 25, WidthCm = 900, Price = 783m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3 },
                new { RALColourId = 26, WidthCm = 1000, Price = 879m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3 },
                new { RALColourId = 27, WidthCm = 1100, Price = 939m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3 },
                new { RALColourId = 28, WidthCm = 1200, Price = 1008m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3 },
                new { RALColourId = 29, WidthCm = 1300, Price = 1066m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3 },
                new { RALColourId = 30, WidthCm = 1400, Price = 1274m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 3 },
               //Markilu-MX-2
                new { RALColourId = 31, WidthCm = 250, Price = 329m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4 },
                new { RALColourId = 32, WidthCm = 300, Price = 352m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4 },
                new { RALColourId = 33, WidthCm = 350, Price = 371m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4 },
                new { RALColourId = 34, WidthCm = 400, Price = 388m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4 },
                new { RALColourId = 35, WidthCm = 450, Price = 406m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4 },
                new { RALColourId = 36, WidthCm = 500, Price = 423m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4 },
                new { RALColourId = 37, WidthCm = 550, Price = 439m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4 },
                new { RALColourId = 38, WidthCm = 600, Price = 458m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 4 },
                //MARKILUX 6000 Single
                new { RALColourId = 39, WidthCm = 250, Price = 316m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { RALColourId = 40, WidthCm = 300, Price = 329m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { RALColourId = 41, WidthCm = 350, Price = 352m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { RALColourId = 42, WidthCm = 400, Price = 371m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { RALColourId = 43, WidthCm = 450, Price = 396m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { RALColourId = 44, WidthCm = 500, Price = 445m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { RALColourId = 45, WidthCm = 550, Price = 477m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { RALColourId = 46, WidthCm = 600, Price = 509m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { RALColourId = 47, WidthCm = 650, Price = 539m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { RALColourId = 48, WidthCm = 700, Price = 640m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 }
            );
            //WallSealingProfile Data
            modelBuilder.Entity<WallSealingProfile>().HasData(
                 new { WallSealingProfileId = 1, WidthCm = 250, Price = 90m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { WallSealingProfileId = 2, WidthCm = 300, Price = 104m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { WallSealingProfileId = 3, WidthCm = 350, Price = 114m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { WallSealingProfileId = 4, WidthCm = 400, Price = 130m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { WallSealingProfileId = 5, WidthCm = 450, Price = 146m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { WallSealingProfileId = 6, WidthCm = 500, Price = 162m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { WallSealingProfileId = 7, WidthCm = 550, Price = 177m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { WallSealingProfileId = 8, WidthCm = 600, Price = 193m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { WallSealingProfileId = 9, WidthCm = 650, Price = 207m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                 new { WallSealingProfileId = 10, WidthCm = 700, Price = 223m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 }
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
                new ArmsType { ArmTypeId = 1, Description = "All", DateCreated = new DateTime(2026, 4, 15, 11, 02, 03, 796), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 2, Description = "2-0-2", DateCreated = new DateTime(2026, 4, 15, 11, 02, 03, 796), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 3, Description = "2-0-3", DateCreated = new DateTime(2026, 4, 15, 11, 37, 04, 580), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 4, Description = "2-0-4", DateCreated = new DateTime(2026, 4, 15, 13, 08, 51, 993), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 5, Description = "2-1-3", DateCreated = new DateTime(2026, 4, 15, 11, 02, 03, 796), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 6, Description = "2-1-5", DateCreated = new DateTime(2026, 4, 15, 13, 08, 51, 993), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 7, Description = "3-1-5", DateCreated = new DateTime(2026, 4, 15, 13, 10, 49, 347), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 8, Description = "3-2-4", DateCreated = new DateTime(2026, 4, 15, 11, 02, 03, 796), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 9, Description = "3-2-6", DateCreated = new DateTime(2026, 4, 15, 13, 08, 51, 993), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 10, Description = "4-0-4", DateCreated = new DateTime(2026, 4, 15, 11, 21, 28, 550), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 11, Description = "4-0-5", DateCreated = new DateTime(2026, 4, 15, 13, 09, 47, 603), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 12, Description = "4-0-6", DateCreated = new DateTime(2026, 4, 15, 11, 52, 53, 557), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 13, Description = "4-0-7", DateCreated = new DateTime(2026, 4, 15, 13, 09, 47, 603), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 14, Description = "4-0-8", DateCreated = new DateTime(2026, 4, 15, 13, 11, 21, 477), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 15, Description = "4-2-6", DateCreated = new DateTime(2026, 4, 15, 11, 21, 28, 550), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 16, Description = "4-2-9", DateCreated = new DateTime(2026, 4, 15, 13, 09, 47, 603), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 17, Description = "6-2-10", DateCreated = new DateTime(2026, 4, 15, 13, 11, 37, 573), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 18, Description = "6-4-8", DateCreated = new DateTime(2026, 4, 15, 11, 21, 28, 550), CreatedBy = "System" },
                new ArmsType { ArmTypeId = 19, Description = "6-4-11", DateCreated = new DateTime(2026, 4, 15, 13, 09, 47, 603), CreatedBy = "System" }
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
                new ShadePlus { ShadePlusId = 1, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 250, Price = 1592m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 2, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 300, Price = 1650m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 3, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 350, Price = 1727m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 4, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 400, Price = 1815m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 5, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 450, Price = 1875m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 6, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 500, Price = 1951m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 7, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 550, Price = 2044m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 8, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 600, Price = 2128m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 9, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 650, Price = 2206m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },
                new ShadePlus { ShadePlusId = 10, Description = "Surcharge for height 170 cm - radio-controlled motor", WidthCm = 700, Price = 2284m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 1 },

                //Mark - 6000 single
                new { ShadePlusId = 11, WidthCm = 250, Description = "Surcharge for height 210 cm with gearbox", Price = 719m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 12, WidthCm = 300, Description = "Surcharge for height 210 cm with gearbox", Price = 798m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 13, WidthCm = 350, Description = "Surcharge for height 210 cm with gearbox", Price = 865m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 14, WidthCm = 400, Description = "Surcharge for height 210 cm with gearbox", Price = 945m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 15, WidthCm = 450, Description = "Surcharge for height 210 cm with gearbox", Price = 1019m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 16, WidthCm = 500, Description = "Surcharge for height 210 cm with gearbox", Price = 1096m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 17, WidthCm = 550, Description = "Surcharge for height 210 cm with gearbox", Price = 1173m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 18, WidthCm = 600, Description = "Surcharge for height 210 cm with gearbox", Price = 1248m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 19, WidthCm = 650, Description = "Surcharge for height 210 cm with gearbox", Price = 1319m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 20, WidthCm = 700, Description = "Surcharge for height 210 cm with gearbox", Price = 1396m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },

                new { ShadePlusId = 21, WidthCm = 250, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 1538m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 22, WidthCm = 300, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 1596m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 23, WidthCm = 350, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 1693m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 24, WidthCm = 400, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 1778m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 25, WidthCm = 450, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 1830m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 26, WidthCm = 500, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 1898m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 27, WidthCm = 550, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 1986m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 28, WidthCm = 600, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 2070m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 29, WidthCm = 650, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 2163m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 30, WidthCm = 700, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 2262m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },

                new { ShadePlusId = 31, WidthCm = 250, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 1669m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 32, WidthCm = 300, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 1729m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 33, WidthCm = 350, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 1828m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 34, WidthCm = 400, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 1911m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 35, WidthCm = 450, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 1961m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 36, WidthCm = 500, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 2032m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 37, WidthCm = 550, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 2119m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 38, WidthCm = 600, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 2204m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 39, WidthCm = 650, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 2297m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ShadePlusId = 40, WidthCm = 700, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 2395m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 }

            );

            //LightingCassette Data
            modelBuilder.Entity<LightingCassette>().HasData(
                //new LightingCassette { LightingId = 1, Description = "Surcharge for LED Line RGB-WW Radio-controlled io - dimmable (without remote control)", Price = 1555.00m, DateCreated = new DateTime(2026, 4, 6), CreatedBy = "System", ProductId = 5 },
                //new LightingCassette { LightingId = 2, Description = "Surcharge for LED Line RGB-WW Zigbee radio control - dimmable (without transmitter)", Price = 1386.00m, DateCreated = new DateTime(2026, 4, 6), CreatedBy = "System", ProductId = 5 },
                new LightingCassette { LightingId = 1, Description = "Surcharge for LED Line RGB-WW Radio-controlled io - dimmable (without remote control)", Price = 1555.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 4 },
                new LightingCassette { LightingId = 2, Description = "Surcharge for LED Line RGB-WW Zigbee radio control - dimmable (without transmitter)", Price = 1386.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 4 }
            );
        }
    }
}