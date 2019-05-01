using System;
using BubelSoft.Core.Domain.Models;
using NUnit.Framework;

namespace BubelSoft.Core.Domain.UnitTests
{
    [TestFixture]
    internal class CompanyTests
    {
        [TestCase("", "number")]
        [TestCase(null, "number")]
        [TestCase("name", "")]
        [TestCase("name", null)]
        public void CreateNew_ThrewException_IfNameOrNumberIsEmpty(string name, string number)
        {
            Assert.Throws<ArgumentException>(() => new Company(name, number));
        }

        [Test]
        public void CreateNew_CreateWithNew_WhenNameAndNumberAreNotEmpty()
        {
            const string name = "name";
            const string number = "number";
            var company = new Company(name, number);
            Assert.That(company.IsNew, Is.True);
            Assert.That(company.Name, Is.EqualTo(name));
            Assert.That(company.Number, Is.EqualTo(number));
        }

        [Test]
        public void SetId_ThrowException_IfItIsExistingBuilding()
        {
            var company = new Company(new CompanyId(1), "name", "number");
            Assert.Throws<InvalidOperationException>(() => company.SetId(new CompanyId(2)));
        }

        [Test]
        public void SetId_ShouldSetId_IfItIsNewBuilding()
        {
            var companyId = new CompanyId(2);
            var company = new Company("name","number");
            company.SetId(companyId);

            Assert.That(company.Id, Is.EqualTo(companyId));
        }

        [Test]
        public void Update_SetAllNewValues()
        {
            var company = new Company(new CompanyId(1),
                "name",
                "nip"
            );


            Assert.That(company.Name, Is.EqualTo("name"));
            Assert.That(company.Number, Is.EqualTo("nip"));

            company.Update(
                "NEW_name",
                "NEW_nip"
                );

            Assert.That(company.Name, Is.EqualTo("NEW_name"));
            Assert.That(company.Number, Is.EqualTo("NEW_nip"));
        }
    }
}
