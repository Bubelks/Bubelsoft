using BuildingContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildingContext
{
    internal class BuildingContext: DbContext
    {
        public BuildingContext(DbContextOptions options): base(options)
        { }

        public DbSet<Estimation> Estimations { get; set; }

        public DbSet<Report> Reports { get; set; }
    }
}
