using Microsoft.EntityFrameworkCore;
using SCV_Portal.HelperMethods;
using SCV_Portal.Models;

namespace SCV_Portal.Entity
{
    public class ExceptionRptsDbContext : DbContext
    {
        public DbSet<ExceptionReports> DbExceptionReportModel { get; set; }
        public DbSet<ExceptionRptHistory> DbExceptionRptHistoryModel { get; set; }

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
            ExceptionRptHistoryodel(modelBuilder);
        }

        protected void MyAppModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExceptionReports>()
                    .ToTable("EXCEPTIONRPTS");

            modelBuilder.Entity<ExceptionReports>(entity =>
            {
                entity.HasKey(pk => pk.ID);
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.RPTNAME).HasColumnName("RPTNAME");
                entity.Property(e => e.ORDERPREC).HasColumnName("ORDERPREC");
                entity.Property(e => e.RPTPATH).HasColumnName("RPTPATH");
                entity.Property(e => e.SCVID).HasColumnName("SCVID");
                entity.Property(e => e.SOURCE).HasColumnName("SOURCE");
                entity.Property(e => e.FUNCNAME).HasColumnName("FUNCNAME");
                entity.Property(e => e.EXCPRPTFLAG).HasColumnName("EXCPRPTFLAG");
                entity.Property(e => e.ENABLED).HasColumnName("ENABLED");
                entity.Property(e => e.DESCRIPTION).HasColumnName("DESCRIPTION");
                entity.Property(e => e.RiskLevel).HasColumnName("RiskLevel");
            });
        }

        protected void ExceptionRptHistoryodel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExceptionRptHistory>()
                    .ToTable("ExceptionRptHistory");

            modelBuilder.Entity<ExceptionRptHistory>(entity =>
            {
                entity.HasKey(pk => pk.ID);
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.ReportName).HasColumnName("ReportName");
                entity.Property(e => e.ReportID).HasColumnName("ReportID");
                entity.Property(e => e.FunctionName).HasColumnName("FunctionName");
                entity.Property(e => e.IsActive).HasColumnName("IsActive");
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
                entity.Property(e => e.CreatedTime).HasColumnName("CreatedTime");
            });
        }

    }
}
