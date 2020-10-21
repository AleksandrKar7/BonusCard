using BonusCardManager.WpfUI.Commands;
using BonusCardManager.WpfUI.Models;
using BonusCardManager.WpfUI.Services;
using BonusCardManager.WpfUI.Services.Interfaces;
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
        private IBonusCardService bonusCardService;

        public SearchByCardNumberViewModel(NavigationViewModel navigationViewModel) 
        {
            this.navigationViewModel = navigationViewModel;
            bonusCardService = new HTTPBonusCardService();
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

                        var bonusCard = await bonusCardService.GetBonusCardByCardNumber(number);

                        if (bonusCard != null)
                        {
                            navigationViewModel.SelectedViewModel = new BonusCardViewModel(bonusCard);
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
