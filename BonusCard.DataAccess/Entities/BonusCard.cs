using System;
using System.ComponentModel.DataAnnotations;

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

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
