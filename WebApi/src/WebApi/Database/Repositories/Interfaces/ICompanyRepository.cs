using System.Collections.Generic;
using System.Xml.Linq;
using WebApi.Domain.Models;

namespace WebApi.Database.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        CompanyId Save(Company company);
        Company Get(CompanyId companyId);
        bool Exists(CompanyId companyId);
        void DeleteWorkers(CompanyId companyId, IEnumerable<UserId> users);
    }
}