using System;
using System.Collections.Generic;

namespace PhoneManagement.Models
{
    public partial class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Token { get; set; }
        public DateTime ExpireTime { get; set; }
        public bool IsActive { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
