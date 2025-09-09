using Microsoft.EntityFrameworkCore;
using TheNextEventAPI.Models;

namespace TheNextEventAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<WebsiteContent> WebsiteContents { get; set; }
        public DbSet<FormSubmission> FormSubmissions { get; set; }
        public DbSet<EmailConfiguration> EmailConfigurations { get; set; }
        public DbSet<SeoMetadata> SeoMetadata { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique constraints
            modelBuilder.Entity<AdminUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<WebsiteContent>()
                .HasIndex(w => w.ContentKey)
                .IsUnique()
                .HasDatabaseName("UK_WebsiteContent_ContentKey");

            modelBuilder.Entity<SeoMetadata>()
                .HasIndex(s => s.PageUrl)
                .IsUnique();

            // Configure default values
            modelBuilder.Entity<AdminUser>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<AdminUser>()
                .Property(u => u.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<WebsiteContent>()
                .Property(w => w.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<WebsiteContent>()
                .Property(w => w.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<WebsiteContent>()
                .Property(w => w.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.SubmittedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.Status)
                .HasDefaultValue("New");

            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.IsRead)
                .HasDefaultValue(false);

            modelBuilder.Entity<EmailConfiguration>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<EmailConfiguration>()
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<EmailConfiguration>()
                .Property(e => e.IsEnabled)
                .HasDefaultValue(true);

            modelBuilder.Entity<EmailConfiguration>()
                .Property(e => e.UseSSL)
                .HasDefaultValue(true);

            modelBuilder.Entity<SeoMetadata>()
                .Property(s => s.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<SeoMetadata>()
                .Property(s => s.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<SeoMetadata>()
                .Property(s => s.IsActive)
                .HasDefaultValue(true);
        }
    }
}
