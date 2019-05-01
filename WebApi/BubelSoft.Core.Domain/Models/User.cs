using System;
using System.Collections.Generic;
using System.Linq;

namespace BubelSoft.Core.Domain.Models
{
    public class User
    {
        public UserId Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public CompanyId CompanyId { get; private set; }
        public UserCompanyRole CompanyRole { get; }
        public bool IsNew => Id.Value == 0;

        public readonly IList<UserRole> Roles;

        public User(UserId id, string firstName, string lastName, UserCompanyRole companyRole, string email) 
            : this(firstName, lastName, companyRole, email)
        {
            Id = id;
        }

        public User(string firstName, string lastName, UserCompanyRole companyRole, string email)
        {
            Update(firstName, lastName, email);
            CompanyRole = companyRole;
            Roles = new List<UserRole>();
        }

        public void From(CompanyId companyId) 
            => CompanyId = companyId;

        public bool CanEdit(CompanyId companyId) 
            => companyId == CompanyId 
               && CompanyRole == UserCompanyRole.Admin;

        public bool CanManageWorkers(CompanyId companyId) 
            => companyId == CompanyId 
               && (CompanyRole == UserCompanyRole.Admin || CompanyRole == UserCompanyRole.UserAdmin);

        public bool CanReport(BuildingId buildingId) 
            => Roles.Any(r => r.BuildingId == buildingId && r.UserBuildingRole == UserBuildingRole.Reporter);

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

        public void Update(string firstName, string lastName, string email)
        {
            if(string.IsNullOrEmpty(firstName))
                throw new ArgumentException("First name cannot be empty", nameof(firstName));
            if(string.IsNullOrEmpty(lastName))
                throw new ArgumentException("Last name cannot be empty", nameof(lastName));
            if(string.IsNullOrEmpty(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            FirstName = firstName;
            LastName = lastName;
            Email = email;
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