using System;

namespace BubelSoft.Core.Domain.Models
{
    public struct BuildingId
    {
        public BuildingId(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
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

        public bool Equals(BuildingId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is BuildingId && Equals((BuildingId)obj);
        }

        public override int GetHashCode()
        {
            return Value;
        }
    }
}