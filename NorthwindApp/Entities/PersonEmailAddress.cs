using System;
using System.Collections.Generic;

namespace NorthwindApp.Entities
{
    public partial class PersonEmailAddress
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public bool? IsPersonal { get; set; }
        public int? PersonId { get; set; }

        public virtual Person? Person { get; set; }
    }
}
