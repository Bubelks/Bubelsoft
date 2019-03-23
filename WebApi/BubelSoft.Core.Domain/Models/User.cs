using System;
using System.Collections.Generic;
using System.Linq;

namespace BubelSoft.Core.Domain.Models
{
    public class User
    {
        public UserId Id { get; private set; }
        public string Name { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public CompanyId CompanyId { get; private set; }
        public UserCompanyRole CompanyRole { get; }

        public readonly IList<UserRole> Roles;

        public User(UserId id, string name, string firstName, string lastName, UserCompanyRole companyRole, string email, string phoneNumber) 
            : this(name, firstName, lastName, companyRole, email, phoneNumber)
        {
            Id = id;
        }

        public User(string name, string firstName, string lastName, UserCompanyRole companyRole, string email, string phoneNumber)
        {
            Name = name;
            FirstName = firstName;
            LastName = lastName;
            CompanyRole = companyRole;
            Email = email;
            PhoneNumber = phoneNumber;
            Roles = new List<UserRole>();
        }

        public void From(CompanyId companyId)
        {
            CompanyId = companyId;
        }

        public bool CanEdit(CompanyId companyId) => companyId == CompanyId &&
                                                    CompanyRole == UserCompanyRole.Admin;

        public bool CanManageWorkers(CompanyId companyId) => companyId == CompanyId &&
                                                             (CompanyRole == UserCompanyRole.Admin || CompanyRole == UserCompanyRole.UserAdmin);

        public bool CanReport(BuildingId buildingId) => Roles.Any(r => r.BuildingId == buildingId && r.UserBuildingRole == UserBuildingRole.Reporter);

        public void SetId(UserId id)
        {
            if (Id.Value != 0)
                throw new InvalidOperationException("Id is already set");
            Id = id;
        }

        public void AddRole(BuildingId buildingId, UserBuildingRole userBuildingRole)
        {
            if (!Roles.Any(r => r.BuildingId == buildingId && r.UserBuildingRole == userBuildingRole))
                Roles.Add(new UserRole(userBuildingRole, buildingId));
        }


        public class UserRole
        {
            public UserRole(UserBuildingRole userBuildingRole, BuildingId buildingId)
            {
                UserBuildingRole = userBuildingRole;
                BuildingId = buildingId;
            }

            public UserBuildingRole UserBuildingRole { get; }

            public BuildingId BuildingId { get; }
        }
    }
    public class UserLogInInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }
    }

    public enum UserCompanyRole
    {
        Admin,
        UserAdmin,
        Worker
    }

    public enum UserBuildingRole
    {
        Admin,
        Reporter,
        Supervisor
    }
}