using System;
using System.Linq;
using BubelSoft.Core.Domain.Models;
using NUnit.Framework;

namespace BubelSoft.Core.Domain.UnitTests
{
    [TestFixture]
    public class UserTests
    {
        [TestCase("", "lastName", "email")]
        [TestCase(null, "lastName", "email")]
        [TestCase("firstName", "", "email")]
        [TestCase("firstName", null, "email")]
        [TestCase("firstName", "lastName", "")]
        [TestCase("firstName", "lastName", null)]
        public void CreateNew_ThrowException_IfNamesOrEmailIsEmpty(string firstName, string lastName, string email)
        {
            Assert.Throws<ArgumentException>(() => new User(firstName, lastName, UserCompanyRole.Worker, email));
        }

        [Test]
        public void CreateNew_CreateWithNullId_WhenAllPropertyAreNotEmpty()
        {
            const string lastName = "name";
            const string firstName = "number";
            const UserCompanyRole companyRole = UserCompanyRole.Admin;
            const string email = "email";

            var company = new User(lastName, firstName, companyRole, email);

            Assert.That(company.IsNew, Is.True);
            Assert.That(company.FirstName, Is.EqualTo(lastName));
            Assert.That(company.LastName, Is.EqualTo(firstName));
            Assert.That(company.CompanyRole, Is.EqualTo(companyRole));
            Assert.That(company.Email, Is.EqualTo(email));
        }

        [TestCase(1, 1, UserCompanyRole.Admin, true)]
        [TestCase(1, 1, UserCompanyRole.UserAdmin, false)]
        [TestCase(1, 1, UserCompanyRole.Worker, false)]
        [TestCase(2, 1, UserCompanyRole.Admin, false)]
        public void CanEdit_CheckCompanyIdAndRole(int companyId, int userCompanyId, UserCompanyRole role, bool result)
        {
            var user = new User("", "", role, "");
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
            var user = new User("", "", role, "");
            user.From(new CompanyId(userCompanyId));

            var canEdit = user.CanManageWorkers(new CompanyId(companyId));

            Assert.That(canEdit, Is.EqualTo(result));
        }

        [Test]
        public void AddRole_ShouldNotduplcateRole()
        {
            var user = new User("", "", UserCompanyRole.Worker, "");

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
            var user = new User("", "", UserCompanyRole.Worker, "");
            
            user.AddRole(new BuildingId(userBuildingId), role);

            var canReport = user.CanReport(new BuildingId(buildingId));

            Assert.That(canReport, Is.EqualTo(result));
        }
    }
}