using Microsoft.EntityFrameworkCore;
using CRS_Portal.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRS_Portal.Models;

namespace CRS_Portal.Entity
{
    public class EntityLookupDbContext : DbContext
    {
        string connstr = "Data Source=WINSQL2012\\INDSQL2014;Initial Catalog=CRSCLOUD;uid=sa;pwd=01sql14*;";
        public DbSet<EntityLookupDetailDto> DbEntityLookup { get; set; }

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
            modelBuilder.Entity<EntityLookupDetailDto>()
                    .ToTable("CRS_ENTITY_LOOKUP");
            modelBuilder.Entity<EntityLookupDetailDto>(entity =>
            {
                entity.HasKey(pk => pk.ID);
            });
        }

    }
}
