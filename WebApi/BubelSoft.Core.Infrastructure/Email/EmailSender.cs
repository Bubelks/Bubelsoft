using System.Net;
using System.Net.Mail;

namespace BubelSoft.Core.Infrastructure.Email
{
    public class EmailSender
    {
        public static void Send(string body, string subject, string to)
        {
            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("bubelsoft.test@gmail.com", "bubelsoft1234!"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("bubelsoft.test@gmail.com"),
                Body = body,
                Subject = subject,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);
            client.Send(mailMessage);
        }
    }
}
