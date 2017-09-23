using Microsoft.AspNetCore.Identity;
using WebApi.Controllers.Security;
using WebApi.Database.Repositories;
using WebApi.Database.Repositories.Interfaces;
using WebApi.Domain.Models;

namespace WebApi.Database.Initialization
{
    public static class DbInitializer
    {
        private static IUserRepository _userRepository;
        private static IBuildingRepository _buildingRepository;
        private static ICompanyRepository _companyRepository;

        public static void Initialize(MainContext context)
        {
            SetUpRepositories(context);

            var companies = new[]
            {
                new Company("Company 1"),
                new Company("Company 2")
            };
            foreach (var company in companies)
                _companyRepository.Save(company);

            var buildings = new[]
            {
                new Building("Building 1", companies[0]),
                new Building("Building 2", companies[1])
            };
            foreach (var building in buildings)
                _buildingRepository.Save(building);
            
            var maciek = new User("MacBub", "Maciek", "Bubel", new CompanyId(1));
            _userRepository.Save(maciek, GeneratePasswordHash(maciek));

            var kamil = new User("KamBub", "Kamil", "Bubel", new CompanyId(2));
            _userRepository.Save(kamil, GeneratePasswordHash(kamil));
        }

        private static void SetUpRepositories(MainContext context)
        {
            _userRepository = new UserRepository(context);
            _buildingRepository = new BuildingRepository(context);
            _companyRepository = new CompanyRepository(context);
        }

        private static string GeneratePasswordHash(User user)
        {
            var userLogInInfo = new UserLogInInfo
            {
                UserName = user.Name,
                Password = "qwe"
            };
            var passwordHasher = new PasswordHasher<UserLogInInfo>();
            return passwordHasher.HashPassword(userLogInInfo, userLogInInfo.Password);
        }
    }
}