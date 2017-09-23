using System.Xml.Linq;
using WebApi.Domain.Models;

namespace WebApi.Database.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        CompanyId Save(Company company);
    }
}