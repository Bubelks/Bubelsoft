using Bubelsoft.Building.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bubelsoft.Building.Infrastructure
{
    public class BuildingContext: DbContext
    {
        public BuildingContext(DbContextOptions options): base(options)
        { }

        public DbSet<Estimation> Estimations { get; set; }

        public DbSet<Report> Reports { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportQuantity>()
                .HasKey(rq => new {rq.ReportId, rq.EstimationId});

            modelBuilder.Entity<ReportQuantity>()
                .HasOne(bc => bc.Report)
                .WithMany(b => b.Quantities)
                .HasForeignKey(bc => bc.ReportId);

            modelBuilder.Entity<ReportQuantity>()
                .HasOne(bc => bc.Estimation)
                .WithMany(b => b.Quantities)
                .HasForeignKey(bc => bc.EstimationId);
        }
    }
}
