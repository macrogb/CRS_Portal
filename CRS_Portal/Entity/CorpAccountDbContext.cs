﻿using Microsoft.EntityFrameworkCore;
using CRS_Portal.HelperMethods;
using CRS_Portal.Models;

namespace CRS_Portal.Entity
{
    public class CorpAccountDbContext : DbContext
    {
        public DbSet<CorpAccounts> DbCorpAccountModel { get; set; }


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

            modelBuilder.Entity<CorpAccounts>()
            .ToTable("CORPACCOUNTDTL");

            modelBuilder.Entity<CorpAccounts>(entity =>
            {
                entity.HasKey(pk => pk.ID);
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.AccountNumber).HasColumnName("CORPACCNO");
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
