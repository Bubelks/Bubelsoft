namespace WebApi.Infrastructure.Email
{
    internal interface IEmailMessageProvider
    {
        string GetWorkerAdded(string userFirstName, string userLastName, int userId, string adminFirstName, string adminLastName, string companyName, string userCompanyRole);
        string GetCompanyInvited(string buildingName, string userFirstName, string userLastName, int buildingId);
    }
}