using BonusCardManager.ApplicationServices.Validation.Interfaces;
using BonusCardManager.DataAccess.Entities;
using System;
using System.Text;

namespace BonusCardManager.ApplicationServices.Validation
{
    class BonusCardValidator : IValidator<BonusCard>
    {
        public string Validate(BonusCard bonusCard)
        {
            StringBuilder errorMessageBuilder = new StringBuilder();

            if (bonusCard.ExpirationUTCDate.Date < DateTime.Now.Date)
            {
                errorMessageBuilder.AppendLine("Expiration date cannot be less than the current date");
            }
            if (bonusCard.Balance < 0)
            {
                errorMessageBuilder.AppendLine("Balance cannot be less than zero");
            }

            return errorMessageBuilder.ToString();
        }
    }
}
