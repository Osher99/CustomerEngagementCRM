using Microsoft.EntityFrameworkCore;
using CustomerEngagementDashboardApp.DbModel;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CustomerEngagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CustomerInteraction> CustomerInteractions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerInteraction>()
                .Property(ci => ci.InteractionType)
                .HasConversion<string>();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                            v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                        ));
                    }
                }
            }
        }
    }
}