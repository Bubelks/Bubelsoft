using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BuildingContext
{
    internal class BuildingContextFactory: IDesignTimeDbContextFactory<BuildingContext>
    {
        public BuildingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BuildingContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=BuildingDatabase;User Id=sa;Password=Bubelsoft1");

            return new BuildingContext(optionsBuilder.Options);
        }

        public BuildingContext CreateBuildingContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BuildingContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new BuildingContext(optionsBuilder.Options);
        }
    }
}
