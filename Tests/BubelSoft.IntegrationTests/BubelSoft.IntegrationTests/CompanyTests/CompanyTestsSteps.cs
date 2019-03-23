using System;
using System.Collections.Generic;
using System.Linq;
using BubelSoft.IntegrationTests.BuildingTests;
using BubelSoft.IntegrationTests.UserTests;
using NUnit.Framework;
using TechTalk.SpecFlow;
using User = BubelSoft.IntegrationTests.UserTests.User;

namespace BubelSoft.IntegrationTests.CompanyTests
{
    [Binding]
    public class CompanyTestsSteps
    {
        private const string UserId = "UserId";
        private const string Company = "Company";

        [When(@"I delete new Worker")]
        public void WhenIDeleteNewWorker()
        {
            var client = ScenarioContext.Current["Client"] as RestClient;

            client.Put("company/1/workers/delete", new[] {(int) ScenarioContext.Current[UserId]});
        }
        
        [Then(@"I do not get Worker on List")]
        public void ThenIDoNotGetWorkerOnList()
        {
            var client = ScenarioContext.Current["Client"] as RestClient;

            var workers = client.Get<IEnumerable<Worker>>("company/1/workers");
            
            Assert.IsTrue(workers.All(w => w.Value != (int)ScenarioContext.Current[UserId]), "Deleted user is on workers list");
        }

        [Given(@"I get company info")]
        public void GivenIGetCompanyInfo()
        {
            var client = ScenarioContext.Current["Client"] as RestClient;

            var company = client.Get<Company>("company/1");

            Assert.IsTrue(company.CanEdit);

            ScenarioContext.Current[Company] = company;
        }

        [When(@"I change company info")]
        public void WhenIChangeCompanyInfo()
        {
            var client = ScenarioContext.Current["Client"] as RestClient;

            var companyInfo = new CompanyInfo
            {
                City = "NEW_CITY",
                Email = "asdasd@sdasd.asda",
                Name = "NEW_NAME",
                Nip = "NEW_NIP",
                PhoneNumber = "NEW_PN",
                PlaceNumber = "NEW_PLACE",
                PostCode = "NEW_POST",
                Street = "NEW_STREET"
            };

            client.Put("company/1", companyInfo);
        }

        [Then(@"I ensure that company info is changed")]
        public void ThenIEnsureThatCompanyInfoIsChanged()
        {
            var client = ScenarioContext.Current["Client"] as RestClient;

            var company = client.Get<Company>("company/1");

            Assert.That(company.City, Is.EqualTo("NEW_CITY"));
            Assert.That(company.Name, Is.EqualTo("NEW_NAME"));
            Assert.That(company.Nip, Is.EqualTo("NEW_NIP"));
            Assert.That(company.PhoneNumber, Is.EqualTo("NEW_PN"));
            Assert.That(company.PlaceNumber, Is.EqualTo("NEW_PLACE"));
            Assert.That(company.PostCode, Is.EqualTo("NEW_POST"));
            Assert.That(company.Street, Is.EqualTo("NEW_STREET"));
        }

        [Then(@"I rest company info")]
        public void ThenIRestCompanyInfo()
        {
            var client = ScenarioContext.Current["Client"] as RestClient;
            var companyInfo = ScenarioContext.Current[Company] as CompanyInfo;
            client.Put("company/1", companyInfo);
        }
    }

    public class Company : CompanyInfo
    {
        public IEnumerable<User> Workers { get; set; }
        public bool CanManageWorkers { get; set; }
        public bool CanEdit { get; set; }
    }
}
