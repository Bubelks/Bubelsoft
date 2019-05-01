using System;
using BubelSoft.Core.Domain.Models;
using NUnit.Framework;

namespace BubelSoft.Core.Domain.UnitTests
{
    [TestFixture]
    public class BuildingTests
    {
        [Test]
        public void Create_MainContractor_CannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Building(new BuildingId(1), "name", mainContractor: null));
        }

        [Test]
        public void SetId_ThrowException_IfItIsExistingBuilding()
        {
            var building = new Building(new BuildingId(1), "name", new Company(new CompanyId(1), "cName", "cNumber"));
            Assert.Throws<InvalidOperationException>(() => building.SetId(new BuildingId(2)));
        }

        [Test]
        public void SetId_ShouldSetId_IfItIsNewBuilding()
        {
            var buildingId = new BuildingId(2);
            var building = new Building("name", new Company(new CompanyId(1), "cName", "cNumber"));
            building.SetId(buildingId);

            Assert.That(building.Id, Is.EqualTo(buildingId));
        }

        [TestCase(3, 3, true)]
        [TestCase(5, 4,false)]
        public void IsOwnedBy_ShouldCheckMainContractor(int mainContractorId, int userCompanyId, bool result)
        {
            var building = new Building("name", new Company(new CompanyId(mainContractorId), "cName", "cNumber"));
            var user = new User(new UserId(1), "", "", UserCompanyRole.Admin, "");
            user.From(new CompanyId(userCompanyId));

            var isOwner = building.IsOwnedBy(user);
            Assert.That(isOwner, Is.EqualTo(result));
        }

        [TestCase(4, 3, 3, true)]
        [TestCase(5, 4, 5, true)]
        [TestCase(5, 4, 3, false)]
        public void CanAccess_ShouldCheckMainContractorAndSubContractors(int mainContractorId,int subContractorId, int userCompanyId, bool result)
        {
            var buildingId = new BuildingId(3);
            var building = new Building(buildingId, "name", new Company(new CompanyId(mainContractorId), "cName", "cNumber"), new []{ new Company(new CompanyId(subContractorId), "sCName", "sCNumber") });
            var user = new User(new UserId(1),"", "", UserCompanyRole.Admin, "");
            user.From(new CompanyId(userCompanyId));
            if(userCompanyId == mainContractorId || userCompanyId == subContractorId)
                user.AddRole(buildingId, UserBuildingRole.Supervisor);

            var access = building.CanAccess(user);

            Assert.That(access, Is.EqualTo(result));
        }
    }
}
