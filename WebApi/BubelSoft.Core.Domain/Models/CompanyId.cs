using System;

namespace BubelSoft.Core.Domain.Models
{
    public struct CompanyId
    {
        public CompanyId(int value)
        {
            if(value < 0)
                throw new ArgumentException("Company Id has to be non-negative");

            Value = value;
        }

        public int Value { get; }

        public static bool operator ==(CompanyId id1, CompanyId id2)
        {
            return id1.Value == id2.Value;
        }

        public static bool operator !=(CompanyId id1, CompanyId id2)
        {
            return !(id1 == id2);
        }

        public bool Equals(CompanyId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CompanyId id && Equals(id);
        }

        public override int GetHashCode()
        {
            return Value;
        }
    }
}