using System.Linq;
using BubelSoft.Building.Domain.Models;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;

namespace BubelSoft.Building.Domain.AccessRules
{
    public class ReportAccessRules : IReportAccessRules
    {
        private readonly IUserRepository _userRepository;
        private readonly IBuildingRepository _buildingRepository;

        public ReportAccessRules(IUserRepository userRepository, IBuildingRepository buildingRepository)
        {
            _userRepository = userRepository;
            _buildingRepository = buildingRepository;
        }

        public bool CanAccess(Report report, UserId userId)
        {
            if (report.ReporterId == userId)
                return true;

            var user = _userRepository.Get(userId);

            if (user.Roles.Any(r =>
                r.BuildingId == report.BuildingId && r.UserBuildingRole == UserBuildingRole.Supervisor))
            {
                var reporter = _userRepository.Get(report.ReporterId);
                if (reporter.CompanyId == user.CompanyId)
                    return true;

                var building = _buildingRepository.Get(report.BuildingId);
                if (building.MainContractor.Id == user.CompanyId)
                    return true;
            }

            return false;
        }
    }
}