using AwiningsIreland_WebAPI.Models;
using AwningsAPI.Model.Auth;
using AwningsAPI.Model.Customers;
using AwningsAPI.Model.Products;
using AwningsAPI.Model.Suppliers;
using AwningsAPI.Model.Workflow;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Arms>  Arms { get; set; }
        public DbSet<Motors> Motors { get; set; }
        public DbSet<ValanceStyle> valanceStyles { get; set; }
        public DbSet<NonStandardRALColours> nonStandardRALColours { get; set; }
        public DbSet<WallSealingProfile> wallSealingProfiles { get; set; }
        public DbSet<Heaters> Heaters { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<QuoteItem> QuoteItems { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-many relationship
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.CustomerContacts)
                .WithOne(cc => cc.Customer)
                .HasForeignKey(cc => cc.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: cascade delete

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


            base.OnModelCreating(modelBuilder);

            var staticCreatedDate = new DateTime(2023, 01, 01, 12, 00, 00);
            var staticUpdatedDate = new DateTime(2023, 01, 02, 12, 00, 00);

            // Seed Customer
            modelBuilder.Entity<Customer>().HasData(
                new Customer{CustomerId = 1, Name = "Acme Corporation",CompanyNumber = "ACME123", Residential = false, RegistrationNumber = "REG456789", VATNumber = "VAT987654",Address1 = "123 Main Street",Address2 = "Suite 100",Address3 = null,County = "Dublin",CountryId = 1,Phone = "+35312345678",Fax = null,Mobile = "+35387654321",Email = "info@acme.ie",TaxNumber = "TAX123456",Eircode = "D01XY12",DateCreated = staticCreatedDate,CreatedBy = "System"
            });
            // Seed CustomerContact
            modelBuilder.Entity<CustomerContact>().HasData(
                new CustomerContact{ContactId = 1,FirstName = "John",LastName = "Doe",DateOfBirth = new DateTime(1985, 5, 20),Mobile = "+35387654322",Phone = "+35312345679",Email = "john.doe@acme.ie",DateCreated = staticCreatedDate,CreatedBy = "System",
                    CustomerId = 1 // Foreign key to Customer
            });
            // Seed ProductTypes
            modelBuilder.Entity<ProductType>().HasData(
                new ProductType { ProductTypeId = 1, SupplierId=1, Description = "Folding-arm Cassette Awnings", DateCreated = staticCreatedDate, CreatedBy = "System" },
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
                new Product { ProductId = 11, Description = "Markilux 900", ProductTypeId = 1, SupplierId = 1, DateCreated = staticCreatedDate, CreatedBy = "System" }
            );
            //Workflow Start
            modelBuilder.Entity<WorkflowStart>().HasData(
                new WorkflowStart { WorkflowId = 1, CustomerId=1, Description = "Markilux 990 for outside garden", DateCreated = staticCreatedDate, CreatedBy = "System", SupplierId=1, ProductTypeId=1, ProductId=6 }
            );
            //Projection Data
            modelBuilder.Entity<Projections>().HasData(
                new Projections { ProjectionId = 1, ProductId = 6, Width_cm = 250, Projection_cm = 150, Price = 1873, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 2, ProductId = 6, Width_cm = 300, Projection_cm = 150, Price = 2023, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 3, ProductId = 6, Width_cm = 350, Projection_cm = 150, Price = 2229, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 4, ProductId = 6, Width_cm = 400, Projection_cm = 150, Price = 2397, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 5, ProductId = 6, Width_cm = 450, Projection_cm = 150, Price = 2554, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 6, ProductId = 6, Width_cm = 500, Projection_cm = 150, Price = 2730, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 7, ProductId = 6, Width_cm = 250, Projection_cm = 200, Price = 1979, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 8, ProductId = 6, Width_cm = 300, Projection_cm = 200, Price = 2145, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 9, ProductId = 6, Width_cm = 350, Projection_cm = 200, Price = 2319, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 10, ProductId = 6, Width_cm = 400, Projection_cm = 200, Price = 2508, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 11, ProductId = 6, Width_cm = 450, Projection_cm = 200, Price = 2664, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 12, ProductId = 6, Width_cm = 500, Projection_cm = 200, Price = 2842, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 13, ProductId = 6, Width_cm = 300, Projection_cm = 250, Price = 2248, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 14, ProductId = 6, Width_cm = 350, Projection_cm = 250, Price = 2431, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 15, ProductId = 6, Width_cm = 400, Projection_cm = 250, Price = 2627, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 16, ProductId = 6, Width_cm = 450, Projection_cm = 250, Price = 2798, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 17, ProductId = 6, Width_cm = 500, Projection_cm = 250, Price = 2970, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 18, ProductId = 6, Width_cm = 350, Projection_cm = 300, Price = 2547, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 19, ProductId = 6, Width_cm = 400, Projection_cm = 300, Price = 2750, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 20, ProductId = 6, Width_cm = 450, Projection_cm = 300, Price = 2904, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Projections { ProjectionId = 21, ProductId = 6, Width_cm = 500, Projection_cm = 300, Price = 3084, DateCreated = staticCreatedDate, CreatedBy = "System" }
            );
            //Bracket Data
            modelBuilder.Entity<Brackets>().HasData(
                new Brackets { BracketId = 1, ProductId = 6, BracketName = "Face fixture bracket 150 mm / 3", PartNumber = "71624",  Price = 42.70m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 2, ProductId = 6, BracketName = "Face fixture bracket 300 mm left / 4", PartNumber = "70617", Price = 73.50m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 3, ProductId = 6, BracketName = "Face fixture bracket 300 mm right / 4", PartNumber = "70600", Price = 73.50m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 4, ProductId = 6, BracketName = "Stand-off bkt. 80-300 mm for face fixture for face fixture bracket 300 mm / 4", PartNumber = "77968", Price = 220.50m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 5, ProductId = 6, BracketName = "Top fixture bracket 150 mm / 4", PartNumber = "71625", Price = 42.70m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 6, ProductId = 6, BracketName = "Eaves fixture bracket 150mm, complete / 4", PartNumber = "71669", Price = 99.30m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 7, ProductId = 6, BracketName = "Eaves fixture bracket 270 mm /4", PartNumber = "71659", Price = 77.00m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 8, ProductId = 6, BracketName = "Angle and plate for eaves fixture (machine finish) / 4", PartNumber = "716620", Price = 125.20m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 9, ProductId = 6, BracketName = "Additional eaves fixture plate 60x260x12 mm / 2", PartNumber = "75383", Price = 42.60m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 10, ProductId = 6, BracketName = "Spreader plate A 430x160x12 mm / 8", PartNumber = "75326", Price = 124.10m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 11, ProductId = 6, BracketName = "Spreader plate B 300x400x12 mm / 4", PartNumber = "75325", Price = 160.20m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 12, ProductId = 6, BracketName = "Spacer block face or top fixt 136x150x20 mm / 3", PartNumber = "716331", Price = 5.50m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 13, ProductId = 6, BracketName = "Spacer block face or top fixt 136x150x12 mm / 3", PartNumber = "71644", Price = 3.60m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 14, ProductId = 6, BracketName = "Cover plate 230x210x2 mm", PartNumber = "71843", Price = 16.50m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 15, ProductId = 6, BracketName = "Cover plate 290x210x2 mm", PartNumber = "71841", Price = 20.50m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Brackets { BracketId = 16, ProductId = 6, BracketName = "Vertical fixture rail incl. fixing material 624291", PartNumber = "62421", Price = 174.90m, DateCreated = staticCreatedDate, CreatedBy = "System" }
            );
            //BF Data
            modelBuilder.Entity<BF>().HasData(
                new BF { BFId = 1, Description = "BF 6", Price = 312.00m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new BF { BFId = 2, Description = "BF 8", Price = 312.00m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new BF { BFId = 3, Description = "BF 16", Price = 312.00m, DateCreated = staticCreatedDate, CreatedBy = "System" }
            );
            //Arms Data
            modelBuilder.Entity<Arms>().HasData(
                new Arms { ArmId = 1, ProductId = 6, BfId=1, Description  = "Surcharge for face fixture", Price = 86m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Arms { ArmId = 2, ProductId = 6, BfId=3, Description = "Surcharge for face fixture incl. spreader plate A", Price = 334m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Arms { ArmId = 3, ProductId = 6, BfId=2, Description = "Surcharge for face fixture incl. spreader plate B", Price = 406m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Arms { ArmId = 4, ProductId = 6, BfId=2, Description = "Surcharge for top fixture", Price = 86m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Arms { ArmId = 5, ProductId = 6, BfId=2, Description = "Surcharge for eaves fixture", Price = 199m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Arms { ArmId = 6, ProductId = 6, Description = "Surcharge for arms with bionic tendon", Price = 117m, DateCreated = staticCreatedDate, CreatedBy = "System" },
                new Arms { ArmId = 7, ProductId = 6, Description = "Surcharge for bespoke arms", Price = 177m, DateCreated = staticCreatedDate, CreatedBy = "System" }
            );
            //Motors Data
            modelBuilder.Entity<Motors>().HasData(
                new Motors { MotorId = 1, ProductId=6, Description= "Surcharge for servo-assisted gear", Price=72m, DateCreated = staticCreatedDate, CreatedBy = "System" },
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
                new Heaters { HeaterId = 1, ProductId = 6, Description = "Markilux Infrared Heater 2500 watt Dimmable",Price = 1393m, PriceNonRALColour = 1635m, DateCreated = staticCreatedDate, CreatedBy = "System" }
             ); 
        }
    }
}
