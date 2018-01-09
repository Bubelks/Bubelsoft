using WebApi.Domain.Models;
using WebApi.Infrastructure.Email;

namespace WebApi.Services
{
    public interface IMailService
    {
        void SendWorkerAddedInfo(User user, User admin, Company company);
    }


    internal class MailService: IMailService
    {
        private readonly IEmailMessageProvider _emailMessageProvider;

        public MailService(IEmailMessageProvider emailMessageProvider)
        {
            _emailMessageProvider = emailMessageProvider;
        }

        public void SendWorkerAddedInfo(User user, User admin, Company company)
        {
            var message = _emailMessageProvider.GetWorkedAdded(
                user.FirstName,
                user.LastName,
                user.Id.Value,
                admin.FirstName,
                admin.LastName,
                company.Name,
                user.CompanyRole.ToString());

            const string subject = "Welcome in BUBELSOFT";

            EmailSender.Send(message,  subject, user.Email);
        }
    }
}
