using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BonusCardManager.DataAccess.Entities
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(12)]
        public string PhoneNumber { get; set; }

        [ForeignKey("BonusCard")]
        public BonusCard BonusCard { get; set; }
    }
}
