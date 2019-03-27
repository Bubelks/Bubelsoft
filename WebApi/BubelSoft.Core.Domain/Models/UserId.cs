namespace BubelSoft.Core.Domain.Models
{
    public struct UserId
    {
        public UserId(int value) 
            => Value = value;

        public int Value { get;  }

        public static bool operator ==(UserId id1, UserId id2) 
            => id1.Value == id2.Value;

        public static bool operator !=(UserId id1, UserId id2) 
            => !(id1 == id2);

        public bool Equals(UserId other) 
            => Value == other.Value;

        public override bool Equals(object obj) 
            => !ReferenceEquals(null, obj) && (obj is UserId id && Equals(id));

        public override int GetHashCode() 
            => Value;

        public override string ToString() 
            => Value.ToString();
    }
}