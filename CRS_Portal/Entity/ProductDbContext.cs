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
    public class ProductDbContext : DbContext
    {
        public DbSet<ProductDetails> DbProductModel { get; set; }


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
            
                modelBuilder.Entity<ProductDetails>().Ignore(i => i.ProductType).Ignore(i => i.ProductNameWithOutSpace)
                .ToTable("PRODUCTDET");

            modelBuilder.Entity<ProductDetails>(entity =>
            {
                entity.HasKey(pk => pk.ID);
                //entity.Property(e => e.ID).ValueGeneratedOnAdd().HasColumnName("ID");
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.ProductTypeDesc).HasColumnName("PRODTYPE");
                entity.Property(e => e.ProductName).HasColumnName("PRODNAME");
                entity.Property(e => e.ProductDescription).HasColumnName("PRODDESC");
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
