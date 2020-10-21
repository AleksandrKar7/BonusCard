using BonusCardManager.WpfUI.Commands;
using BonusCardManager.WpfUI.Models;
using System;
using System.Linq;
using System.Net;
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

        public SearchByPhoneNumberViewModel(NavigationViewModel navigationViewModel)
        {
            this.navigationViewModel = navigationViewModel;
        }

        public ICommand SearchByPhoneNumber
        {
            get
            {
                return new DelegateCommand(async (obj) =>
                {
                    if (!String.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.All(char.IsDigit))
                    {
                        Message = "Идет поиск...";

                        var response = await BonusCardModelService.GetBonusCardByPhoneNumber(phoneNumber);
                        if (response.ResponseCode == HttpStatusCode.OK)
                        {
                            navigationViewModel.SelectedViewModel = new BonusCardViewModel(response.BonusCard);
                        }
                        else
                        {
                            Message = "Карточка по номеру телефона " + phoneNumber + " не найдена";
                        }
                    }
                    else
                    {
                        Message = "Доступны только числа";
                    }
                });
            }
        }

    }
}
