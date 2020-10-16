using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BonusCardManager.DataAccess.Entities
{
    public class BonusCard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime ExpirationUTCDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.0, double.MaxValue)]
        public decimal Balance { get; set; }

        [Required]
        [MaxLength(999999)]
        public int Number { get; set; }

        [Required]
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
    }
}
