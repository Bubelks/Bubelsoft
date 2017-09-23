using System;

namespace WebApi.Domain.Models
{
    public class Building
    {
        public Building(string name, Company mainContractor)
        {
            Name = name;
            MainContractor = mainContractor;
        }

        public Building(BuildingId id, string name, Company mainContractor): this(name, mainContractor)
        {
            Id = id;
        }

        public Company MainContractor { get; }
        public BuildingId Id { get; private set; }
        public string Name { get; }

        public void SetId(BuildingId id)
        {
            if (Id.Value != 0)
                throw new InvalidOperationException("Id is already set");
            Id = id;
        }
    }

    public struct BuildingId
    {
        public BuildingId(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}