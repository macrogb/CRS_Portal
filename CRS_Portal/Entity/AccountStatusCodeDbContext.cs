using Microsoft.EntityFrameworkCore;
using SCV_Portal.HelperMethods;
using SCV_Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SCV_Portal.Entity
{
    public class AccountStatusCodeDbContext : DbContext
    {
        public DbSet<AccountStatusCode> DbAccountStatusCodeModel { get; set; }


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

            modelBuilder.Entity<AccountStatusCode>().Ignore(i => i.CategoryType).Ignore(i => i.SubCategoryType)
            .ToTable("AccountStatusCode");

            modelBuilder.Entity<AccountStatusCode>(entity =>
            {
                entity.HasKey(pk => pk.ID);
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.Category).HasColumnName("Category");
                entity.Property(e => e.SubCategory).HasColumnName("SubCategory");
                entity.Property(e => e.Description).HasColumnName("Description");
                entity.Property(e => e.StatusCode).HasColumnName("StatusCode");
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
