using BonusCardManager.WpfUI.Commands;
using BonusCardManager.WpfUI.Models;
using System;
using System.Net;
using System.Windows.Input;

namespace BonusCardManager.WpfUI.ViewModels
{
    class SearchByCardNumberViewModel : BaseViewModel
    {
        private string cardNumber;
        public string CardNumber 
        {
            get { return cardNumber; } 
            set { cardNumber = value; OnPropertyChanged(nameof(CardNumber)); } 
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(nameof(Message)); }
        }

        private readonly NavigationViewModel navigationViewModel;

        public SearchByCardNumberViewModel(NavigationViewModel navigationViewModel) 
        {
            this.navigationViewModel = navigationViewModel;
        }
 
        public ICommand SearchByCardNumber
        {
            get
            {
                return new DelegateCommand(async (obj) =>
                {
                    if (!String.IsNullOrWhiteSpace(cardNumber) && Int32.TryParse(cardNumber, out int number))
                    {
                        Message = "Идет поиск...";

                        var response = await BonusCardModelService.GetBonusCardByCardNumber(number);

                        if (response.ResponseCode == HttpStatusCode.OK)
                        {
                            navigationViewModel.SelectedViewModel = new BonusCardViewModel(response.BonusCard);
                        }
                        else
                        {
                            Message = "Карточка №" + number + " не найдена";
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
