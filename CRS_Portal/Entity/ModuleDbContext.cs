using Microsoft.EntityFrameworkCore;
using CRS_Portal.HelperMethods;
using CRS_Portal.Models;

namespace CRS_Portal.Entity
{
    public class ModuleDbContext : DbContext
    {
        public DbSet<UserAccessLevel> DbUserAccessLevelModel { get; set; }

        public DbSet<ModuleParent> DbModuleParentModel { get; set; }

        public DbSet<ModuleChild> DbModuleChildModel { get; set; }


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
            modelBuilder.Entity<UserAccessLevel>()
            .ToTable("UserAccessLevel");

            modelBuilder.Entity<UserAccessLevel>(entity =>
            {
                entity.HasKey(pk => pk.ID);
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.UserID).HasColumnName("UserID");
                entity.Property(e => e.MenuID).HasColumnName("MenuID");
                entity.Property(e => e.IsAccess).HasColumnName("IsAccess");
            });


            modelBuilder.Entity<ModuleParent>().Ignore(r=>r.ModuleChild).Ignore(r => r.IsChecked_P)
            .ToTable("SCV_ParentModuleDetails");

            modelBuilder.Entity<ModuleParent>(entity =>
            {
                entity.HasKey(pk => pk.ID);
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.ModuleTitle).HasColumnName("ModuleTitle");
            });


            modelBuilder.Entity<ModuleChild>().Ignore(r=>r.IsChecked_C)
            .ToTable("SCV_ChildModuleDetails");

            modelBuilder.Entity<ModuleChild>(entity =>
            {
                entity.HasKey(pk => pk.ID);
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.ModuleName).HasColumnName("ModuleName");
                entity.Property(e => e.ModuleParentId).HasColumnName("ModuleParentId");
                entity.Property(e => e.PageID).HasColumnName("PageID");
            });
        }
    }
}

