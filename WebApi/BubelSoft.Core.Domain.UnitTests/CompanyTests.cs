using System;
using BubelSoft.Core.Domain.Models;
using NUnit.Framework;

namespace BubelSoft.Core.Domain.UnitTests
{
    [TestFixture]
    class CompanyTests
    {

        [Test]
        public void SetId_ThrowException_IfItIsExistingBuilding()
        {
            var company = new Company(new CompanyId(1), "name");
            Assert.Throws<InvalidOperationException>(() => company.SetId(new CompanyId(2)));
        }

        [Test]
        public void SetId_ShouldSetId_IfItIsNewBuilding()
        {
            var companyId = new CompanyId(2);
            var company = new Company("name");
            company.SetId(companyId);

            Assert.That(company.Id, Is.EqualTo(companyId));
        }

        [Test]
        public void Update_SetAllNewValues()
        {
            var company = new Company(new CompanyId(1),
                "name",
                "nip",
                "phoneNumber",
                "eMail",
                "city",
                "postCode",
                "street",
                "placeNumber"
            );


            Assert.That(company.Name, Is.EqualTo("name"));
            Assert.That(company.Nip, Is.EqualTo("nip"));
            Assert.That(company.PhoneNumber, Is.EqualTo("phoneNumber"));
            Assert.That(company.Email, Is.EqualTo("eMail"));
            Assert.That(company.City, Is.EqualTo("city"));
            Assert.That(company.PostCode, Is.EqualTo("postCode"));
            Assert.That(company.Street, Is.EqualTo("street"));
            Assert.That(company.PlaceNumber, Is.EqualTo("placeNumber"));

            company.Update(
                "NEW_name",
                "NEW_nip",
                "NEW_phoneNumber",
                "NEW_eMail",
                "NEW_city",
                "NEW_postCode",
                "NEW_street",
                "NEW_placeNumber"
                );

            Assert.That(company.Name, Is.EqualTo("NEW_name"));
            Assert.That(company.Nip, Is.EqualTo("NEW_nip"));
            Assert.That(company.PhoneNumber, Is.EqualTo("NEW_phoneNumber"));
            Assert.That(company.Email, Is.EqualTo("NEW_eMail"));
            Assert.That(company.City, Is.EqualTo("NEW_city"));
            Assert.That(company.PostCode, Is.EqualTo("NEW_postCode"));
            Assert.That(company.Street, Is.EqualTo("NEW_street"));
            Assert.That(company.PlaceNumber, Is.EqualTo("NEW_placeNumber"));
        }
    }
}
