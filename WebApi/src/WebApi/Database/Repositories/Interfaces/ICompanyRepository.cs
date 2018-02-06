using System.Collections.Generic;
using WebApi.Domain.Models;

namespace WebApi.Database.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        CompanyId Save(Company company);
        Company Get(CompanyId companyId);
        bool Exists(CompanyId companyId);
        void DeleteWorkers(CompanyId companyId, IEnumerable<UserId> users);
        IEnumerable<Company> GetContractors(BuildingId buildingId);
        IEnumerable<Company> GetAll();
        void AddSubContract(BuildingId buildingId, CompanyId companyId);
    }
}