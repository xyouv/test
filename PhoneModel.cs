using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PhoneManagement {
    public class PhoneModel {

        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Brand { get; set; }

        public decimal? Price { get; set; }

        public bool? IsActive { get; set; }

        public int UserId { get; set; }
    }
}
