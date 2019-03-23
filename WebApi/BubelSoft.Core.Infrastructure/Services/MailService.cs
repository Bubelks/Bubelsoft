using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Email;

namespace BubelSoft.Core.Infrastructure.Services
{
    public interface IMailService
    {
        void SendWorkerAddedInfo(User user, User admin, Company company);
        void SendCompanyInvitedInfo(User user, Building building, string email);
    }


    public class MailService: IMailService
    {
        private readonly IEmailMessageProvider _emailMessageProvider;

        public MailService(IEmailMessageProvider emailMessageProvider)
        {
            _emailMessageProvider = emailMessageProvider;
        }

        public void SendWorkerAddedInfo(User user, User admin, Company company)
        {
            var message = _emailMessageProvider.GetWorkerAdded(
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

        public void SendCompanyInvitedInfo(User user, Building building, string email)
        {
            var message = _emailMessageProvider.GetCompanyInvited(
                building.Name,
                user.FirstName,
                user.LastName,
                building.Id.Value
            );


            const string subject = "Welcome in BUBELSOFT";

            EmailSender.Send(message, subject, email);
        }
    }
}
