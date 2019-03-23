using System.Linq;
using BubelSoft.Core.Domain.Models;
using NUnit.Framework;

namespace BubelSoft.Core.Domain.UnitTests
{
    [TestFixture]
    public class UserTests
    {
        [TestCase(1, 1, UserCompanyRole.Admin, true)]
        [TestCase(1, 1, UserCompanyRole.UserAdmin, false)]
        [TestCase(1, 1, UserCompanyRole.Worker, false)]
        [TestCase(2, 1, UserCompanyRole.Admin, false)]
        public void CanEdit_CheckCompanyIdAndRole(int companyId, int userCompanyId, UserCompanyRole role, bool result)
        {
            var user = new User("", "", "", role, "", "");
            user.From(new CompanyId(userCompanyId));

            var canEdit = user.CanEdit(new CompanyId(companyId));

            Assert.That(canEdit, Is.EqualTo(result));
        }

        [TestCase(1, 1, UserCompanyRole.Admin, true)]
        [TestCase(1, 1, UserCompanyRole.UserAdmin, true)]
        [TestCase(1, 1, UserCompanyRole.Worker, false)]
        [TestCase(2, 1, UserCompanyRole.Admin, false)]
        public void CanManageWorkers_CheckCompanyIdAndRole(int companyId, int userCompanyId, UserCompanyRole role, bool result)
        {
            var user = new User("", "", "", role, "", "");
            user.From(new CompanyId(userCompanyId));

            var canEdit = user.CanManageWorkers(new CompanyId(companyId));

            Assert.That(canEdit, Is.EqualTo(result));
        }

        [Test]
        public void AddRole_ShouldNotduplcateRole()
        {
            var user = new User("", "", "", UserCompanyRole.Worker, "", "");

            var buildingId = new BuildingId(1);
            user.AddRole(buildingId, UserBuildingRole.Admin);
            user.AddRole(buildingId, UserBuildingRole.Reporter);

            Assert.That(user.Roles, Has.Count.EqualTo(2));
            Assert.That(user.Roles.SingleOrDefault(r => r.UserBuildingRole == UserBuildingRole.Admin).BuildingId, Is.EqualTo(buildingId));
            Assert.That(user.Roles.SingleOrDefault(r => r.UserBuildingRole == UserBuildingRole.Reporter).BuildingId, Is.EqualTo(buildingId));

            user.AddRole(buildingId, UserBuildingRole.Reporter);

            Assert.That(user.Roles, Has.Count.EqualTo(2));
            Assert.That(user.Roles.SingleOrDefault(r => r.UserBuildingRole == UserBuildingRole.Admin).BuildingId, Is.EqualTo(buildingId));
            Assert.That(user.Roles.SingleOrDefault(r => r.UserBuildingRole == UserBuildingRole.Reporter).BuildingId, Is.EqualTo(buildingId));
        }


        [TestCase(1, 1, UserBuildingRole.Reporter, true)]
        [TestCase(1, 1, UserBuildingRole.Admin, false)]
        [TestCase(2, 1, UserBuildingRole.Reporter, false)]
        public void CanManageWorkers_CheckCompanyIdAndRole(int buildingId, int userBuildingId, UserBuildingRole role, bool result)
        {
            var user = new User("", "", "", UserCompanyRole.Worker, "", "");
            
            user.AddRole(new BuildingId(userBuildingId), role);

            var canReport = user.CanReport(new BuildingId(buildingId));

            Assert.That(canReport, Is.EqualTo(result));
        }
    }
}