using System;

namespace NETCoreRemoteServices.Tests.Contracts
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Mail { get; set; }
        public DateTime BirthDate { get; set; }

        protected bool Equals(User other)
        {
            return  string.Equals(FirstName, other.FirstName, StringComparison.OrdinalIgnoreCase) && 
                    string.Equals(LastName, other.LastName, StringComparison.OrdinalIgnoreCase) && 
                    Age == other.Age && 
                    string.Equals(Mail, other.Mail, StringComparison.OrdinalIgnoreCase) && 
                    BirthDate.Equals(other.BirthDate);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (FirstName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(FirstName) : 0);
                hashCode = (hashCode * 397) ^ (LastName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(LastName) : 0);
                hashCode = (hashCode * 397) ^ Age;
                hashCode = (hashCode * 397) ^ (Mail != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Mail) : 0);
                hashCode = (hashCode * 397) ^ BirthDate.GetHashCode();
                return hashCode;
            }
        }
    }
}
