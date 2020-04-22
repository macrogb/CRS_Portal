using CRS_Portal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRS_Portal.Entity
{
    public class CountryDbContext : DbContext
    {
        string connstr = "Data Source=WINSQL2012\\INDSQL2014;Initial Catalog=CRSCLOUD;uid=sa;pwd=01sql14*;";
        public DbSet<CountryDto> DbCountry { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connstr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MyAppModel(modelBuilder);
        }

        protected void MyAppModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CountryDto>()
                    .ToTable("COUNTRYPF");
            modelBuilder.Entity<CountryDto>(entity =>
            {
                entity.HasKey(pk => pk.COUNTRYID);
            });
        }
    }
}
