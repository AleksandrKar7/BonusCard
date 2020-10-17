using BonusCardManager.ApplicationServices.DTOs;
using BonusCardManager.ApplicationServices.Validation.Interfaces;
using System;
using System.Text;

namespace BonusCardManager.ApplicationServices.Validation
{
    class BonusCardValidator : IValidator<BonusCardDto>
    {
        public string Validate(BonusCardDto bonusCard)
        {
            if (bonusCard == null)
            {
                return "bonusCard can not be null";
            }

            StringBuilder errorMessageBuilder = new StringBuilder();

            if (bonusCard.ExpirationUTCDate.Date < DateTime.Now.Date)
            {
                errorMessageBuilder.AppendLine("Expiration date cannot be less than the current date");
            }
            if (bonusCard.Balance < 0)
            {
                errorMessageBuilder.AppendLine("Balance cannot be less than zero");
            }
            if (bonusCard.CustomerId <= 0)
            {
                errorMessageBuilder.AppendLine("CustomerId must be above zero");
            }

            return errorMessageBuilder.ToString();
        }
    }
}
