﻿using System.Collections.Generic;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database;
using BubelSoft.Core.Infrastructure.Database.Repositories;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BubelSoft.Core.Infrastructure.Initialization
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
                new Company("Company 1", "123-456-789", "123654789", "company1.fake@mail.com", "City", "78-963", "Steet", "221b"),
                new Company("Company 2", "123-456-789", "123654789", "company1.fake@mail.com", "City", "78-963", "Steet", "221b")
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
            
            var maciek = new User("MacBub", "Maciek", "Bubel", UserCompanyRole.Admin, "macbub.fake@mail.com", "123456789");
            maciek.From(companies[0].Id);
            maciek.AddRole(buildings[0].Id, UserBuildingRole.Admin);
            maciek.AddRole(buildings[0].Id, UserBuildingRole.Reporter);
            _userRepository.Save(maciek, GeneratePasswordHash(maciek));

            var kamil = new User("KamBub", "Kamil", "Bubel", UserCompanyRole.Admin, "kambub.fake@mail.com", "123456789");
            kamil.From(companies[1].Id);
            kamil.AddRole(buildings[1].Id, UserBuildingRole.Admin);
            kamil.AddRole(buildings[0].Id, UserBuildingRole.Admin);
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