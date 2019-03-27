using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BubelSoft.IntegrationTests.SecurityTests
{
    [Binding]
    public class LogInTestsSteps
    {
        private RestClient _client;
        private ActionResult<User> _userResult;

        [Given(@"I am logged in as MacBub")]
        public void GivenILoggedAsMacBub()
        {
            _client = new RestClient("MacBub", "qwe");
            ScenarioContext.Current["Client"] = _client;
        }

        [Given(@"I am not logged in")]
        public void GivenIDontLogged()
        {
            _client = new RestClient();
        }


        [When(@"I try to get current user")]
        public async Task WhenIGetUserInfo()
        {
            _userResult = await _client.GetAsync<User>("api/user/current").ConfigureAwait(false);
        }

        [Then(@"Result should be (.*)")]
        public void ThenResultShouldBeUnauthorized(HttpStatusCode statusCode)
        {
            Assert.That(_userResult.StatusCode, Is.EqualTo(statusCode));
        }

        [Then(@"Data current user should be MacBub")]
        public void ThenDataCurrentUserShouldBeMacBub()
        {
            var user = _userResult.Data;
            Assert.That(user.Name, Is.EqualTo("MacBub"));
            Assert.That(user.FirstName, Is.EqualTo("Maciek"));
            Assert.That(user.LastName, Is.EqualTo("Bubel"));
            Assert.That(user.Email, Is.EqualTo("macbub.fake@mail.com"));
        }

    }

    public class User
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}