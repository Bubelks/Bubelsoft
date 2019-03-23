using BubelSoft.Building.Domain.Models;
using BubelSoft.Core.Domain.Models;

namespace BubelSoft.Building.Domain.AccessRules
{
    public interface IReportAccessRules
    {
        bool CanAccess(Report report, UserId userId);
    }
}