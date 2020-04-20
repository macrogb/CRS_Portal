using Microsoft.EntityFrameworkCore;
using SCV_Portal.HelperMethods;
using SCV_Portal.Models;


namespace SCV_Portal.Entity
{
    public class SCVAuditLogDbContext : DbContext
    {
        public DbSet<SCV_AuditLog> DbSCV_AuditLogModel { get; set; }


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
            modelBuilder.Entity<SCV_AuditLog>()
                .ToTable("SCV_AuditLog");
            modelBuilder.Entity<SCV_AuditLog>()
                .Property(p => p.CreatedDate).HasDefaultValueSql("REPLACE(CONVERT(varchar(10), GetDate(),126),'-','')");
            modelBuilder.Entity<SCV_AuditLog>()
                .Property(p => p.CreatedTime).HasDefaultValueSql("REPLACE(convert(varchar(10), GetDate(), 108),':','')");
                //.Ignore(r=>r.CreatedDate)
                //.Ignore(r => r.CreatedTime)
           

            modelBuilder.Entity<SCV_AuditLog>(entity =>
            {
                entity.HasKey(pk => pk.ID);
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.LogType).HasColumnName("LogType");
                entity.Property(e => e.LogMessage).HasColumnName("LogMessage");
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
                entity.Property(e => e.CreatedTime).HasColumnName("CreatedTime");
            });
        }
    }
}
