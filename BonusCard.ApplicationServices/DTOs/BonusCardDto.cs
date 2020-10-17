using System;

namespace BonusCardManager.ApplicationServices.DTOs
{
    public class BonusCardDto
    {
        public int Id { get; set; }

        public DateTime ExpirationUTCDate { get; set; }

        public decimal Balance { get; set; }

        public int Number { get; set; }

        public int CustomerId { get; set; }

        public string CustomerFullName { get; set; }

        public string CustomerPhoneNumber { get; set; }
    }
}
