namespace BubelSoft.Core.Domain.Models
{
    public struct UserId
    {
        public UserId(int value)
        {
            Value = value;
        }

        public int Value { get;  }

        public static bool operator ==(UserId id1, UserId id2)
        {
            return id1.Value == id2.Value;
        }

        public static bool operator !=(UserId id1, UserId id2)
        {
            return !(id1 == id2);
        }

        public bool Equals(UserId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is UserId id && Equals(id);
        }

        public override int GetHashCode()
        {
            return Value;
        }
    }
}