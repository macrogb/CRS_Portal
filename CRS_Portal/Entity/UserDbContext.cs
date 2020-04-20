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
    public class UserDbContext : DbContext
    {
        public DbSet<UserDetails> DbUserModel { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string CS = SCVMacroSettings.Initial_CS;
                optionsBuilder.UseSqlServer(CS);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MyAppModel(modelBuilder);
        }

        protected void MyAppModel(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserDetails>()
                .Ignore(i => i.ConfirmPassword)
                .Ignore(i => i.UserType)
            .ToTable("UserDetails");

            modelBuilder.Entity<UserDetails>(entity =>
            {
                entity.HasKey(pk => pk.UserID);
                entity.Property(e => e.UserID).HasColumnName("UserID");
                entity.Property(e => e.FirstName).HasColumnName("FirstName");
                entity.Property(e => e.LastName).HasColumnName("LastName");
                entity.Property(e => e.UserName).HasColumnName("UserName");
                entity.Property(e => e.Password).HasColumnName("Password");
                entity.Property(e => e.UserTypeDesc).HasColumnName("UserType");

                entity.Property(e => e.BankName).HasColumnName("BankName");
                entity.Property(e => e.BankSortName).HasColumnName("BankSortName");
                entity.Property(e => e.BankFRNNo).HasColumnName("BankFRNNumber");

                entity.Property(e => e.IsActive).HasColumnName("IsActive");
                entity.Property(e => e.LastLoginDate).HasColumnName("LastLoginDate");
                entity.Property(e => e.LastLoginTime).HasColumnName("LastLoginTime");

                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
                entity.Property(e => e.CreatedTime).HasColumnName("CreatedTime");
                entity.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
                entity.Property(e => e.ModifiedTime).HasColumnName("ModifiedTime");
            });
        }
    }
}
