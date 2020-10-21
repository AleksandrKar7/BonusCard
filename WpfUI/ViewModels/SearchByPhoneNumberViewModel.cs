﻿using BonusCardManager.WpfUI.Commands;
using BonusCardManager.WpfUI.Models;
using BonusCardManager.WpfUI.Services;
using BonusCardManager.WpfUI.Services.Interfaces;
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
        private IBonusCardService bonusCardService;

        public SearchByPhoneNumberViewModel(NavigationViewModel navigationViewModel)
        {
            this.navigationViewModel = navigationViewModel;
            bonusCardService = new HTTPBonusCardService();
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

                        var bonusCard = await bonusCardService.GetBonusCardByPhoneNumber(phoneNumber);
                        if (bonusCard != null)
                        {
                            navigationViewModel.SelectedViewModel = new BonusCardViewModel(bonusCard);
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
