using BonusCardManager.WpfUI.Commands;
using BonusCardManager.WpfUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
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

                        using (var httpClient = new HttpClient())
                        {
                            Message = "Идет поиск...";

                            var request = "https://localhost:44389/api/BonusCard/ByCardNumber/" + number;
                            using (var response = await httpClient.GetAsync(request))
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    string apiResponse = await response.Content.ReadAsStringAsync();

                                    var bonusCard = JsonConvert.DeserializeObject<BonusCardModel>(apiResponse);

                                    navigationViewModel.SelectedViewModel = new BonusCardViewModel(bonusCard);
                                }
                                else
                                {
                                    Message = "Карточка №" + number + " не найдена";
                                }
                            }
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
