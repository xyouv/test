using System;
using System.Collections.Generic;

namespace PhoneManagement.Models
{
    public partial class Phone
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public decimal? Money { get; set; }
        public bool? IsActive { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
