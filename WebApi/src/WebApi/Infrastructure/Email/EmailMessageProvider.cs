using Microsoft.Extensions.Configuration;

namespace WebApi.Infrastructure.Email
{
    internal class EmailMessageProvider: IEmailMessageProvider
    {
        private readonly IConfigurationRoot _configuration;

        public EmailMessageProvider(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public string GetWorkerAdded(string userFirstName, string userLastName, int userId, string adminFirstName, string adminLastName, string companyName, string userCompanyRole)
        {
            var registerLink = $"{_configuration["Links:WebLink"]}#user/register/{userId}";
            var content = EmailTemplates.WorkerAdded
                .Replace("#UserFirstName", userFirstName)
                .Replace("#UserLastName", userLastName)
                .Replace("#AdminFirstName", adminFirstName)
                .Replace("#AdminLastName", adminLastName)
                .Replace("#CompanyName", companyName)
                .Replace("#UserCompanyRole", userCompanyRole)
                .Replace("#Link", registerLink);

            var full = EmailTemplates.FullBody
                .Replace("#Body", content);

            return full;
        }

        public string GetCompanyInvited(string buildingName, string userFirstName, string userLastName, int buildingId)
        {
            var registryLink = $"{_configuration["Links:WebLink"]}#buildings/{buildingId}/subContractor";
            var content = EmailTemplates.CompanyInvited
                .Replace("#BuildingName", buildingName)
                .Replace("#UserFirstName", userFirstName)
                .Replace("#UserLastName", userLastName)
                .Replace("#Link", registryLink);

            var full = EmailTemplates.FullBody
                .Replace("#Body", content);

            return full;
        }
    }
}
