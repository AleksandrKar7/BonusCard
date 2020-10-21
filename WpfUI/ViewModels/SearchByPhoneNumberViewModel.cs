using BonusCardManager.WpfUI.Commands;
using BonusCardManager.WpfUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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

                        using (var httpClient = new HttpClient())
                        {
                            Message = "Идет поиск...";

                            var request = "https://localhost:44389/api/BonusCard/ByPhoneNumber/" + phoneNumber;
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
                                    Message = "Карточка по номеру телефона " + phoneNumber + " не найдена";
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
