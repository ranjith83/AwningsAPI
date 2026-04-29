using AwningsEmailFunction.Models;
using Microsoft.EntityFrameworkCore;

namespace AwningsEmailFunction.Database;

public class EmailFunctionDbContext : DbContext
{
    public EmailFunctionDbContext(DbContextOptions<EmailFunctionDbContext> options) : base(options) { }

    public DbSet<IncomingEmail> IncomingEmails { get; set; }
    public DbSet<EmailAttachment> EmailAttachments { get; set; }
    public DbSet<AppTask> Tasks { get; set; }
    public DbSet<TaskAttachment> TaskAttachments { get; set; }
    public DbSet<TaskHistory> TaskHistories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerContact> CustomerContacts { get; set; }
    public DbSet<WorkflowStart> WorkflowStarts { get; set; }
    public DbSet<InitialEnquiry> InitialEnquiries { get; set; }
    public DbSet<GraphSubscription> GraphSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailAttachment>()
            .HasOne(a => a.IncomingEmail)
            .WithMany(e => e.Attachments)
            .HasForeignKey(a => a.IncomingEmailId);

        modelBuilder.Entity<TaskAttachment>()
            .HasOne(a => a.Task)
            .WithMany(t => t.TaskAttachments)
            .HasForeignKey(a => a.TaskId);

        modelBuilder.Entity<TaskHistory>()
            .HasOne(h => h.Task)
            .WithMany(t => t.TaskHistories)
            .HasForeignKey(h => h.TaskId);

        modelBuilder.Entity<CustomerContact>()
            .HasOne(c => c.Customer)
            .WithMany(cu => cu.CustomerContacts)
            .HasForeignKey(c => c.CustomerId);
    }
}
