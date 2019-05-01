using System;

namespace BubelSoft.Core.Domain.Models
{
    public class Company
    {
        public Company(CompanyId id, string name, string number)
            : this(name, number)
        {
            Id = id;
        }

        public Company(string name, string number)
        {
            Update(name, number);
        }

        public CompanyId Id { get; private set; }
        public string Name { get; private set; }
        public string Number { get; private set; }
        public bool IsNew => Id.Value == 0;
        
        public void SetId(CompanyId id)
        {
            if (Id.Value != 0)
                throw new InvalidOperationException("Id is already set");
            Id = id;
        }

        public void Update(string name, string number)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentException("Company name cannot be empty", nameof(name));

            if(string.IsNullOrEmpty(number))
                throw new ArgumentException("Company number cannot be empty", nameof(number));

            Name = name;
            Number = number;
        }
    }
}