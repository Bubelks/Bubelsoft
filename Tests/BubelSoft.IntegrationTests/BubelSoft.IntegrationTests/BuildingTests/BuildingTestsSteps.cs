using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BubelSoft.IntegrationTests.BuildingTests
{
    [Binding]
    public class BuildingTestsSteps
    {
        private const string BuildingId = "buildingId";
        private const string CompanyName = "companyName";
        private const string UserName = "userName";

        private RestClient _client;

        [Given(@"I logged as MacBub")]
        public void GivenILoggedAsMacBub()
        {
            _client = new RestClient("MacBub", "qwe");
            ScenarioContext.Current["Client"] = _client;
        }

        [Given(@"I have access to Building")]
        public void GivenIHaveAccessToBuilding()
        {
            var buildings = _client.Get<IEnumerable<BuildingsListItem>>("buildings").ToList();

            Assert.That(buildings, Has.Count.EqualTo(1));

            ScenarioContext.Current[BuildingId] = buildings[0].Id;
        }

        [When(@"I add new Company to Building")]
        public void WhenIAddCompany()
        {
            var buildingId = (int)ScenarioContext.Current[BuildingId];
            var company = new BuildingCompany
            {
                BuildingId = buildingId,
                City = "City",
                Email = "Email",
                Name = Guid.NewGuid().ToString(),
                Nip = "Nip",
                PhoneNumber = "12334541",
                PlaceNumber = "12",
                PostCode = "12343",
                Street = "Street"
            };

            ScenarioContext.Current[CompanyName] = company.Name;

            _client.Post("company", company);
        }

        [Then(@"I have new Company on List")]
        public void ThenIGetTwoCompaniesOnList()
        {
            var buildingId = ScenarioContext.Current[BuildingId];
            var companyName = ScenarioContext.Current[CompanyName];
            var companies = _client.Get<IEnumerable<BuildingCompanyListItem>>($"buildings/{buildingId}/companies").ToList();
            
            Assert.IsTrue(companies.Any(c => c.Name == (string) companyName));
        }

        [When(@"I add new User to my Company to Building")]
        public void WhenIAddNewUserToMyCompanyToBuilding()
        {
            var user = new User
            {
                FirstName = "FName",
                CompanyRole = UserCompanyRole.Admin,
                Email = "fake@asdasd.asdfafa",
                LastName = "LName",
                PhoneNumber = "123321123",
                Username = Guid.NewGuid().ToString()
            };

            ScenarioContext.Current[UserName] = user.Username;
            var userId = _client.Put("company/1/workers/add", user);

            var worker = new BuildingWorker
            {
                UserBuildingRoles = new []{UserBuildingRole.Admin},
                UserId = userId.Value
            };

            var buildingId = ScenarioContext.Current[BuildingId];
            _client.Post($"buildings/{buildingId}/worker/add", worker);
        }

        [Then(@"I get new Worker on List")]
        public void ThenIGetNewWorkerOnList()
        {
            var buildingId = ScenarioContext.Current[BuildingId];
            var companies = _client.Get<IEnumerable<BuildingCompanyListItem>>($"buildings/{buildingId}/companies").ToList();

            var userName = ScenarioContext.Current[UserName];
            Assert.IsTrue(
                companies
                .SelectMany(c => c.Workers)
                .Any(w => w.DisplayName.Contains((string)userName)));
        }
    }
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserCompanyRole CompanyRole { get; set; }
        public int Id { get; set; }
    }

    public enum UserCompanyRole
    {
        Admin,
        UserAdmin,
        Worker
    }

    public class BuildingCompanyListItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool MainContract { get; set; }

        public IEnumerable<BuildingWorker> Workers { get; set; }
        public bool CanAddWorker { get; set; }
    }

    public class BuildingWorker
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<UserBuildingRole> UserBuildingRoles { get; set; }
    }


    internal class BuildingsListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool OwnedByMy { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
    }

    public class BuildingCompany : CompanyInfo
    {
        public int BuildingId { get; set; }
    }

    public class CompanyInfo : CompanyBase
    {
        public string Nip { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public string PlaceNumber { get; set; }
    }

    public class CompanyBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public enum UserBuildingRole
    {
        Admin,
        Reporter,
        Supervisor
    }
}
