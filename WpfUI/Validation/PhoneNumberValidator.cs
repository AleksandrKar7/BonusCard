using BonusCardManager.WpfUI.Validation.Interfaces;
using System.Linq;

namespace BonusCardManager.WpfUI.Validation
{
    class PhoneNumberValidator : IValidator<string>
    {
        public bool IsValid(string phoneNumber)
        {
            return phoneNumber != null 
                   && phoneNumber.All(char.IsDigit) 
                   && phoneNumber.StartsWith("38") 
                   && phoneNumber.Length == 12;
        }
    }
}
