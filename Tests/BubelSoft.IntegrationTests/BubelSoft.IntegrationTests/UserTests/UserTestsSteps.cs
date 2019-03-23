using System;
using System.Collections.Generic;
using System.Linq;
using BubelSoft.IntegrationTests.BuildingTests;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BubelSoft.IntegrationTests.UserTests
{
    [Binding]
    public class UserTestsSteps
    {
        private const string UserName = "UserName";
        private const string UserId = "UserId";
        private const string Password = "Password";

        [When(@"I add new worker to company")]
        public void WhenIAddNewWorkerToCompany()
        {
            var client = ScenarioContext.Current["Client"] as RestClient;

            var newUser = new User
            {
                Username = Guid.NewGuid().ToString().Substring(0,5),
                CompanyRole = UserCompanyRole.Worker,
                FirstName = "FN",
                LastName = "LN",
                PhoneNumber = "123123123",
                Email = "asdasd@dasd.asdas"
            };

            ScenarioContext.Current[UserName] = newUser.Username;

            ScenarioContext.Current[UserId] = client.Put("company/1/workers/add", newUser);
        }

        [When(@"I registry new user")]
        public void GivenIRegistryNewUser()
        {
            var password = "qwert";
            ScenarioContext.Current[Password] = password;

            var newUser = new UserRegisterInfo
            {
                Id = (int)ScenarioContext.Current[UserId],
                Password = password,
                Username = ScenarioContext.Current[UserName] as string,
                CompanyRole = UserCompanyRole.Worker,
                FirstName = "FN",
                LastName = "LN",
                PhoneNumber = "123123123",
                Email = "asdasd@dasd.asdas"
            };

            RestClient.RegistryUser(newUser);
        }
        
        [When(@"I login as new user")]
        public void WhenILoginAsNewUser()
        {
            ScenarioContext.Current["Client"] = new RestClient(ScenarioContext.Current[UserName] as string, ScenarioContext.Current[Password] as string);
        }
        
        [Then(@"I have user on workers list")]
        public void ThenIHaveUserOnWorkersList()
        {
            var client = ScenarioContext.Current["Client"] as RestClient;

            var workers = client.Get<IEnumerable<Worker>>("company/1/workers");

            var newWokrer = workers.Single(w => w.Value == (int)ScenarioContext.Current[UserId]);

            StringAssert.Contains(ScenarioContext.Current[UserName] as string, newWokrer.DisplayValue);
        }
    }

    public class Worker
    {
        public int Value { get; set; }
        public string DisplayValue { get; set; }
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
    public class UserRegisterInfo : User
    {
        public string Password { get; set; }
    }
}
