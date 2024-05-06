using System;
using System.Collections.Generic;

namespace PhoneManagement.Models
{
    public partial class User
    {
        public User()
        {
            Phones = new HashSet<Phone>();
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? VertificationToken { get; set; }
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }

        public virtual ICollection<Phone> Phones { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
