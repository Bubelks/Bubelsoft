using System;

namespace WebApi.Domain.Models
{
    public class Company
    {
        public Company(CompanyId id, string name): this(name)
        {
            Id = id;
        }

        public Company(string name)
        {
            Name = name;
        }

        public CompanyId Id { get; private set; }

        public string Name { get; }

        public void SetId(CompanyId id)
        {
            if (Id.Value != 0)
                throw new InvalidOperationException("Id is already set");
            Id = id;
        }
    }

    public struct CompanyId
    {
        public CompanyId(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}