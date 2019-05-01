using System.Collections.Generic;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database;
using BubelSoft.Core.Infrastructure.Database.Repositories;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using BubelSoft.Security;
using User = BubelSoft.Core.Domain.Models.User;

namespace BubelSoft.Core.Infrastructure.Initialization
{
    public static class DbInitializer
    {
        private static IUserRepository _userRepository;
        private static IBuildingRepository _buildingRepository;
        private static ICompanyRepository _companyRepository;

        public static void Initialize(MainContext context, IBubelSoftUserPassword bubelSoftUserPassword)
        {
            SetUpRepositories(context);

            var companies = new[]
            {
                new Company("Company 1", "123-456-789"),
                new Company("Company 2", "123-456-789")

            };
            foreach (var company in companies)
                _companyRepository.Save(company);

            var buildings = new[]
            {
                new Building("Building 1", companies[0], new []{ companies[1] }),
                new Building("Building 2", companies[1], new List<Company>())
            };
            foreach (var building in buildings)
                _buildingRepository.Save(building);
            
            var maciek = new User("MacBub", "Bubel", UserCompanyRole.Admin, "macbub.fake@mail.com");
            maciek.From(companies[0].Id);
            maciek.AddRole(buildings[0].Id, UserBuildingRole.Admin);
            maciek.AddRole(buildings[0].Id, UserBuildingRole.Reporter);
            _userRepository.Save(maciek, GeneratePasswordHash(maciek, bubelSoftUserPassword));

            var kamil = new User("KamBub", "Bubel", UserCompanyRole.Admin, "kambub.fake@mail.com");
            kamil.From(companies[1].Id);
            kamil.AddRole(buildings[1].Id, UserBuildingRole.Admin);
            kamil.AddRole(buildings[0].Id, UserBuildingRole.Admin);
            _userRepository.Save(kamil, GeneratePasswordHash(kamil, bubelSoftUserPassword));
        }

        private static void SetUpRepositories(MainContext context)
        {
            _userRepository = new UserRepository(context);
            _buildingRepository = new BuildingRepository(context);
            _companyRepository = new CompanyRepository(context);
        }

        private static string GeneratePasswordHash(User user, IBubelSoftUserPassword bubelSoftUserPassword)
        {
            var userLogInInfo = new UserLogInInfo
            {
                Email = user.Email,
                Password = "qwe"
            };
            return bubelSoftUserPassword.Hash(userLogInInfo);
        }
    }
}