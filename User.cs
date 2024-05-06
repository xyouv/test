using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneManagement {
    [Table("User")]
    public class User {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string? VertificationToken { get; set; }
        public string Email { get; set; }
        //public string? ResetToken { get; set; }
        //public DateTime ResetTokenExpire { get; set; }

        public bool IsActive { get; set; }
        public ICollection<Phone> Phones { get; set; }
        public ICollection<RefreshTokens> RefreshTokens { get; set; }
    }
}
