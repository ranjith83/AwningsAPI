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
            new { ProjectionId = 202, Width_cm = 700, Projection_cm = 150, Price = 7146m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 8 },
            // Markilux 6000 Coupler
            new { ProjectionId = 203, Width_cm = 500, Projection_cm = 150, Price = 5644m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
            new { ProjectionId = 204, Width_cm = 600, Projection_cm = 150, Price = 5953m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
            new { ProjectionId = 205, Width_cm = 700, Projection_cm = 150, Price = 6335m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
            new { ProjectionId = 206, Width_cm = 800, Projection_cm = 150, Price = 6726m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
            new { ProjectionId = 207, Width_cm = 900, Projection_cm = 150, Price = 7148m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
            new { ProjectionId = 208, Width_cm = 1000, Projection_cm = 150, Price = 7557m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 209, Width_cm = 1100, Projection_cm = 150, Price = 8069m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 210, Width_cm = 1200, Projection_cm = 150, Price = 9025m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 211, Width_cm = 1300, Projection_cm = 150, Price = 10094m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 212, Width_cm = 1390, Projection_cm = 150, Price = 11662m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 18 },

            // 200 projection
            new { ProjectionId = 213, Width_cm = 600, Projection_cm = 200, Price = 6317m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
            new { ProjectionId = 214, Width_cm = 700, Projection_cm = 200, Price = 6739m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
            new { ProjectionId = 215, Width_cm = 800, Projection_cm = 200, Price = 7146m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
            new { ProjectionId = 216, Width_cm = 900, Projection_cm = 200, Price = 7576m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
            new { ProjectionId = 217, Width_cm = 1000, Projection_cm = 200, Price = 8010m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 218, Width_cm = 1100, Projection_cm = 200, Price = 8512m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 219, Width_cm = 1200, Projection_cm = 200, Price = 9454m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 220, Width_cm = 1300, Projection_cm = 200, Price = 10606m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 221, Width_cm = 1390, Projection_cm = 200, Price = 12208m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 18 },

            // 250 projection
            new { ProjectionId = 222, Width_cm = 700, Projection_cm = 250, Price = 7112m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
            new { ProjectionId = 223, Width_cm = 800, Projection_cm = 250, Price = 7524m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
            new { ProjectionId = 224, Width_cm = 900, Projection_cm = 250, Price = 8003m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
            new { ProjectionId = 225, Width_cm = 1000, Projection_cm = 250, Price = 8517m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 226, Width_cm = 1100, Projection_cm = 250, Price = 9076m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 227, Width_cm = 1200, Projection_cm = 250, Price = 9947m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 228, Width_cm = 1300, Projection_cm = 250, Price = 11235m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 229, Width_cm = 1390, Projection_cm = 250, Price = 12798m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 18 },

            // 300 projection
            new { ProjectionId = 230, Width_cm = 800, Projection_cm = 300, Price = 7890m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
            new { ProjectionId = 231, Width_cm = 900, Projection_cm = 300, Price = 8382m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
            new { ProjectionId = 232, Width_cm = 1000, Projection_cm = 300, Price = 8936m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 233, Width_cm = 1100, Projection_cm = 300, Price = 9832m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 234, Width_cm = 1200, Projection_cm = 300, Price = 10466m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 235, Width_cm = 1300, Projection_cm = 300, Price = 11729m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 236, Width_cm = 1390, Projection_cm = 300, Price = 13335m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 18 },

            // 350 projection
            new { ProjectionId = 237, Width_cm = 900, Projection_cm = 350, Price = 9143m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 238, Width_cm = 1000, Projection_cm = 350, Price = 9779m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 239, Width_cm = 1100, Projection_cm = 350, Price = 10623m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 240, Width_cm = 1200, Projection_cm = 350, Price = 11303m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 241, Width_cm = 1300, Projection_cm = 350, Price = 12866m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 18 },
            new { ProjectionId = 242, Width_cm = 1390, Projection_cm = 350, Price = 13634m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 18 },

            // 400 projection
            new { ProjectionId = 243, Width_cm = 1000, Projection_cm = 400, Price = 11070m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 244, Width_cm = 1100, Projection_cm = 400, Price = 11948m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            new { ProjectionId = 245, Width_cm = 1200, Projection_cm = 400, Price = 12452m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
            //Markilux MX-3 
            new { ProjectionId = 246, Width_cm = 250, Projection_cm = 150, Price = 2304m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 247, Width_cm = 300, Projection_cm = 150, Price = 2488m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 248, Width_cm = 350, Projection_cm = 150, Price = 2740m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 249, Width_cm = 400, Projection_cm = 150, Price = 2945m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 250, Width_cm = 450, Projection_cm = 150, Price = 3139m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 251, Width_cm = 500, Projection_cm = 150, Price = 3355m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 252, Width_cm = 550, Projection_cm = 150, Price = 3570m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 253, Width_cm = 600, Projection_cm = 150, Price = 3796m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },

            new { ProjectionId = 254, Width_cm = 250, Projection_cm = 200, Price = 2431m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 255, Width_cm = 300, Projection_cm = 200, Price = 2635m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 256, Width_cm = 350, Projection_cm = 200, Price = 2854m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 257, Width_cm = 400, Projection_cm = 200, Price = 3082m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 258, Width_cm = 450, Projection_cm = 200, Price = 3273m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 259, Width_cm = 500, Projection_cm = 200, Price = 3494m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 260, Width_cm = 550, Projection_cm = 200, Price = 3717m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 261, Width_cm = 600, Projection_cm = 200, Price = 3949m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },

            new { ProjectionId = 262, Width_cm = 300, Projection_cm = 250, Price = 2765m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 263, Width_cm = 350, Projection_cm = 250, Price = 2988m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 264, Width_cm = 400, Projection_cm = 250, Price = 3228m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 265, Width_cm = 450, Projection_cm = 250, Price = 3437m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 266, Width_cm = 500, Projection_cm = 250, Price = 3653m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 267, Width_cm = 550, Projection_cm = 250, Price = 3874m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 268, Width_cm = 600, Projection_cm = 250, Price = 4114m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },

            new { ProjectionId = 269, Width_cm = 350, Projection_cm = 300, Price = 3130m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 270, Width_cm = 400, Projection_cm = 300, Price = 3379m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 271, Width_cm = 450, Projection_cm = 300, Price = 3571m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 272, Width_cm = 500, Projection_cm = 300, Price = 3792m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 273, Width_cm = 550, Projection_cm = 300, Price = 4016m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 274, Width_cm = 600, Projection_cm = 300, Price = 4259m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },

            new { ProjectionId = 275, Width_cm = 400, Projection_cm = 350, Price = 3581m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 276, Width_cm = 450, Projection_cm = 350, Price = 3791m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },
            new { ProjectionId = 277, Width_cm = 500, Projection_cm = 350, Price = 4017m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7, ArmTypeId = 2 },

            // 150 cm
            new { ProjectionId = 278, Width_cm = 250, Projection_cm = 150, Price = 1928m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 279, Width_cm = 300, Projection_cm = 150, Price = 2082m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 280, Width_cm = 350, Projection_cm = 150, Price = 2294m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 281, Width_cm = 400, Projection_cm = 150, Price = 2467m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 282, Width_cm = 450, Projection_cm = 150, Price = 2629m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 283, Width_cm = 500, Projection_cm = 150, Price = 2810m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },

            // 200 cm
            new { ProjectionId = 284, Width_cm = 250, Projection_cm = 200, Price = 2037m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 285, Width_cm = 300, Projection_cm = 200, Price = 2208m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 286, Width_cm = 350, Projection_cm = 200, Price = 2387m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 287, Width_cm = 400, Projection_cm = 200, Price = 2581m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 288, Width_cm = 450, Projection_cm = 200, Price = 2742m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 289, Width_cm = 500, Projection_cm = 200, Price = 2925m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },

            // 250 cm
            new { ProjectionId = 290, Width_cm = 300, Projection_cm = 250, Price = 2314m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 291, Width_cm = 350, Projection_cm = 250, Price = 2502m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 292, Width_cm = 400, Projection_cm = 250, Price = 2704m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 293, Width_cm = 450, Projection_cm = 250, Price = 2880m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 294, Width_cm = 500, Projection_cm = 250, Price = 3057m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },

            // 300 cm
            new { ProjectionId = 295, Width_cm = 350, Projection_cm = 300, Price = 2621m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 296, Width_cm = 400, Projection_cm = 300, Price = 2830m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 297, Width_cm = 450, Projection_cm = 300, Price = 2989m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },
            new { ProjectionId = 298, Width_cm = 500, Projection_cm = 300, Price = 3174m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8, ArmTypeId = 2 },

            // 150 cm
            new { ProjectionId = 299, Width_cm = 250, Projection_cm = 150, Price = 2923m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 300, Width_cm = 300, Projection_cm = 150, Price = 3156m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 301, Width_cm = 350, Projection_cm = 150, Price = 3475m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 302, Width_cm = 400, Projection_cm = 150, Price = 3742m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 303, Width_cm = 450, Projection_cm = 150, Price = 3987m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 304, Width_cm = 500, Projection_cm = 150, Price = 4259m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 305, Width_cm = 550, Projection_cm = 150, Price = 4562m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 306, Width_cm = 600, Projection_cm = 150, Price = 4864m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },

            // 200 cm
            new { ProjectionId = 307, Width_cm = 250, Projection_cm = 200, Price = 3037m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 308, Width_cm = 300, Projection_cm = 200, Price = 3347m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 309, Width_cm = 350, Projection_cm = 200, Price = 3625m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 310, Width_cm = 400, Projection_cm = 200, Price = 3914m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 311, Width_cm = 450, Projection_cm = 200, Price = 4158m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 312, Width_cm = 500, Projection_cm = 200, Price = 4438m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 313, Width_cm = 550, Projection_cm = 200, Price = 4748m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 314, Width_cm = 600, Projection_cm = 200, Price = 5059m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },

            // 250 cm
            new { ProjectionId = 315, Width_cm = 300, Projection_cm = 250, Price = 3433m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 316, Width_cm = 350, Projection_cm = 250, Price = 3859m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 317, Width_cm = 400, Projection_cm = 250, Price = 4125m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 318, Width_cm = 450, Projection_cm = 250, Price = 4369m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 319, Width_cm = 500, Projection_cm = 250, Price = 4637m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },

            // 300 cm
            new { ProjectionId = 320, Width_cm = 350, Projection_cm = 300, Price = 3980m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 321, Width_cm = 400, Projection_cm = 300, Price = 4290m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 322, Width_cm = 450, Projection_cm = 300, Price = 4536m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 323, Width_cm = 500, Projection_cm = 300, Price = 4813m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 324, Width_cm = 550, Projection_cm = 300, Price = 5143m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 325, Width_cm = 600, Projection_cm = 300, Price = 5472m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },

            // 350 cm
            new { ProjectionId = 326, Width_cm = 400, Projection_cm = 350, Price = 4485m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 327, Width_cm = 450, Projection_cm = 350, Price = 4737m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 328, Width_cm = 500, Projection_cm = 350, Price = 5032m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },

            // 150 cm
            new { ProjectionId = 329, Width_cm = 250, Projection_cm = 150, Price = 2683m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 330, Width_cm = 300, Projection_cm = 150, Price = 2828m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 331, Width_cm = 350, Projection_cm = 150, Price = 3014m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 332, Width_cm = 400, Projection_cm = 150, Price = 3164m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 333, Width_cm = 450, Projection_cm = 150, Price = 3361m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 334, Width_cm = 500, Projection_cm = 150, Price = 3556m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 335, Width_cm = 550, Projection_cm = 150, Price = 3795m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 336, Width_cm = 600, Projection_cm = 150, Price = 4138m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 337, Width_cm = 650, Projection_cm = 150, Price = 4474m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 338, Width_cm = 700, Projection_cm = 150, Price = 5530m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 8 },

            // 200 cm
            new { ProjectionId = 339, Width_cm = 250, Projection_cm = 200, Price = 2845m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 340, Width_cm = 300, Projection_cm = 200, Price = 3003m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 341, Width_cm = 350, Projection_cm = 200, Price = 3199m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 342, Width_cm = 400, Projection_cm = 200, Price = 3360m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 343, Width_cm = 450, Projection_cm = 200, Price = 3563m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 344, Width_cm = 500, Projection_cm = 200, Price = 3766m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 345, Width_cm = 550, Projection_cm = 200, Price = 4040m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 346, Width_cm = 600, Projection_cm = 200, Price = 4370m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 347, Width_cm = 650, Projection_cm = 200, Price = 4778m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 348, Width_cm = 700, Projection_cm = 200, Price = 5791m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 8 },

            // 250 cm
            new { ProjectionId = 349, Width_cm = 300, Projection_cm = 250, Price = 3177m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 350, Width_cm = 350, Projection_cm = 250, Price = 3376m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 351, Width_cm = 400, Projection_cm = 250, Price = 3536m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 352, Width_cm = 450, Projection_cm = 250, Price = 3763m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 353, Width_cm = 500, Projection_cm = 250, Price = 4004m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 354, Width_cm = 550, Projection_cm = 250, Price = 4242m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 355, Width_cm = 600, Projection_cm = 250, Price = 4582m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 356, Width_cm = 650, Projection_cm = 250, Price = 4999m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 357, Width_cm = 700, Projection_cm = 250, Price = 6073m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 8 },

            // 300 cm
            new { ProjectionId = 358, Width_cm = 350, Projection_cm = 300, Price = 3492m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 359, Width_cm = 400, Projection_cm = 300, Price = 3713m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 360, Width_cm = 450, Projection_cm = 300, Price = 3937m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 361, Width_cm = 500, Projection_cm = 300, Price = 4228m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 362, Width_cm = 550, Projection_cm = 300, Price = 4568m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 363, Width_cm = 600, Projection_cm = 300, Price = 4891m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 364, Width_cm = 650, Projection_cm = 300, Price = 5295m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 365, Width_cm = 700, Projection_cm = 300, Price = 6320m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 8 },

            // 350 cm
            new { ProjectionId = 366, Width_cm = 400, Projection_cm = 350, Price = 4012m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 2 },
            new { ProjectionId = 367, Width_cm = 450, Projection_cm = 350, Price = 4299m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 368, Width_cm = 500, Projection_cm = 350, Price = 4595m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 369, Width_cm = 550, Projection_cm = 350, Price = 4901m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 370, Width_cm = 600, Projection_cm = 350, Price = 5263m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 371, Width_cm = 650, Projection_cm = 350, Price = 6192m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 8 },
            new { ProjectionId = 372, Width_cm = 700, Projection_cm = 350, Price = 6470m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 8 },

            // 400 cm
            new { ProjectionId = 373, Width_cm = 450, Projection_cm = 400, Price = 4816m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 374, Width_cm = 500, Projection_cm = 400, Price = 5054m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 375, Width_cm = 550, Projection_cm = 400, Price = 5435m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 3 },
            new { ProjectionId = 376, Width_cm = 700, Projection_cm = 400, Price = 6774m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 8 },

                // 150 cm
                new { ProjectionId = 377, Width_cm = 500, Projection_cm = 150, Price = 5353m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 378, Width_cm = 600, Projection_cm = 150, Price = 5643m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 379, Width_cm = 700, Projection_cm = 150, Price = 6013m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 380, Width_cm = 800, Projection_cm = 150, Price = 6313m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 381, Width_cm = 900, Projection_cm = 150, Price = 6713m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 382, Width_cm = 1000, Projection_cm = 150, Price = 7099m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 383, Width_cm = 1100, Projection_cm = 150, Price = 7580m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 384, Width_cm = 1200, Projection_cm = 150, Price = 8271m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 385, Width_cm = 1300, Projection_cm = 150, Price = 8936m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 386, Width_cm = 1390, Projection_cm = 150, Price = 11051m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 18 },

                // 200 cm
                new { ProjectionId = 387, Width_cm = 500, Projection_cm = 200, Price = 5681m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 388, Width_cm = 600, Projection_cm = 200, Price = 5989m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 389, Width_cm = 700, Projection_cm = 200, Price = 6386m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 390, Width_cm = 800, Projection_cm = 200, Price = 6709m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 391, Width_cm = 900, Projection_cm = 200, Price = 7113m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 392, Width_cm = 1000, Projection_cm = 200, Price = 7521m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 393, Width_cm = 1100, Projection_cm = 200, Price = 8072m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 394, Width_cm = 1200, Projection_cm = 200, Price = 8731m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 395, Width_cm = 1300, Projection_cm = 200, Price = 9548m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 396, Width_cm = 1390, Projection_cm = 200, Price = 11575m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 18 },

                // 250 cm
                new { ProjectionId = 397, Width_cm = 600, Projection_cm = 250, Price = 6346m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 398, Width_cm = 700, Projection_cm = 250, Price = 6745m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 399, Width_cm = 800, Projection_cm = 250, Price = 7066m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 400, Width_cm = 900, Projection_cm = 250, Price = 7515m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 401, Width_cm = 1000, Projection_cm = 250, Price = 7994m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 402, Width_cm = 1100, Projection_cm = 250, Price = 8477m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 403, Width_cm = 1200, Projection_cm = 250, Price = 9149m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 404, Width_cm = 1300, Projection_cm = 250, Price = 9989m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 405, Width_cm = 1390, Projection_cm = 250, Price = 12129m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 18 },

                // 300 cm
                new { ProjectionId = 406, Width_cm = 700, Projection_cm = 300, Price = 6971m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 407, Width_cm = 800, Projection_cm = 300, Price = 7415m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 408, Width_cm = 900, Projection_cm = 300, Price = 7868m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 409, Width_cm = 1000, Projection_cm = 300, Price = 8451m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 410, Width_cm = 1100, Projection_cm = 300, Price = 9131m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 411, Width_cm = 1200, Projection_cm = 300, Price = 9774m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 412, Width_cm = 1300, Projection_cm = 300, Price = 10580m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 413, Width_cm = 1390, Projection_cm = 300, Price = 12631m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 18 },

                // 350 cm
                new { ProjectionId = 414, Width_cm = 800, Projection_cm = 350, Price = 8007m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 10 },
                new { ProjectionId = 415, Width_cm = 900, Projection_cm = 350, Price = 8586m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 416, Width_cm = 1000, Projection_cm = 350, Price = 9179m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 417, Width_cm = 1100, Projection_cm = 350, Price = 9792m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 418, Width_cm = 1200, Projection_cm = 350, Price = 10520m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 419, Width_cm = 1300, Projection_cm = 350, Price = 12373m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 18 },
                new { ProjectionId = 420, Width_cm = 1390, Projection_cm = 350, Price = 12930m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 18 },

                // 400 cm
                new { ProjectionId = 421, Width_cm = 900, Projection_cm = 400, Price = 9620m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 422, Width_cm = 1000, Projection_cm = 400, Price = 10101m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },
                new { ProjectionId = 423, Width_cm = 1100, Projection_cm = 400, Price = 10855m, ProductId = 11, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ArmTypeId = 12 },

                // 150 cm
                new { ProjectionId = 424, Width_cm = 250, Projection_cm = 150, Price = 3247m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 425, Width_cm = 300, Projection_cm = 150, Price = 3492m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 426, Width_cm = 350, Projection_cm = 150, Price = 3832m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 427, Width_cm = 400, Projection_cm = 150, Price = 4125m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 428, Width_cm = 450, Projection_cm = 150, Price = 4451m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 429, Width_cm = 500, Projection_cm = 150, Price = 4738m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 430, Width_cm = 550, Projection_cm = 150, Price = 4994m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 431, Width_cm = 600, Projection_cm = 150, Price = 5245m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 432, Width_cm = 650, Projection_cm = 150, Price = 5901m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 433, Width_cm = 700, Projection_cm = 150, Price = 6560m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // 200 cm
                new { ProjectionId = 434, Width_cm = 250, Projection_cm = 200, Price = 3411m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 435, Width_cm = 300, Projection_cm = 200, Price = 3696m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 436, Width_cm = 350, Projection_cm = 200, Price = 4029m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 437, Width_cm = 400, Projection_cm = 200, Price = 4308m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 438, Width_cm = 450, Projection_cm = 200, Price = 4635m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 439, Width_cm = 500, Projection_cm = 200, Price = 4876m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 440, Width_cm = 550, Projection_cm = 200, Price = 5176m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 441, Width_cm = 600, Projection_cm = 200, Price = 5470m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 442, Width_cm = 650, Projection_cm = 200, Price = 6117m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 443, Width_cm = 700, Projection_cm = 200, Price = 6810m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // 250 cm
                new { ProjectionId = 444, Width_cm = 300, Projection_cm = 250, Price = 3934m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 445, Width_cm = 350, Projection_cm = 250, Price = 4252m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 446, Width_cm = 400, Projection_cm = 250, Price = 4583m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 447, Width_cm = 450, Projection_cm = 250, Price = 4929m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 448, Width_cm = 500, Projection_cm = 250, Price = 5166m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 449, Width_cm = 550, Projection_cm = 250, Price = 5484m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 450, Width_cm = 600, Projection_cm = 250, Price = 5745m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 451, Width_cm = 650, Projection_cm = 250, Price = 6376m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 452, Width_cm = 700, Projection_cm = 250, Price = 7118m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // 300 cm
                new { ProjectionId = 453, Width_cm = 350, Projection_cm = 300, Price = 4552m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 454, Width_cm = 400, Projection_cm = 300, Price = 4877m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 455, Width_cm = 450, Projection_cm = 300, Price = 5148m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 456, Width_cm = 500, Projection_cm = 300, Price = 5469m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 457, Width_cm = 550, Projection_cm = 300, Price = 5778m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 458, Width_cm = 600, Projection_cm = 300, Price = 6031m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 459, Width_cm = 650, Projection_cm = 300, Price = 6647m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 460, Width_cm = 700, Projection_cm = 300, Price = 7543m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // 350 cm
                new { ProjectionId = 461, Width_cm = 400, Projection_cm = 350, Price = 5405m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 462, Width_cm = 450, Projection_cm = 350, Price = 5731m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 463, Width_cm = 500, Projection_cm = 350, Price = 6060m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 464, Width_cm = 550, Projection_cm = 350, Price = 6360m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 465, Width_cm = 600, Projection_cm = 350, Price = 6647m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 466, Width_cm = 650, Projection_cm = 350, Price = 7647m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 467, Width_cm = 700, Projection_cm = 350, Price = 7953m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // 400 cm
                new { ProjectionId = 468, Width_cm = 450, Projection_cm = 400, Price = 6309m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 469, Width_cm = 500, Projection_cm = 400, Price = 6660m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 470, Width_cm = 550, Projection_cm = 400, Price = 6973m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 471, Width_cm = 600, Projection_cm = 400, Price = 7269m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ProjectionId = 472, Width_cm = 700, Projection_cm = 400, Price = 8517m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

            // 150 cm
            new { ProjectionId = 473, Width_cm = 500, Projection_cm = 150, Price = 6548m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 474, Width_cm = 600, Projection_cm = 150, Price = 7039m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 475, Width_cm = 700, Projection_cm = 150, Price = 7707m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 476, Width_cm = 800, Projection_cm = 150, Price = 8298m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 477, Width_cm = 900, Projection_cm = 150, Price = 8960m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 478, Width_cm = 1000, Projection_cm = 150, Price = 9532m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 479, Width_cm = 1100, Projection_cm = 150, Price = 10050m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 480, Width_cm = 1200, Projection_cm = 150, Price = 10554m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 481, Width_cm = 1300, Projection_cm = 150, Price = 11862m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 482, Width_cm = 1390, Projection_cm = 150, Price = 13171m, ProductId = 13, ArmTypeId = 19, DateCreated = staticCreatedDate, CreatedBy = "System" },

            // 200 cm
            new { ProjectionId = 483, Width_cm = 500, Projection_cm = 200, Price = 6880m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 484, Width_cm = 600, Projection_cm = 200, Price = 7451m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 485, Width_cm = 700, Projection_cm = 200, Price = 8114m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 486, Width_cm = 800, Projection_cm = 200, Price = 8667m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 487, Width_cm = 900, Projection_cm = 200, Price = 9318m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 488, Width_cm = 1000, Projection_cm = 200, Price = 9806m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 489, Width_cm = 1100, Projection_cm = 200, Price = 10408m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 490, Width_cm = 1200, Projection_cm = 200, Price = 10993m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 491, Width_cm = 1300, Projection_cm = 200, Price = 12285m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 492, Width_cm = 1390, Projection_cm = 200, Price = 13684m, ProductId = 13, ArmTypeId = 19, DateCreated = staticCreatedDate, CreatedBy = "System" },

            // 250 cm
            new { ProjectionId = 493, Width_cm = 600, Projection_cm = 250, Price = 7925m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 494, Width_cm = 700, Projection_cm = 250, Price = 8553m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 495, Width_cm = 800, Projection_cm = 250, Price = 9213m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 496, Width_cm = 900, Projection_cm = 250, Price = 9915m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 497, Width_cm = 1000, Projection_cm = 250, Price = 10385m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 498, Width_cm = 1100, Projection_cm = 250, Price = 11012m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 499, Width_cm = 1200, Projection_cm = 250, Price = 11546m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 500, Width_cm = 1300, Projection_cm = 250, Price = 12814m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 501, Width_cm = 1390, Projection_cm = 250, Price = 14290m, ProductId = 13, ArmTypeId = 19, DateCreated = staticCreatedDate, CreatedBy = "System" },

            // 300 cm
            new { ProjectionId = 502, Width_cm = 700, Projection_cm = 300, Price = 9159m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 503, Width_cm = 800, Projection_cm = 300, Price = 9803m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 504, Width_cm = 900, Projection_cm = 300, Price = 10351m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 505, Width_cm = 1000, Projection_cm = 300, Price = 10987m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 506, Width_cm = 1100, Projection_cm = 300, Price = 11606m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 507, Width_cm = 1200, Projection_cm = 300, Price = 12114m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 508, Width_cm = 1300, Projection_cm = 300, Price = 13337m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 509, Width_cm = 1390, Projection_cm = 300, Price = 15143m, ProductId = 13, ArmTypeId = 19, DateCreated = staticCreatedDate, CreatedBy = "System" },

            // 350 cm
            new { ProjectionId = 510, Width_cm = 800, Projection_cm = 350, Price = 10856m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 511, Width_cm = 900, Projection_cm = 350, Price = 11520m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 512, Width_cm = 1000, Projection_cm = 350, Price = 12174m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 513, Width_cm = 1100, Projection_cm = 350, Price = 12778m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 514, Width_cm = 1200, Projection_cm = 350, Price = 13348m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 515, Width_cm = 1300, Projection_cm = 350, Price = 15289m, ProductId = 13, ArmTypeId = 18, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 516, Width_cm = 1390, Projection_cm = 350, Price = 15958m, ProductId = 13, ArmTypeId = 18, DateCreated = staticCreatedDate, CreatedBy = "System" },

            // 400 cm
            new { ProjectionId = 517, Width_cm = 1000, Projection_cm = 400, Price = 13371m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 518, Width_cm = 1100, Projection_cm = 400, Price = 14005m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 519, Width_cm = 1200, Projection_cm = 400, Price = 14590m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },

            new { ProjectionId = 520, Width_cm = 250, Projection_cm = 150, Price = 2177m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 521, Width_cm = 300, Projection_cm = 150, Price = 2304m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 522, Width_cm = 350, Projection_cm = 150, Price = 2461m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 523, Width_cm = 400, Projection_cm = 150, Price = 2606m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 524, Width_cm = 450, Projection_cm = 150, Price = 2757m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 525, Width_cm = 500, Projection_cm = 150, Price = 2894m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 526, Width_cm = 550, Projection_cm = 150, Price = 3074m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 527, Width_cm = 600, Projection_cm = 150, Price = 3217m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 528, Width_cm = 650, Projection_cm = 150, Price = 3439m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 529, Width_cm = 700, Projection_cm = 150, Price = 3976m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

            new { ProjectionId = 530, Width_cm = 250, Projection_cm = 200, Price = 2309m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 531, Width_cm = 300, Projection_cm = 200, Price = 2457m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 532, Width_cm = 350, Projection_cm = 200, Price = 2620m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 533, Width_cm = 400, Projection_cm = 200, Price = 2788m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 534, Width_cm = 450, Projection_cm = 200, Price = 2923m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 535, Width_cm = 500, Projection_cm = 200, Price = 3095m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 536, Width_cm = 550, Projection_cm = 200, Price = 3288m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 537, Width_cm = 600, Projection_cm = 200, Price = 3449m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 538, Width_cm = 650, Projection_cm = 200, Price = 3679m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 539, Width_cm = 700, Projection_cm = 200, Price = 4254m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

            new { ProjectionId = 540, Width_cm = 300, Projection_cm = 250, Price = 2905m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 541, Width_cm = 350, Projection_cm = 250, Price = 2758m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 542, Width_cm = 400, Projection_cm = 250, Price = 2934m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 543, Width_cm = 450, Projection_cm = 250, Price = 3107m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 544, Width_cm = 500, Projection_cm = 250, Price = 3328m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 545, Width_cm = 550, Projection_cm = 250, Price = 3502m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 546, Width_cm = 600, Projection_cm = 250, Price = 3736m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 547, Width_cm = 650, Projection_cm = 250, Price = 3922m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 548, Width_cm = 700, Projection_cm = 250, Price = 4551m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

            new { ProjectionId = 549, Width_cm = 350, Projection_cm = 300, Price = 2905m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 550, Width_cm = 400, Projection_cm = 300, Price = 3099m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 551, Width_cm = 450, Projection_cm = 300, Price = 3288m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 552, Width_cm = 500, Projection_cm = 300, Price = 3523m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 553, Width_cm = 550, Projection_cm = 300, Price = 3769m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 554, Width_cm = 600, Projection_cm = 300, Price = 3957m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 555, Width_cm = 650, Projection_cm = 300, Price = 4165m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 556, Width_cm = 700, Projection_cm = 300, Price = 4833m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

            new { ProjectionId = 557, Width_cm = 400, Projection_cm = 350, Price = 3262m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 558, Width_cm = 450, Projection_cm = 350, Price = 3522m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 559, Width_cm = 500, Projection_cm = 350, Price = 3732m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 560, Width_cm = 550, Projection_cm = 350, Price = 3992m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 561, Width_cm = 600, Projection_cm = 350, Price = 4191m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 562, Width_cm = 650, Projection_cm = 350, Price = 4887m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 563, Width_cm = 700, Projection_cm = 350, Price = 5092m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

            new { ProjectionId = 564, Width_cm = 250, Projection_cm = 150, Price = 1941m, ProductId = 15, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 565, Width_cm = 300, Projection_cm = 150, Price = 2074m, ProductId = 15, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 566, Width_cm = 350, Projection_cm = 150, Price = 2279m, ProductId = 15, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 567, Width_cm = 400, Projection_cm = 150, Price = 2429m, ProductId = 15, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },

            new { ProjectionId = 568, Width_cm = 250, Projection_cm = 200, Price = 2035m, ProductId = 15, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 569, Width_cm = 300, Projection_cm = 200, Price = 2182m, ProductId = 15, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 570, Width_cm = 350, Projection_cm = 200, Price = 2360m, ProductId = 15, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
            new { ProjectionId = 571, Width_cm = 400, Projection_cm = 200, Price = 2529m, ProductId = 15, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" }

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
                new { BracketId = 100, ProductId = 5, BracketName = "Cover plate 320x210x2 mm", PartNumber = "71842", Price = 21.90m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = (int?)null },
                //Markilie - 6000 couple
                new { BracketId = 101, ProductId = 6, BracketName = "Surcharge for face fixture", Price = 596m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 10 },
                new { BracketId = 102, ProductId = 6, BracketName = "Surcharge for face fixture", Price = 894m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 12 },
                new { BracketId = 103, ProductId = 6, BracketName = "Surcharge for face fixture", Price = 1192m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 18 },

                new { BracketId = 104, ProductId = 6, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 1107m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 10 },
                new { BracketId = 105, ProductId = 6, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 1421m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 12 },
                new { BracketId = 106, ProductId = 6, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 1975m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 18 },

                new { BracketId = 107, ProductId = 6, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 1256m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 10 },
                new { BracketId = 108, ProductId = 6, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 1570m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 12 },
                new { BracketId = 109, ProductId = 6, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 2198m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 18 },

                new { BracketId = 110, ProductId = 6, BracketName = "Surcharge for top fixture", Price = 794m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 10 },
                new { BracketId = 111, ProductId = 6, BracketName = "Surcharge for top fixture", Price = 1191m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 12 },
                new { BracketId = 112, ProductId = 6, BracketName = "Surcharge for top fixture", Price = 1588m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 18 },

                new { BracketId = 113, ProductId = 6, BracketName = "Surcharge for eaves fixture", Price = 950m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 10 },
                new { BracketId = 114, ProductId = 6, BracketName = "Surcharge for eaves fixture", Price = 1424m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 12 },
                new { BracketId = 115, ProductId = 6, BracketName = "Surcharge for eaves fixture", Price = 1899m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 18 },

                new { BracketId = 116, ProductId = 6, BracketName = "Surcharge for bespoke arms", Price = 361m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 10 },
                new { BracketId = 117, ProductId = 6, BracketName = "Surcharge for bespoke arms", Price = 361m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 12 },
                new { BracketId = 118, ProductId = 6, BracketName = "Surcharge for bespoke arms", Price = 536m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 18 },

                new { BracketId = 119, ProductId = 6, BracketName = "Surcharge for junction roller", Price = 291m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },
                new { BracketId = 120, ProductId = 6, BracketName = "Surcharge for one-piece cover", Price = 291m, DateCreated = staticCreatedDate, CreatedBy = "System", ArmTypeId = 1 },

                new { BracketId = 121, BracketName = "Surcharge for face fixture", Price = 113m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 122, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 369m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 123, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 443m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 124, BracketName = "Surcharge for top fixture", Price = 122m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 125, BracketName = "Surcharge for eaves fixture", Price = 224m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 126, BracketName = "Surcharge for bespoke arms", Price = 183m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 127, BracketName = "Face fixture bracket 100 mm left / 3", PartNumber = "75885", Price = 56.40m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 128, BracketName = "Face fixture bracket 100 mm right / 3", PartNumber = "75886", Price = 56.40m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 129, BracketName = "Face fixture bracket 300 mm left / 4", PartNumber = "75877", Price = 89.50m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 130, BracketName = "Face fixture bracket 300 mm right / 4", PartNumber = "75878", Price = 89.50m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 131, BracketName = "Stand-off bkt. 80-300 mm for face fixture bkt. / 4", PartNumber = "72872", Price = 253.50m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 132, BracketName = "Stand-off bkt. 80-300 mm for face fixture for face fixture bracket 300 mm / 4", PartNumber = "77968", Price = 226.90m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 133, BracketName = "Top fixture bracket 100 mm left / 4", PartNumber = "75887", Price = 61m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 134, BracketName = "Top fixture bracket 100 mm right / 4", PartNumber = "75888", Price = 61m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 135, BracketName = "Eaves fixture bracket 150 mm / 4", PartNumber = "75889", Price = 50.70m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 136, BracketName = "Eaves fixture bracket 270 mm / 4", PartNumber = "71659", Price = 79.30m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 137, BracketName = "Vertical fixture rail incl. fixing material 624291", PartNumber = "62421", Price = 180m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 138, BracketName = "Angle and plate for eaves fixture (machine finish) / 4", PartNumber = "716620", Price = 128.90m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 139, BracketName = "Additional eaves fixture plate 60x260x12 mm / 2", PartNumber = "75383", Price = 43.90m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 140, BracketName = "Spreader plate A 430x160x12 mm / 8", PartNumber = "73466", Price = 127.70m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 141, BracketName = "Spreader plate B 300x400x12 mm / 4", PartNumber = "73465", Price = 164.90m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 142, BracketName = "Spacer block face fixture 100x150x20 mm / 3", PartNumber = "758831", Price = 8.60m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 143, BracketName = "Spacer block face fixture 100x150x12 mm / 3", PartNumber = "758841", Price = 7.50m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 144, BracketName = "Spacer block for top fixture 90x140x20 mm / 4", PartNumber = "716311", Price = 4.40m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 145, BracketName = "Spacer block for top fixture 90x140x12 mm / 4", PartNumber = "716411", Price = 5m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 146, BracketName = "Cover plate 230x210x2 mm", PartNumber = "71843", Price = 17m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                // ProductId = 8
                new { BracketId = 153, BracketName = "Face fixture bracket 150 mm / 3", PartNumber = "71624", Price = 44m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 154, BracketName = "Face fixture bracket 300 mm left / 4", PartNumber = "70617", Price = 75.70m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 155, BracketName = "Face fixture bracket 300 mm right / 4", PartNumber = "70600", Price = 75.70m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 156, BracketName = "Stand-off bkt. 80-300 mm for face fixture bracket 300 mm / 4", PartNumber = "77968", Price = 226.90m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 157, BracketName = "Top fixture bracket 150 mm / 4", PartNumber = "71625", Price = 44m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 158, BracketName = "Eaves fixture bracket 150mm, complete / 4", PartNumber = "71669", Price = 102.20m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 159, BracketName = "Eaves fixture bracket 270 mm / 4", PartNumber = "71659", Price = 79.30m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 160, BracketName = "Angle and plate for eaves fixture (machine finish) / 4", PartNumber = "716620", Price = 128.90m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 161, BracketName = "Additional eaves fixture plate 60x260x12 mm / 2", PartNumber = "75383", Price = 43.90m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 162, BracketName = "Spreader plate A 430x160x12 mm / 8", PartNumber = "75326", Price = 127.70m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 163, BracketName = "Spreader plate B 300x400x12 mm / 4", PartNumber = "75325", Price = 164.90m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 164, BracketName = "Spacer block face or top fixt.136x150x20 mm / 3", PartNumber = "716331", Price = 5.70m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 165, BracketName = "Spacer block face or top fixt.136x150x12 mm / 3", PartNumber = "71644", Price = 3.80m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 166, BracketName = "Cover plate 230x210x2 mm", PartNumber = "71843", Price = 17m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 167, BracketName = "Vertical fixture rail incl. fixing material 624291", PartNumber = "62421", Price = 180m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                // ProductId = 9
                new { BracketId = 174, BracketName = "Face fixture bracket left / 3", PartNumber = "72826", Price = 109.80m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 175, BracketName = "Face fixture bracket right / 3", PartNumber = "72827", Price = 109.80m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 176, BracketName = "Stand-off bkt. 80-300 mm for face fixture bkt. / 4", PartNumber = "72872", Price = 253.50m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 177, BracketName = "Top fixture bracket left / 4", PartNumber = "72860", Price = 138.70m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 178, BracketName = "Top fixture bracket right / 4", PartNumber = "72861", Price = 138.70m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 179, BracketName = "Eaves fixture bracket left 150 mm, complete / 4", PartNumber = "72874", Price = 185.30m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 180, BracketName = "Eaves fixture bracket right 150 mm, complete / 4", PartNumber = "72875", Price = 185.30m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 181, BracketName = "Eaves fixture bracket 270 mm / 4", PartNumber = "71659", Price = 79.30m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 182, BracketName = "Angle and plate for eaves fixture (machine finish) / 4", PartNumber = "716620", Price = 128.90m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 183, BracketName = "Additional eaves fixture plate 60x260x12 mm / 2", PartNumber = "75383", Price = 43.90m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 184, BracketName = "Spreader plate A 430x160x12 mm / 8", PartNumber = "72870", Price = 186m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 185, BracketName = "Spreader plate B 300x400x12 mm / 4", PartNumber = "73465", Price = 164.90m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 186, BracketName = "Spreader Plate C 310x130x12 mm / 6", PartNumber = "72526", Price = 186m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 187, BracketName = "Spacer block face fixture 100x120x20 mm / 3", PartNumber = "718581", Price = 14.70m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 188, BracketName = "Spacer block face fixture 100x120x12 mm / 3", PartNumber = "718571", Price = 14.30m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 189, BracketName = "Spacer block for top fixture 90x140x20 mm / 4", PartNumber = "716311", Price = 4.40m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 190, BracketName = "Spacer block for top fixture 90x140x12 mm / 4", PartNumber = "716411", Price = 5m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 191, BracketName = "Cover plate 230x210x2 mm", PartNumber = "71843", Price = 17m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 192, BracketName = "Vertical fixture rail incl. fixing material 624291", PartNumber = "62421", Price = 180m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 193, BracketName = "Surcharge for face fixture", Price = 263m, ProductId = 10, ArmTypeId = 2, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 194, BracketName = "Surcharge for face fixture", Price = 394m, ProductId = 10, ArmTypeId = 3, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 195, BracketName = "Surcharge for face fixture", Price = 525m, ProductId = 10, ArmTypeId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 196, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 518m, ProductId = 10, ArmTypeId = 2, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 197, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 653m, ProductId = 10, ArmTypeId = 3, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 198, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 912m, ProductId = 10, ArmTypeId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 199, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 593m, ProductId = 10, ArmTypeId = 2, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 200, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 728m, ProductId = 10, ArmTypeId = 3, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 201, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 1024m, ProductId = 10, ArmTypeId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 202, BracketName = "Surcharge for top fixture", Price = 326m, ProductId = 10, ArmTypeId = 2, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 203, BracketName = "Surcharge for top fixture", Price = 489m, ProductId = 10, ArmTypeId = 3, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 204, BracketName = "Surcharge for top fixture", Price = 652m, ProductId = 10, ArmTypeId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 205, BracketName = "Surcharge for eaves fixture", Price = 404m, ProductId = 10, ArmTypeId = 2, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 206, BracketName = "Surcharge for eaves fixture", Price = 605m, ProductId = 10, ArmTypeId = 3, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 207, BracketName = "Surcharge for eaves fixture", Price = 807m, ProductId = 10, ArmTypeId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 208, BracketName = "Surcharge for bespoke arms", Price = 183m, ProductId = 10, ArmTypeId = 2, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 209, BracketName = "Surcharge for bespoke arms", Price = 183m, ProductId = 10, ArmTypeId = 3, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 210, BracketName = "Surcharge for bespoke arms", Price = 269m, ProductId = 10, ArmTypeId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 211, BracketName = "Surcharge for arms with bionic tendon", Price = 121m, ProductId = 10, ArmTypeId = 2, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 212, BracketName = "Surcharge for arms with bionic tendon", Price = 121m, ProductId = 10, ArmTypeId = 3, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 213, BracketName = "Surcharge for arms with bionic tendon", Price = 177m, ProductId = 10, ArmTypeId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                // Part-number rows (converted)
                new { BracketId = 214, BracketName = "Face fixture bracket assembly 5 - 35° / 4", PartNumber = "77921", Price = 131.20m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 215, BracketName = "Face fixture bracket assembly 38 - 65° / 4", PartNumber = "77936", Price = 131.20m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 216, BracketName = "Stand-off bkt. 80-300 mm for face fixture bkt. / 4", PartNumber = "77969", Price = 238.60m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 217, BracketName = "Top fixture bracket 5 - 35° / 4", PartNumber = "77937", Price = 163m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 218, BracketName = "Top fixture bracket 38 - 65° / 4", PartNumber = "77938", Price = 245.50m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 219, BracketName = "Eaves fixture bracket 150mm, complete / 4", PartNumber = "77939", Price = 201.60m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 220, BracketName = "Eaves fixture bracket 270 mm, complete / 4", PartNumber = "77940", Price = 231.90m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 221, BracketName = "Angle and plate for eaves fixture (machine finish) / 4", PartNumber = "741290", Price = 142.80m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 222, BracketName = "Additional eaves fixture plate 60x260x12 mm / 2", PartNumber = "75383", Price = 43.90m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 223, BracketName = "Spreader plate A 430x160x12 mm / 8", PartNumber = "75328", Price = 127.70m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 224, BracketName = "Spreader plate B 300x400x12 mm / 4", PartNumber = "75327", Price = 164.90m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 225, BracketName = "Spacer block face or top fixt.136x150x20 mm / 4", PartNumber = "716331", Price = 5.70m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 226, BracketName = "Spacer block face or top fixt.136x150x12 mm / 4", PartNumber = "71644", Price = 3.80m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 227, BracketName = "Cover plate 290x210x2 mm", PartNumber = "71841", Price = 21.10m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 228, BracketName = "Bottom fixture bracket 150 mm, 5 - 35° / 4", PartNumber = "77941", Price = 245.50m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 229, BracketName = "Surcharge for face fixture", Price = 525m, ProductId = 11, ArmTypeId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 230, BracketName = "Surcharge for face fixture", Price = 788m, ProductId = 11, ArmTypeId = 12, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 231, BracketName = "Surcharge for face fixture", Price = 1050m, ProductId = 11, ArmTypeId = 18, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 232, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 1036m, ProductId = 11, ArmTypeId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 233, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 1306m, ProductId = 11, ArmTypeId = 12, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 234, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 1824m, ProductId = 11, ArmTypeId = 18, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 235, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 1185m, ProductId = 11, ArmTypeId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 236, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 1455m, ProductId = 11, ArmTypeId = 12, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 237, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 2047m, ProductId = 11, ArmTypeId = 18, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 238, BracketName = "Surcharge for top fixture", Price = 652m, ProductId = 11, ArmTypeId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 239, BracketName = "Surcharge for top fixture", Price = 978m, ProductId = 11, ArmTypeId = 12, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 240, BracketName = "Surcharge for top fixture", Price = 1304m, ProductId = 11, ArmTypeId = 18, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 241, BracketName = "Surcharge for eaves fixture", Price = 807m, ProductId = 11, ArmTypeId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 242, BracketName = "Surcharge for eaves fixture", Price = 1210m, ProductId = 11, ArmTypeId = 12, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 243, BracketName = "Surcharge for eaves fixture", Price = 1613m, ProductId = 11, ArmTypeId = 18, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 244, BracketName = "Surcharge for bespoke arms", Price = 361m, ProductId = 11, ArmTypeId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 245, BracketName = "Surcharge for bespoke arms", Price = 361m, ProductId = 11, ArmTypeId = 12, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 246, BracketName = "Surcharge for bespoke arms", Price = 536m, ProductId = 11, ArmTypeId = 18, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 247, BracketName = "Surcharge for arms with bionic tendon", Price = 234m, ProductId = 11, ArmTypeId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 248, BracketName = "Surcharge for arms with bionic tendon", Price = 234m, ProductId = 11, ArmTypeId = 12, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 249, BracketName = "Surcharge for arms with bionic tendon", Price = 348m, ProductId = 11, ArmTypeId = 18, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { BracketId = 250, BracketName = "Surcharge for junction roller", Price = 291m, ProductId = 11, ArmTypeId = 1, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { BracketId = 251, BracketName = "Surcharge for one-piece cover", Price = 291m, ProductId = 11, ArmTypeId = 1, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                // ProductId = 12 surcharge rows
                new { BracketId = 252, BracketName = "Surcharge for face fixture", Price = 79m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 253, BracketName = "Surcharge for face fixture", Price = 130m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 254, BracketName = "Surcharge for face fixture", Price = 156m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 255, BracketName = "Surcharge for face fixture", Price = 195m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 256, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 335m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 257, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 394m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 258, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 424m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 259, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 591m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 260, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 409m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 261, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 468m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 262, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 498m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 263, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 702m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 264, BracketName = "Surcharge for top fixture", Price = 140m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 265, BracketName = "Surcharge for top fixture", Price = 279m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 266, BracketName = "Surcharge for top fixture", Price = 348m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 267, BracketName = "Surcharge for top fixture", Price = 418m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 268, BracketName = "Surcharge for eaves fixture", Price = 246m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 269, BracketName = "Surcharge for eaves fixture", Price = 492m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 270, BracketName = "Surcharge for eaves fixture", Price = 615m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 271, BracketName = "Surcharge for eaves fixture", Price = 738m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 272, BracketName = "Surcharge for bespoke arms", Price = 183m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 273, BracketName = "Surcharge for bespoke arms", Price = 183m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 274, BracketName = "Surcharge for bespoke arms", Price = 183m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 275, BracketName = "Surcharge for bespoke arms", Price = 269m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Part-number rows (converted)
                new { BracketId = 276, BracketName = "Face fixture bracket 200 mm / 4", PartNumber = "71966", Price = 66.40m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 277, BracketName = "Face fixture bracket 100 mm / 3", PartNumber = "71964", Price = 39.50m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 278, BracketName = "Face fixture bracket 60 mm / 2", PartNumber = "71955", Price = 25.50m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 279, BracketName = "Stand-off bkt. 80-300 mm for face fixture for face fixture bracket 60 mm / 4", PartNumber = "77967", Price = 226.90m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 280, BracketName = "Top fixture bracket 90 mm / 4", PartNumber = "77967", Price = 69.60m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 281, BracketName = "Top fixture bracket 200 mm / 4", PartNumber = "71652", Price = 138.30m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 282, BracketName = "Eaves fixture bracket 150mm, complete / 4", PartNumber = "71898", Price = 123m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 283, BracketName = "Eaves fixture bracket 270 mm / 4", PartNumber = "71659", Price = 79.30m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 284, BracketName = "Angle and plate for eaves fixture (machine finish) / 4", PartNumber = "716620", Price = 128.90m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 285, BracketName = "Additional eaves fixture plate 60x260x12 mm / 2", PartNumber = "75383", Price = 43.90m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 286, BracketName = "Spreader plate A 430x160x12 mm / 8", PartNumber = "75324", Price = 127.70m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 287, BracketName = "Spreader plate B 300x400x12 mm / 4", PartNumber = "75323", Price = 164.90m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 288, BracketName = "Spacer block face fixture 100x150x20 mm / 3", PartNumber = "758831", Price = 8.60m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 289, BracketName = "Spacer block face fixture 60x140x20 mm / 2", PartNumber = "716321", Price = 4.10m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 290, BracketName = "Spacer block face fixture 60x140x12 mm / 2", PartNumber = "71642", Price = 4.10m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 291, BracketName = "Spacer block for top fixture 90x140x20 mm / 4", PartNumber = "716311", Price = 4.40m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 292, BracketName = "Cover plate 230x210x2 mm for face fixture bracket 100 mm", PartNumber = "71843", Price = 17m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },


                new { BracketId = 295, BracketName = "Surcharge for face fixture", Price = 158m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 296, BracketName = "Surcharge for face fixture", Price = 260m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 297, BracketName = "Surcharge for face fixture", Price = 311m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 298, BracketName = "Surcharge for face fixture", Price = 390m, ProductId = 13, ArmTypeId = 19, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 299, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 669m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 300, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 788m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 301, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 847m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 302, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 1181m, ProductId = 13, ArmTypeId = 19, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 303, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 818m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 304, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 936m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 305, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 996m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 306, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 1404m, ProductId = 13, ArmTypeId = 19, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 307, BracketName = "Surcharge for top fixture", Price = 279m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 308, BracketName = "Surcharge for top fixture", Price = 557m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 309, BracketName = "Surcharge for top fixture", Price = 696m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 310, BracketName = "Surcharge for top fixture", Price = 836m, ProductId = 13, ArmTypeId = 19, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 311, BracketName = "Surcharge for eaves fixture", Price = 492m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 312, BracketName = "Surcharge for eaves fixture", Price = 984m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 313, BracketName = "Surcharge for eaves fixture", Price = 1230m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 314, BracketName = "Surcharge for eaves fixture", Price = 1476m, ProductId = 13, ArmTypeId = 19, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 315, BracketName = "Surcharge for bespoke arms", Price = 361m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 316, BracketName = "Surcharge for bespoke arms", Price = 361m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 317, BracketName = "Surcharge for bespoke arms", Price = 361m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 318, BracketName = "Surcharge for bespoke arms", Price = 536m, ProductId = 13, ArmTypeId = 19, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 319, BracketName = "Surcharge for face fixture", Price = 146m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 320, BracketName = "Surcharge for face fixture", Price = 196m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 321, BracketName = "Surcharge for face fixture", Price = 269m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 322, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 402m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 323, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 455m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 324, BracketName = "Surcharge for face fixture incl. spreader plate A", Price = 656m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 325, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 476m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 326, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 530m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 327, BracketName = "Surcharge for face fixture incl. spreader plate B", Price = 767m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 328, BracketName = "Surcharge for top fixture", Price = 196m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 329, BracketName = "Surcharge for top fixture", Price = 260m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 330, BracketName = "Surcharge for top fixture", Price = 358m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 331, BracketName = "Surcharge for eaves fixture", Price = 290m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 332, BracketName = "Surcharge for eaves fixture", Price = 435m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 333, BracketName = "Surcharge for eaves fixture", Price = 580m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 334, BracketName = "Surcharge for bespoke arms", Price = 183m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 335, BracketName = "Surcharge for bespoke arms", Price = 183m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 336, BracketName = "Surcharge for bespoke arms", Price = 269m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

                // Part-numbered brackets
                new { BracketId = 337, BracketName = "Face fixture bracket 100 mm / 3", PartNumber = "70867", Price = 72.90m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 338, BracketName = "Face fixture bracket 45 mm / 2", PartNumber = "71813", Price = 50.10m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 339, BracketName = "Stand-off bracket 80-300 mm for face fixture bracket 100 mm", PartNumber = "77969", Price = 226.90m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 340, BracketName = "Stand-off bracket 80-300 mm for 45 mm face fixture bracket", PartNumber = "77967", Price = 226.90m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 341, BracketName = "Top fixture bracket 90 mm / 4", PartNumber = "70868", Price = 97.70m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 342, BracketName = "Top fixture bracket 45 mm / 2", PartNumber = "71818", Price = 64.30m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 343, BracketName = "Eaves fixture bracket 150mm, complete / 4", PartNumber = "70871", Price = 144.90m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 344, BracketName = "Eaves fixture bracket 270 mm / 4", PartNumber = "71659", Price = 79.30m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 345, BracketName = "Adjustable eaves fixture bracket / 2", PartNumber = "71198", Price = 139.90m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 346, BracketName = "Angle and plate for eaves fixture (machine finish) / 4", PartNumber = "716620", Price = 128.90m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 347, BracketName = "Vertical fixture rail incl. fixing material 624291", PartNumber = "62421", Price = 180m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 348, BracketName = "Additional eaves fixture plate 60x260x12 mm / 2", PartNumber = "75383", Price = 43.90m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 349, BracketName = "Spreader plate A 430x160x12 mm / 8", PartNumber = "75326", Price = 127.70m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 350, BracketName = "Spreader plate B 300x400x12 mm / 4", PartNumber = "75325", Price = 164.90m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 351, BracketName = "Spacer block face fixture 100x150x20 mm / 3", PartNumber = "758831", Price = 8.60m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 352, BracketName = "Spacer block face fixture 45x150x20 mm / 2", PartNumber = "718251", Price = 4m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 353, BracketName = "Spacer block face fixture 100x150x12 mm / 3", PartNumber = "758841", Price = 7.50m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 354, BracketName = "Spacer block face fixture 45x150x12 mm / 2", PartNumber = "71826", Price = 3.50m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 355, BracketName = "Spacer block for top fixture 90x140x20 mm / 4", PartNumber = "716311", Price = 4.40m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 356, BracketName = "Spacer block for top fixture 45x140x20 mm / 2", PartNumber = "716261", Price = 3.40m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 357, BracketName = "Cover plate 230x210x2 mm for face fixture bracket 100 mm", PartNumber = "71843", Price = 17m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 358, BracketName = "Cover plate 210x230x2 mm for face fixture bracket 45 mm", PartNumber = "71844", Price = 15.80m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { BracketId = 365, BracketName = "Face fixture bracket right / top bracket left / 4", PartNumber = "62247", Price = 99.10m, ProductId = 15, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 366, BracketName = "Face fixture bracket left / Top bracket right / 4", PartNumber = "62248", Price = 99.10m, ProductId = 15, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 367, BracketName = "Eaves fixture bracket 150 mm / 4", PartNumber = "75889", Price = 50.70m, ProductId = 15, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 368, BracketName = "Eaves fixture bracket 270 mm / 4", PartNumber = "71659", Price = 79.30m, ProductId = 15, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 369, BracketName = "Spacer block face or top fixt.136x150x20 mm / 3", PartNumber = "716331", Price = 5.70m, ProductId = 15, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 370, BracketName = "Spacer block face or top fixt.136x150x12 mm / 3", PartNumber = "71644", Price = 3.80m, ProductId = 15, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 371, BracketName = "Angle and plate for eaves fixture (machine finish) / 4", PartNumber = "716620", Price = 128.90m, ProductId = 15, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { BracketId = 372, BracketName = "Additional eaves fixture plate 60x260x12 mm / 2", PartNumber = "75383", Price = 43.90m, ProductId = 15, DateCreated = staticCreatedDate, CreatedBy = "System" }

            );


            //Arms Data — values moved to Brackets above; Arms table left empty
            modelBuilder.Entity<Arms>().HasData(
            );
            // Markilux 6000 Single — Motors
            modelBuilder.Entity<Motors>().HasData(
                // Servo-assisted gear
                new { MotorId = 1, Description = "Surcharge for servo-assisted gear", Price = 75m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
                new { MotorId = 2, Description = "Surcharge for servo-assisted gear", Price = 75m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
                new { MotorId = 3, Description = "Surcharge for servo-assisted gear", Price = 0m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 8 },

                // Hard-wired motor
                new { MotorId = 4, Description = "Surcharge for hard-wired motor", Price = 484m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
                new { MotorId = 5, Description = "Surcharge for hard-wired motor", Price = 484m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
                new { MotorId = 6, Description = "Surcharge for hard-wired motor", Price = 574m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 8 },

                // Radio-controlled io + 1 ch transmitter
                new { MotorId = 7, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
                new { MotorId = 8, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
                new { MotorId = 9, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 809m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 8 },

                // Radio-controlled io w/o transmitter
                new { MotorId = 10, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
                new { MotorId = 11, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
                new { MotorId = 12, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 691m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 8 },

                // Radio-controlled io with manual override + 1 ch transmitter
                new { MotorId = 13, Description = "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", Price = 1115m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
                new { MotorId = 14, Description = "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", Price = 1115m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
                new { MotorId = 15, Description = "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", Price = 0m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 8 },

                // Radio-controlled io with manual override w/o transmitter
                new { MotorId = 16, Description = "Surcharge for radio-contr. motor io with manual override w/o transmitter", Price = 997m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 2 },
                new { MotorId = 17, Description = "Surcharge for radio-contr. motor io with manual override w/o transmitter", Price = 997m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 3 },
                new { MotorId = 18, Description = "Surcharge for radio-contr. motor io with manual override w/o transmitter", Price = 0m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5, ArmTypeId = 8 },
                new { MotorId = 19, Description = "Surcharge for hard-wired motor", Price = 574m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
                new { MotorId = 20, Description = "Surcharge for hard-wired motor", Price = 574m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
                new { MotorId = 21, Description = "Surcharge for hard-wired motor", Price = 682m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 18 },

                new { MotorId = 22, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 809m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
                new { MotorId = 23, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 809m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
                new { MotorId = 24, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 916m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 18 },

                new { MotorId = 25, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 691m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 10 },
                new { MotorId = 26, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 691m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 12 },
                new { MotorId = 27, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 798m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6, ArmTypeId = 18 },

                new { MotorId = 28, Description = "Surcharge for servo-assisted gear", Price = 75m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7 },
                new { MotorId = 29, Description = "Surcharge for hard-wired motor", Price = 484m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7 },
                new { MotorId = 30, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7 },
                new { MotorId = 31, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7 },

                new { MotorId = 32, Description = "Surcharge for servo-assisted gear", Price = 75m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8 },
                new { MotorId = 33, Description = "Surcharge for hard-wired motor", Price = 484m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8 },
                new { MotorId = 34, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8 },
                new { MotorId = 35, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8 },
                new { MotorId = 36, Description = "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", Price = 1115m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8 },
                new { MotorId = 37, Description = "Surcharge for radio-contr. motor io with manual override w/o transmitter", Price = 997m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 8 },
                new { MotorId = 38, Description = "Surcharge for servo-assisted gear", Price = 75m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 39, Description = "Surcharge for hard-wired motor", Price = 484m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 40, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 41, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { MotorId = 42, Description = "Surcharge for servo-assisted gear", Price = 75m, ProductId = 10, ArmTypeId = 2, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 43, Description = "Surcharge for servo-assisted gear", Price = 75m, ProductId = 10, ArmTypeId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 44, Description = "Surcharge for servo-assisted gear", Price = 0m, ProductId = 10, ArmTypeId = 3, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { MotorId = 45, Description = "Surcharge for hard-wired motor", Price = 484m, ProductId = 10, ArmTypeId = 2, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 46, Description = "Surcharge for hard-wired motor", Price = 574m, ProductId = 10, ArmTypeId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 47, Description = "Surcharge for hard-wired motor", Price = 574m, ProductId = 10, ArmTypeId = 3, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { MotorId = 48, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, ProductId = 10, ArmTypeId = 2, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 49, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, ProductId = 10, ArmTypeId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 50, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 809m, ProductId = 10, ArmTypeId = 3, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { MotorId = 51, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, ProductId = 10, ArmTypeId = 2, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 52, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, ProductId = 10, ArmTypeId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 53, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 691m, ProductId = 10, ArmTypeId = 3, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { MotorId = 54, Description = "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", Price = 1115m, ProductId = 10, ArmTypeId = 2, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 55, Description = "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", Price = 1115m, ProductId = 10, ArmTypeId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 56, Description = "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", Price = 0m, ProductId = 10, ArmTypeId = 3, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { MotorId = 57, Description = "Surcharge for radio-contr. motor io with manual override w/o transmitter", Price = 997m, ProductId = 10, ArmTypeId = 2, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 58, Description = "Surcharge for radio-contr. motor io with manual override w/o transmitter", Price = 997m, ProductId = 10, ArmTypeId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { MotorId = 59, Description = "Surcharge for radio-contr. motor io with manual override w/o transmitter", Price = 0m, ProductId = 10, ArmTypeId = 3, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { MotorId = 60, Description = "Surcharge for hard-wired motor", Price = 574m, ProductId = 11, ArmTypeId = 10, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 61, Description = "Surcharge for hard-wired motor", Price = 574m, ProductId = 11, ArmTypeId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 62, Description = "Surcharge for hard-wired motor", Price = 682m, ProductId = 11, ArmTypeId = 18, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 63, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 809m, ProductId = 11, ArmTypeId = 10, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 64, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 809m, ProductId = 11, ArmTypeId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 65, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 916m, ProductId = 11, ArmTypeId = 18, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 66, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 691m, ProductId = 11, ArmTypeId = 10, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 67, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 691m, ProductId = 11, ArmTypeId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 68, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 798m, ProductId = 11, ArmTypeId = 18, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 69, Description = "Surcharge for servo-assisted gear", Price = 75m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 70, Description = "Surcharge for servo-assisted gear", Price = 75m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 71, Description = "Surcharge for servo-assisted gear", Price = 75m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 72, Description = "Surcharge for servo-assisted gear", Price = 0m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 73, Description = "Surcharge for hard-wired motor", Price = 75m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 74, Description = "Surcharge for hard-wired motor", Price = 75m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 75, Description = "Surcharge for hard-wired motor", Price = 75m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 76, Description = "Surcharge for hard-wired motor", Price = 0m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 77, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 78, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 79, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 80, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 809m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 81, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, ProductId = 12, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 82, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, ProductId = 12, ArmTypeId = 4, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 83, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, ProductId = 12, ArmTypeId = 6, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 84, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 691m, ProductId = 12, ArmTypeId = 9, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 85, Description = "Surcharge for hard-wired motor", Price = 361m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 86, Description = "Surcharge for hard-wired motor", Price = 361m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 87, Description = "Surcharge for hard-wired motor", Price = 361m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 88, Description = "Surcharge for hard-wired motor", Price = 536m, ProductId = 13, ArmTypeId = 19, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 89, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 574m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 90, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 574m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 91, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 574m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 92, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 682m, ProductId = 13, ArmTypeId = 19, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 93, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 691m, ProductId = 13, ArmTypeId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 94, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 691m, ProductId = 13, ArmTypeId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 95, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 691m, ProductId = 13, ArmTypeId = 16, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 96, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 798m, ProductId = 13, ArmTypeId = 19, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 97, Description = "Surcharge for servo-assisted gear", Price = 75m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 98, Description = "Surcharge for servo-assisted gear", Price = 75m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 99, Description = "Surcharge for servo-assisted gear", Price = 0m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 100, Description = "Surcharge for hard-wired motor", Price = 484m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 101, Description = "Surcharge for hard-wired motor", Price = 574m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 102, Description = "Surcharge for hard-wired motor", Price = 574m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 103, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 104, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 105, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 809m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 106, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 107, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 108, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 691m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 109, Description = "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", Price = 721m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 110, Description = "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", Price = 721m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 111, Description = "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", Price = 809m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 112, Description = "Surcharge for radio-contr. motor io with manual override w/o transmitter", Price = 603m, ProductId = 14, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 113, Description = "Surcharge for radio-contr. motor io with manual override w/o transmitter", Price = 603m, ProductId = 14, ArmTypeId = 5, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 114, Description = "Surcharge for radio-contr. motor io with manual override w/o transmitter", Price = 691m, ProductId = 14, ArmTypeId = 8, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { MotorId = 115, Description = "Surcharge for hard-wired motor", Price = 484m, ProductId = 15, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 116, Description = "Surcharge for radio-contr. motor io + 1 ch. transmitter", Price = 721m, ProductId = 15, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { MotorId = 117, Description = "Surcharge for radio-contr. motor io w/o transmitter", Price = 603m, ProductId = 15, ArmTypeId = 2, DateCreated = staticCreatedDate, CreatedBy = "System" }
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
                 new { ValanceStyleId = 10, WidthCm = 700, Price = 177m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { ValanceStyleId = 11, WidthCm = 500, Price = 138m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ValanceStyleId = 12, WidthCm = 600, Price = 159m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ValanceStyleId = 13, WidthCm = 700, Price = 182m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ValanceStyleId = 14, WidthCm = 800, Price = 209m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ValanceStyleId = 15, WidthCm = 900, Price = 230m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ValanceStyleId = 16, WidthCm = 1000, Price = 254m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ValanceStyleId = 17, WidthCm = 1100, Price = 275m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ValanceStyleId = 18, WidthCm = 1200, Price = 302m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ValanceStyleId = 19, WidthCm = 1300, Price = 321m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ValanceStyleId = 20, WidthCm = 1390, Price = 347m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ValanceStyleId = 21, WidthCm = 250, Price = 79m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 22, WidthCm = 300, Price = 87m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 23, WidthCm = 350, Price = 96m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 24, WidthCm = 400, Price = 109m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 25, WidthCm = 450, Price = 122m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 26, WidthCm = 500, Price = 134m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 27, WidthCm = 250, Price = 79m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 28, WidthCm = 300, Price = 87m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 29, WidthCm = 350, Price = 96m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 30, WidthCm = 400, Price = 109m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 31, WidthCm = 450, Price = 122m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 32, WidthCm = 500, Price = 134m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 33, WidthCm = 550, Price = 145m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 34, WidthCm = 600, Price = 158m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 35, WidthCm = 650, Price = 166m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 36, WidthCm = 700, Price = 177m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ValanceStyleId = 37, WidthCm = 500, Price = 138m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 38, WidthCm = 600, Price = 159m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 39, WidthCm = 700, Price = 182m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 40, WidthCm = 800, Price = 209m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 41, WidthCm = 900, Price = 230m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 42, WidthCm = 1000, Price = 254m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 43, WidthCm = 1100, Price = 275m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 44, WidthCm = 1200, Price = 302m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 45, WidthCm = 1300, Price = 321m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 46, WidthCm = 1390, Price = 347m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { ValanceStyleId = 47, WidthCm = 250, Price = 79m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 48, WidthCm = 300, Price = 87m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 49, WidthCm = 350, Price = 96m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 50, WidthCm = 400, Price = 109m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 51, WidthCm = 450, Price = 122m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 52, WidthCm = 500, Price = 134m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 53, WidthCm = 550, Price = 145m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 54, WidthCm = 600, Price = 158m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 55, WidthCm = 650, Price = 166m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 56, WidthCm = 700, Price = 177m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { ValanceStyleId = 57, WidthCm = 500, Price = 138m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 58, WidthCm = 600, Price = 159m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 59, WidthCm = 700, Price = 182m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 60, WidthCm = 800, Price = 209m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 61, WidthCm = 900, Price = 230m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 62, WidthCm = 1000, Price = 254m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 63, WidthCm = 1100, Price = 275m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 64, WidthCm = 1200, Price = 302m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 65, WidthCm = 1300, Price = 321m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ValanceStyleId = 66, WidthCm = 1390, Price = 347m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" }


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
                new { RALColourId = 48, WidthCm = 700, Price = 640m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },

            //Markilux -  6000 Couple
                new { RALColourId = 49, WidthCm = 500, Price = 627m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { RALColourId = 50, WidthCm = 600, Price = 651m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { RALColourId = 51, WidthCm = 700, Price = 692m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { RALColourId = 52, WidthCm = 800, Price = 730m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { RALColourId = 53, WidthCm = 900, Price = 783m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { RALColourId = 54, WidthCm = 1000, Price = 880m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { RALColourId = 55, WidthCm = 1100, Price = 939m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { RALColourId = 56, WidthCm = 1200, Price = 1009m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { RALColourId = 57, WidthCm = 1300, Price = 1066m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { RALColourId = 58, WidthCm = 1390, Price = 1274m, MultiplyBy = 2.5m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },

                new { RALColourId = 59, WidthCm = 250, Price = 329m, MultiplyBy = 2.5m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 60, WidthCm = 300, Price = 352m, MultiplyBy = 2.5m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 61, WidthCm = 350, Price = 371m, MultiplyBy = 2.5m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 62, WidthCm = 400, Price = 388m, MultiplyBy = 2.5m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 63, WidthCm = 450, Price = 406m, MultiplyBy = 2.5m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 64, WidthCm = 500, Price = 423m, MultiplyBy = 2.5m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 65, WidthCm = 550, Price = 439m, MultiplyBy = 2.5m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 66, WidthCm = 600, Price = 458m, MultiplyBy = 2.5m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { RALColourId = 67, WidthCm = 250, Price = 329m, MultiplyBy = 2.5m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 68, WidthCm = 300, Price = 352m, MultiplyBy = 2.5m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 69, WidthCm = 350, Price = 371m, MultiplyBy = 2.5m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 70, WidthCm = 400, Price = 388m, MultiplyBy = 2.5m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 71, WidthCm = 450, Price = 406m, MultiplyBy = 2.5m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 72, WidthCm = 500, Price = 423m, MultiplyBy = 2.5m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { RALColourId = 73, WidthCm = 250, Price = 329m, MultiplyBy = 2.5m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 74, WidthCm = 300, Price = 352m, MultiplyBy = 2.5m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 75, WidthCm = 350, Price = 371m, MultiplyBy = 2.5m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 76, WidthCm = 400, Price = 388m, MultiplyBy = 2.5m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 77, WidthCm = 450, Price = 406m, MultiplyBy = 2.5m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 78, WidthCm = 500, Price = 423m, MultiplyBy = 2.5m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 79, WidthCm = 550, Price = 439m, MultiplyBy = 2.5m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 80, WidthCm = 600, Price = 458m, MultiplyBy = 2.5m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { RALColourId = 81, WidthCm = 250, Price = 316m, MultiplyBy = 2.5m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 82, WidthCm = 300, Price = 329m, MultiplyBy = 2.5m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 83, WidthCm = 350, Price = 352m, MultiplyBy = 2.5m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 84, WidthCm = 400, Price = 371m, MultiplyBy = 2.5m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 85, WidthCm = 450, Price = 396m, MultiplyBy = 2.5m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 86, WidthCm = 500, Price = 445m, MultiplyBy = 2.5m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 87, WidthCm = 550, Price = 477m, MultiplyBy = 2.5m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 88, WidthCm = 600, Price = 509m, MultiplyBy = 2.5m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 89, WidthCm = 650, Price = 539m, MultiplyBy = 2.5m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 90, WidthCm = 700, Price = 640m, MultiplyBy = 2.5m, ProductId = 10, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { RALColourId = 91, WidthCm = 500, Price = 627m, MultiplyBy = 2.5m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 92, WidthCm = 600, Price = 651m, MultiplyBy = 2.5m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 93, WidthCm = 700, Price = 692m, MultiplyBy = 2.5m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 94, WidthCm = 800, Price = 730m, MultiplyBy = 2.5m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 95, WidthCm = 900, Price = 783m, MultiplyBy = 2.5m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 96, WidthCm = 1000, Price = 880m, MultiplyBy = 2.5m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 97, WidthCm = 1100, Price = 939m, MultiplyBy = 2.5m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 98, WidthCm = 1200, Price = 1009m, MultiplyBy = 2.5m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 99, WidthCm = 1300, Price = 1066m, MultiplyBy = 2.5m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 100, WidthCm = 1390, Price = 1274m, MultiplyBy = 2.5m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { RALColourId = 101, WidthCm = 250, Price = 316m, MultiplyBy = 2.5m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 102, WidthCm = 300, Price = 329m, MultiplyBy = 2.5m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 103, WidthCm = 350, Price = 352m, MultiplyBy = 2.5m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 104, WidthCm = 400, Price = 371m, MultiplyBy = 2.5m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 105, WidthCm = 450, Price = 396m, MultiplyBy = 2.5m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 106, WidthCm = 500, Price = 445m, MultiplyBy = 2.5m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 107, WidthCm = 550, Price = 477m, MultiplyBy = 2.5m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 108, WidthCm = 600, Price = 509m, MultiplyBy = 2.5m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 109, WidthCm = 650, Price = 539m, MultiplyBy = 2.5m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 110, WidthCm = 700, Price = 640m, MultiplyBy = 2.5m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { RALColourId = 111, WidthCm = 500, Price = 627m, MultiplyBy = 2.5m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 112, WidthCm = 600, Price = 651m, MultiplyBy = 2.5m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 113, WidthCm = 700, Price = 692m, MultiplyBy = 2.5m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 114, WidthCm = 800, Price = 730m, MultiplyBy = 2.5m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 115, WidthCm = 900, Price = 783m, MultiplyBy = 2.5m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 116, WidthCm = 1000, Price = 880m, MultiplyBy = 2.5m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 117, WidthCm = 1100, Price = 939m, MultiplyBy = 2.5m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 118, WidthCm = 1200, Price = 1009m, MultiplyBy = 2.5m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 119, WidthCm = 1300, Price = 1066m, MultiplyBy = 2.5m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 120, WidthCm = 1390, Price = 1274m, MultiplyBy = 2.5m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 121, WidthCm = 250, Price = 316m, MultiplyBy = 2.5m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 122, WidthCm = 300, Price = 329m, MultiplyBy = 2.5m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 123, WidthCm = 350, Price = 352m, MultiplyBy = 2.5m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 124, WidthCm = 400, Price = 371m, MultiplyBy = 2.5m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 125, WidthCm = 450, Price = 396m, MultiplyBy = 2.5m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 126, WidthCm = 500, Price = 445m, MultiplyBy = 2.5m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 127, WidthCm = 550, Price = 477m, MultiplyBy = 2.5m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 128, WidthCm = 600, Price = 509m, MultiplyBy = 2.5m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 129, WidthCm = 650, Price = 539m, MultiplyBy = 2.5m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 130, WidthCm = 700, Price = 640m, MultiplyBy = 2.5m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { RALColourId = 131, WidthCm = 250, Price = 312m, MultiplyBy = 2.5m, ProductId = 15, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 132, WidthCm = 300, Price = 335m, MultiplyBy = 2.5m, ProductId = 15, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 133, WidthCm = 350, Price = 353m, MultiplyBy = 2.5m, ProductId = 15, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { RALColourId = 134, WidthCm = 400, Price = 369m, MultiplyBy = 2.5m, ProductId = 15, DateCreated = staticCreatedDate, CreatedBy = "System" }
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
                 new { WallSealingProfileId = 10, WidthCm = 700, Price = 223m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                new { WallSealingProfileId = 11, WidthCm = 500, Price = 170m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { WallSealingProfileId = 12, WidthCm = 600, Price = 197m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { WallSealingProfileId = 13, WidthCm = 700, Price = 223m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { WallSealingProfileId = 14, WidthCm = 800, Price = 253m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { WallSealingProfileId = 15, WidthCm = 900, Price = 281m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { WallSealingProfileId = 16, WidthCm = 1000, Price = 313m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { WallSealingProfileId = 17, WidthCm = 1100, Price = 347m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { WallSealingProfileId = 18, WidthCm = 1200, Price = 378m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { WallSealingProfileId = 19, WidthCm = 1300, Price = 406m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { WallSealingProfileId = 20, WidthCm = 1390, Price = 435m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },

                new { WallSealingProfileId = 21, WidthCm = 250, Price = 90m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 22, WidthCm = 300, Price = 104m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 23, WidthCm = 350, Price = 114m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 24, WidthCm = 400, Price = 130m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 25, WidthCm = 450, Price = 146m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 26, WidthCm = 500, Price = 162m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 27, WidthCm = 550, Price = 177m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 28, WidthCm = 600, Price = 193m, ProductId = 7, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { WallSealingProfileId = 29, WidthCm = 250, Price = 90m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 30, WidthCm = 300, Price = 104m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 31, WidthCm = 350, Price = 114m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 32, WidthCm = 400, Price = 130m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 33, WidthCm = 450, Price = 146m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 34, WidthCm = 500, Price = 162m, ProductId = 8, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { WallSealingProfileId = 35, WidthCm = 250, Price = 90m, ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 36, WidthCm = 300, Price = 104m, ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 37, WidthCm = 350, Price = 114m, ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 38, WidthCm = 400, Price = 130m, ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 39, WidthCm = 450, Price = 146m, ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 40, WidthCm = 500, Price = 162m, ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 41, WidthCm = 550, Price = 177m, ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 42, WidthCm = 600, Price = 193m, ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 43, WidthCm = 650, Price = 207m, ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 44, WidthCm = 700, Price = 223m, ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { WallSealingProfileId = 45, WidthCm = 500, Price = 170m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 46, WidthCm = 600, Price = 197m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 47, WidthCm = 700, Price = 223m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 48, WidthCm = 800, Price = 253m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 49, WidthCm = 900, Price = 281m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 50, WidthCm = 1000, Price = 313m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 51, WidthCm = 1100, Price = 347m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 52, WidthCm = 1200, Price = 378m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 53, WidthCm = 1300, Price = 406m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 54, WidthCm = 1390, Price = 435m, ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { WallSealingProfileId = 55, WidthCm = 250, Price = 65m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 56, WidthCm = 300, Price = 74m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 57, WidthCm = 350, Price = 79m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 58, WidthCm = 400, Price = 86m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 59, WidthCm = 450, Price = 93m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 60, WidthCm = 500, Price = 103m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 61, WidthCm = 550, Price = 109m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 62, WidthCm = 600, Price = 116m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 63, WidthCm = 650, Price = 123m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 64, WidthCm = 700, Price = 134m, ProductId = 12, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { WallSealingProfileId = 65, WidthCm = 500, Price = 121m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 66, WidthCm = 600, Price = 134m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 67, WidthCm = 700, Price = 144m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 68, WidthCm = 800, Price = 158m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 69, WidthCm = 900, Price = 173m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 70, WidthCm = 1000, Price = 193m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 71, WidthCm = 1100, Price = 206m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 72, WidthCm = 1200, Price = 225m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 73, WidthCm = 1300, Price = 236m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 74, WidthCm = 1390, Price = 258m, ProductId = 13, DateCreated = staticCreatedDate, CreatedBy = "System" },

                new { WallSealingProfileId = 75, WidthCm = 250, Price = 90m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 76, WidthCm = 300, Price = 104m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 77, WidthCm = 350, Price = 114m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 78, WidthCm = 400, Price = 130m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 79, WidthCm = 450, Price = 146m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 80, WidthCm = 500, Price = 162m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 81, WidthCm = 550, Price = 177m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 82, WidthCm = 600, Price = 193m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 83, WidthCm = 650, Price = 207m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { WallSealingProfileId = 84, WidthCm = 700, Price = 223m, ProductId = 14, DateCreated = staticCreatedDate, CreatedBy = "System" }

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
                new { ShadePlusId = 40, WidthCm = 700, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 2395m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 5 },
                // 6000 couple
                new { ShadePlusId = 41, WidthCm = 500, Description = "Surcharge for height 210 cm with gearbox", Price = 1424m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 42, WidthCm = 600, Description = "Surcharge for height 210 cm with gearbox", Price = 1581m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 43, WidthCm = 700, Description = "Surcharge for height 210 cm with gearbox", Price = 1726m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 44, WidthCm = 800, Description = "Surcharge for height 210 cm with gearbox", Price = 1878m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 45, WidthCm = 900, Description = "Surcharge for height 210 cm with gearbox", Price = 2027m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 46, WidthCm = 1000, Description = "Surcharge for height 210 cm with gearbox", Price = 2179m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 47, WidthCm = 1100, Description = "Surcharge for height 210 cm with gearbox", Price = 2329m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 48, WidthCm = 1200, Description = "Surcharge for height 210 cm with gearbox", Price = 2479m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 49, WidthCm = 1300, Description = "Surcharge for height 210 cm with gearbox", Price = 2628m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 50, WidthCm = 1390, Description = "Surcharge for height 210 cm with gearbox", Price = 2779m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 51, WidthCm = 500, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 3060m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 52, WidthCm = 600, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 3183m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 53, WidthCm = 700, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 3374m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 54, WidthCm = 800, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 3546m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 55, WidthCm = 900, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 3649m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 56, WidthCm = 1000, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 3787m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 57, WidthCm = 1100, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 3959m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 58, WidthCm = 1200, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 4128m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 59, WidthCm = 1300, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 4318m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 60, WidthCm = 1390, Description = "Surcharge for height 210 cm with hard-wired motor", Price = 4511m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },

                new { ShadePlusId = 61, WidthCm = 500, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 3325m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 62, WidthCm = 600, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 3449m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 63, WidthCm = 700, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 3638m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 64, WidthCm = 800, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 3809m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 65, WidthCm = 900, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 3913m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 66, WidthCm = 1000, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 4053m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 67, WidthCm = 1100, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 4225m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 68, WidthCm = 1200, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 4392m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 69, WidthCm = 1300, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 4585m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },
                new { ShadePlusId = 70, WidthCm = 1390, Description = "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", Price = 4776m, DateCreated = staticCreatedDate, CreatedBy = "System", ProductId = 6 },

                new { ShadePlusId = 71, WidthCm = 250, Description = "Surcharge for height 170 cm with gearbox", Price = 718m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 72, WidthCm = 300, Description = "Surcharge for height 170 cm with gearbox", Price = 797m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 73, WidthCm = 350, Description = "Surcharge for height 170 cm with gearbox", Price = 865m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 74, WidthCm = 400, Description = "Surcharge for height 170 cm with gearbox", Price = 944m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 75, WidthCm = 450, Description = "Surcharge for height 170 cm with gearbox", Price = 1018m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 76, WidthCm = 500, Description = "Surcharge for height 170 cm with gearbox", Price = 1095m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 77, WidthCm = 550, Description = "Surcharge for height 170 cm with gearbox", Price = 1170m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 78, WidthCm = 600, Description = "Surcharge for height 170 cm with gearbox", Price = 1247m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { ShadePlusId = 79, WidthCm = 250, Description = "Surcharge for height 170 cm with hard-wired motor", Price = 1538m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 80, WidthCm = 300, Description = "Surcharge for height 170 cm with hard-wired motor", Price = 1596m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 81, WidthCm = 350, Description = "Surcharge for height 170 cm with hard-wired motor", Price = 1693m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 82, WidthCm = 400, Description = "Surcharge for height 170 cm with hard-wired motor", Price = 1777m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 83, WidthCm = 450, Description = "Surcharge for height 170 cm with hard-wired motor", Price = 1829m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 84, WidthCm = 500, Description = "Surcharge for height 170 cm with hard-wired motor", Price = 1898m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 85, WidthCm = 550, Description = "Surcharge for height 170 cm with hard-wired motor", Price = 1986m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 86, WidthCm = 600, Description = "Surcharge for height 170 cm with hard-wired motor", Price = 2069m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { ShadePlusId = 87, WidthCm = 250, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", Price = 1667m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 88, WidthCm = 300, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", Price = 1729m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 89, WidthCm = 350, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", Price = 1827m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 90, WidthCm = 400, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", Price = 1911m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 91, WidthCm = 450, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", Price = 1960m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 92, WidthCm = 500, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", Price = 2032m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 93, WidthCm = 550, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", Price = 2119m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 94, WidthCm = 600, Description = "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", Price = 2204m, ProductId = 9, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System" },

                new { ShadePlusId = 95, WidthCm = 250, Price = 719m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 96, WidthCm = 300, Price = 798m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 97, WidthCm = 350, Price = 865m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 98, WidthCm = 400, Price = 945m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 99, WidthCm = 450, Price = 1019m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 100, WidthCm = 500, Price = 1096m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 101, WidthCm = 550, Price = 1173m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 102, WidthCm = 600, Price = 1248m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 103, WidthCm = 650, Price = 1319m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 104, WidthCm = 700, Price = 1396m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 10, DateCreated =new DateTime(2026, 4, 16), CreatedBy = "System" },
                new { ShadePlusId = 105, WidthCm = 500, Price = 1424m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ShadePlusId = 106, WidthCm = 600, Price = 1581m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ShadePlusId = 107, WidthCm = 700, Price = 1726m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ShadePlusId = 108, WidthCm = 800, Price = 1878m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ShadePlusId = 109, WidthCm = 900, Price = 2027m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ShadePlusId = 110, WidthCm = 1000, Price = 2179m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ShadePlusId = 111, WidthCm = 1100, Price = 2329m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ShadePlusId = 112, WidthCm = 1200, Price = 2479m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ShadePlusId = 113, WidthCm = 1300, Price = 2628m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new { ShadePlusId = 114, WidthCm = 1390, Price = 2779m, Description = "Surcharge for height 210 cm with gearbox", ProductId = 11, DateCreated = staticCreatedDate, CreatedBy = "System" }
                );

            //LightingCassette Data
            modelBuilder.Entity<LightingCassette>().HasData(
                //new LightingCassette { LightingId = 1, Description = "Surcharge for LED Line RGB-WW Radio-controlled io - dimmable (without remote control)", Price = 1555.00m, DateCreated = new DateTime(2026, 4, 6), CreatedBy = "System", ProductId = 5 },
                //new LightingCassette { LightingId = 2, Description = "Surcharge for LED Line RGB-WW Zigbee radio control - dimmable (without transmitter)", Price = 1386.00m, DateCreated = new DateTime(2026, 4, 6), CreatedBy = "System", ProductId = 5 },
                new LightingCassette { LightingId = 1, Description = "Surcharge for LED Line RGB-WW Radio-controlled io - dimmable (without remote control)", Price = 1555.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 4 },
                new LightingCassette { LightingId = 2, Description = "Surcharge for LED Line RGB-WW Zigbee radio control - dimmable (without transmitter)", Price = 1386.00m, DateCreated = new DateTime(2026, 4, 7), CreatedBy = "System", ProductId = 4 },
                new { LightingId = 3,Description = "Surcharge for LED Line RGB-WW Radio-controlled io - dimmable (without remote control)",  Price = 1555m,  DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7 },
                new { LightingId = 4, Description = "Surcharge for LED Line RGB-WW Zigbee radio control - dimmable (without transmitter)", Price = 1386m, DateCreated = new DateTime(2026, 4, 16), CreatedBy = "System", ProductId = 7 }
            );
        }
    }
}