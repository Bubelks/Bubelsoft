using BubelSoft.Building.Domain.Models;
using BubelSoft.Core.Domain.Models;

namespace BubelSoft.Building.Domain.AccessRules
{
    public interface IEstimationAccessRules
    {
        bool CanEdit(Estimation estimation, UserId userId, BuildingId buildingId);
    }
}