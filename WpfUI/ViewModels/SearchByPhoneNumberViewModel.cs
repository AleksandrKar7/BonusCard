using BonusCardManager.WpfUI.Commands;
using BonusCardManager.WpfUI.Services;
using BonusCardManager.WpfUI.Services.Interfaces;
using BonusCardManager.WpfUI.Validation;
using BonusCardManager.WpfUI.Validation.Interfaces;
using System.Windows.Input;

namespace BonusCardManager.WpfUI.ViewModels
{
    class SearchByPhoneNumberViewModel : BaseViewModel
    {
        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; OnPropertyChanged(nameof(PhoneNumber)); }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(nameof(Message)); }
        }

        private readonly NavigationViewModel navigationViewModel;
        private readonly IBonusCardService bonusCardService;
        private readonly IValidator<string> phoneNumberValidator;
        public SearchByPhoneNumberViewModel(NavigationViewModel navigationViewModel)
        {
            this.navigationViewModel = navigationViewModel;
            bonusCardService = new HTTPBonusCardService();
            phoneNumberValidator = new PhoneNumberValidator();
        }

        public ICommand SearchByPhoneNumber
        {
            get
            {
                return new DelegateCommand(async (obj) =>
                {
                    if (!phoneNumberValidator.IsValid(phoneNumber))
                    {
                        Message = "Формат: 38 xxx xxx xx xx";
                        return;
                    }

                    try
                    {
                        Message = "Идет поиск...";
                        var bonusCard = await bonusCardService.GetBonusCardByPhoneNumber(phoneNumber);
                        if (bonusCard != null)
                        {
                            navigationViewModel.SelectedViewModel = new BonusCardViewModel(bonusCard);
                        }
                        else
                        {
                            Message = "Карта не найдена";
                        }
                    }
                    catch
                    {
                        Message = "Ошибка при обращении к серверу";
                    }
                });
            }
        }

    }
}
