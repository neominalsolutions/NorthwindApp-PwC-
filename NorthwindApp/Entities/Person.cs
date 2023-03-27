using System;
using System.Collections.Generic;

namespace NorthwindApp.Entities
{
    public partial class Person
    {
        public Person()
        {
            PersonEmailAddresses = new HashSet<PersonEmailAddress>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? IdentityNumber { get; set; }
        public short? Age { get; set; }

        public virtual ICollection<PersonEmailAddress> PersonEmailAddresses { get; set; }
    }
}
