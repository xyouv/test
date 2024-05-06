using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneManagement {
    [Table("Phone")]
    public class Phone {

        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Name { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Brand { get; set; }

        [Column("Money")]
        public decimal? Price { get; set; }

        [DefaultValue(false)]
        public bool? IsActive { get; set; }

        [ForeignKey("userID")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
