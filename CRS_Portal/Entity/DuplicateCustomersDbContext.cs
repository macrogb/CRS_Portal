using Microsoft.EntityFrameworkCore;
using SCV_Portal.HelperMethods;
using SCV_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCV_Portal.Entity
{
    public class DuplicateCustomersDbContext : DbContext
    {
        public DbSet<NotDuplicateCustomer> DbNotDuplicateCustomers { get; set; }

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
            modelBuilder.Entity<NotDuplicateCustomer>()
                    .ToTable("NOTDUPCUSTS");
            modelBuilder.Entity<NotDuplicateCustomer>(entity =>
            {
                entity.HasKey(pk => pk.ID);
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.CustNo).HasColumnName("CustNo");
                entity.Property(e => e.NDUPCustNo).HasColumnName("NDUPCustNo");
                entity.Property(e => e.IsActive).HasColumnName("IsActive");
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
                entity.Property(e => e.CreatedTime).HasColumnName("CreatedTime");
                entity.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
                entity.Property(e => e.ModifiedTime).HasColumnName("ModifiedTime");
            });
        }
    }
}
