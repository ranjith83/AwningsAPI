using AwningsEmailFunction.Models;
using Microsoft.EntityFrameworkCore;

namespace AwningsEmailFunction.Database;

public class EmailFunctionDbContext : DbContext
{
    public EmailFunctionDbContext(DbContextOptions<EmailFunctionDbContext> options) : base(options) { }

    public DbSet<IncomingEmail> IncomingEmails { get; set; }
    public DbSet<EmailAttachment> EmailAttachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailAttachment>()
            .HasOne(a => a.IncomingEmail)
            .WithMany(e => e.Attachments)
            .HasForeignKey(a => a.IncomingEmailId);
    }
}
