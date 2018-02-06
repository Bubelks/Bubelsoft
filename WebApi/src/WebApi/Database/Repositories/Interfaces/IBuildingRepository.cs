using System.Collections.Generic;
using WebApi.Domain.Models;

namespace WebApi.Database.Repositories.Interfaces
{
    public interface IBuildingRepository
    {
        Building Get(BuildingId id);
        IEnumerable<Building> GetFor(UserId userId);
        BuildingId Save(Building building);
        string GetConnectionString(BuildingId buildingId);
    }
}