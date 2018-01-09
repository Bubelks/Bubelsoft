namespace WebApi.Infrastructure.Email
{
    internal interface IEmailMessageProvider
    {
        string GetWorkedAdded(string userFirstName, string userLastName, int userId, string adminFirstName, string adminLastName, string companyName, string userCompanyRole);
    }
}