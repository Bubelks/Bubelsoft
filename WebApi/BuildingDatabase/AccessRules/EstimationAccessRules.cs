using System.Linq;
using BubelSoft.Building.Domain.Models;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;

namespace BubelSoft.Building.Domain.AccessRules
{
    public class EstimationAccessRules: IEstimationAccessRules
    {
        private readonly IUserRepository _userRepository;
        private readonly IBuildingRepository _buildingRepository;

        public EstimationAccessRules(IUserRepository userRepository, IBuildingRepository buildingRepository)
        {
            _userRepository = userRepository;
            _buildingRepository = buildingRepository;
        }
        public bool CanEdit(Estimation estimation, UserId userId, BuildingId buildingId)
        {
            var user = _userRepository.Get(userId);
            var building = _buildingRepository.Get(buildingId);

            return user.CompanyId == building.MainContractor.Id
                   && user.Roles.Any(r => r.BuildingId == buildingId && r.UserBuildingRole == UserBuildingRole.Admin);
        }
    }
}