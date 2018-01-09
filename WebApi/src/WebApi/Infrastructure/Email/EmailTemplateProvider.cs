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

        public string GetWorkedAdded(
            string userFirstName,
            string userLastName,
            string userId,
            string adminFirstName,
            string adminLastName,
            string companyName,
            string userCompanyRole)
        {
            var registerLink = $"{_configuration["Links:WebLink"]}register/{userId}";
            var contetnt = EmailTemplates.WorkerAdded
                .Replace("#UserFirstName", userFirstName)
                .Replace("#UserLastName", userLastName)
                .Replace("#AdminFirstName", adminFirstName)
                .Replace("#AdminLastName", adminLastName)
                .Replace("#CompanyName", companyName)
                .Replace("#UserCompanyRole", userCompanyRole)
                .Replace("#Link", registerLink);

            var full = EmailTemplates.FullBody
                .Replace("#Body", contetnt);

            return full;
        }
    }
}
