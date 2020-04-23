using CRS_Portal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.Entity
{
    public class CurrencyDBContext : DbContext
    {
        string connstr = "Data Source=WINSQL2012\\INDSQL2014;Initial Catalog=CRSCLOUD;uid=sa;pwd=01sql14*;";
        public DbSet<Currency> DbCurrency { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connstr);
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            LoadFile(modelBuilder);
        }

        protected void LoadFile(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>()
                .ToTable("Currency");

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.C8CCY).HasColumnName("C8CCY");
                entity.Property(e => e.C8CUR).HasColumnName("C8CUR");
                entity.Property(e => e.C8CED).HasColumnName("C8CED");
                entity.Property(e => e.C8SPT).HasColumnName("C8SPT");
                entity.Property(e => e.C8SEI).HasColumnName("C8SEI");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(pk => pk.C8CCY);
            });

        }
    }
}
