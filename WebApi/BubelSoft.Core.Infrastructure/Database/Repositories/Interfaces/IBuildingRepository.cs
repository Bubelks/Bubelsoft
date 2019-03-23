using System.Collections.Generic;
using BubelSoft.Core.Domain.Models;

namespace BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces
{
    public interface IBuildingRepository
    {
        Building Get(BuildingId id);
        IEnumerable<Building> GetFor(UserId userId);
        BuildingId Save(Building building);
        string GetConnectionString(BuildingId buildingId);
    }
}