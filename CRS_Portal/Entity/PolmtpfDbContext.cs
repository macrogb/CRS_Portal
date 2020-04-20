using Microsoft.EntityFrameworkCore;
using SCV_Portal.HelperMethods;
using SCV_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCV_Portal.Entity
{
    public class PolmtpfDbContext : DbContext
    {
        public DbSet<POLMTPF> DbPOLMTPFModel { get; set; }


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
            modelBuilder.Entity<POLMTPF>().Ignore(i=>i.myonoffswitch)
                .ToTable("POLMTPF");

            modelBuilder.Entity<POLMTPF>(entity =>
            {
                entity.HasKey(pk => pk.ID);
                entity.Property(e => e.PMODULE).HasColumnName("PMODULE");
                entity.Property(e => e.PTYPE).HasColumnName("PTYPE");
                entity.Property(e => e.PDESC).HasColumnName("PDESC");
                entity.Property(e => e.PEDTYP).HasColumnName("PEDTYP");
            });
        }
    }
}
