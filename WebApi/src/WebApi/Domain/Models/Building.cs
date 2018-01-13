using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Domain.Models
{
    public class Building
    {
        public Building(string name, Company mainContractor, IEnumerable<Company> subContractors)
        {
            Name = name;
            MainContractor = mainContractor;
            SubContractors = subContractors;
        }

        public Building(BuildingId id, string name, Company mainContractor, IEnumerable<Company> companies): this(name, mainContractor, companies)
        {
            Id = id;
        }

        public Company MainContractor { get; }

        public IEnumerable<Company> SubContractors { get; }

        public BuildingId Id { get; private set; }
        public string Name { get; }

        public void SetId(BuildingId id)
        {
            if (Id.Value != 0)
                throw new InvalidOperationException("Id is already set");
            Id = id;
        }

        public bool IsOwnedBy(User user)
        {
            return MainContractor.Id == user.CompanyId;
        }

        public bool CanAccess(User user)
        {
            return SubContractors.Any(c => c.Id == user.CompanyId) || MainContractor.Id == user.CompanyId;
        }
    }

    public struct BuildingId
    {
        public bool Equals(BuildingId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is BuildingId && Equals((BuildingId) obj);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public BuildingId(int value)
        {
            Value = value;
        }

        public int Value { get; }


        public static bool operator ==(BuildingId id1, BuildingId id2)
        {
            return id1.Value == id2.Value;
        }

        public static bool operator !=(BuildingId id1, BuildingId id2)
        {
            return !(id1 == id2);
        }
    }
}