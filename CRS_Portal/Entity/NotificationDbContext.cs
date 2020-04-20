using Microsoft.EntityFrameworkCore;
using CRS_Portal.HelperMethods;
using CRS_Portal.Models;

namespace CRS_Portal.Entity
{
    public class NotificationDbContext : DbContext
    {
        public DbSet<Notification> DbNotificationModel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Helper.LoadDatabaseByBankName());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MyAppModel(modelBuilder);
        }

        protected void MyAppModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>()
                 .Ignore(i => i.Category)
                 .ToTable("SCV_Notification");

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(pk => pk.ID);
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.CategoryDesc).HasColumnName("Category");
                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
                entity.Property(e => e.CreatedTime).HasColumnName("CreatedTime");
                entity.Property(e => e.DestinationBankFRN).HasColumnName("DestinationBankFRN");
                entity.Property(e => e.DestinationBankname).HasColumnName("DestinationBankname");
                entity.Property(e => e.Message).HasColumnName("Message");
                entity.Property(e => e.IsActive).HasColumnName("IsActive");
                entity.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
                entity.Property(e => e.ModifiedTime).HasColumnName("ModifiedTime");
                entity.Property(e => e.Source).HasColumnName("Source");
                entity.Property(e => e.UserSeen).HasColumnName("UserSeen");
            });
        }
    }
}
