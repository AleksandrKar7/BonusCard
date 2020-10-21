using System;

namespace BonusCardManager.WpfUI.Models
{
    class BonusCardModel
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
