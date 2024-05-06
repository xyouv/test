using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneManagement {
    [Table("RefreshTokens")]
    public class RefreshTokens {

        public int Id { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }
        public string? Token { get; set; }
        public DateTime ExpireTime { get; set; }
        public bool IsActive { get; set; }

    }
}
