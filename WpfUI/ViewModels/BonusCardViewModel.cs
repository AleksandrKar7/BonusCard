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
    class BonusCardViewModel : BaseViewModel
    {
        private BonusCardModel bonusCard;

        public BonusCardViewModel(BonusCardModel bonusCard)
        {
            this.bonusCard = bonusCard;
        }

        public string ExpirationUTCDate
        {
            get { return bonusCard.ExpirationUTCDate.ToShortDateString(); }
        }

        public decimal Balance
        {
            get { return bonusCard.Balance; }
        }

        public int Number
        {
            get { return bonusCard.Number; }
        }

        public string CustomerFullName
        {
            get { return bonusCard.CustomerFullName; }
        }

        public string CustomerPhoneNumber
        {
            get { return bonusCard.CustomerPhoneNumber; }
        }

        private string balanceChanges;
        public string BalanceChanges
        {
            get { return balanceChanges; }
            set { balanceChanges = value; OnPropertyChanged(nameof(BalanceChanges)); }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(nameof(Message)); }
        }

        public ICommand AccrualBonusCardBalance
        {
            get
            {
                return new DelegateCommand(async (obj) =>
                {

                    if (!String.IsNullOrWhiteSpace(balanceChanges) && Decimal.TryParse(balanceChanges, out decimal amount))
                    {

                        using (var httpClient = new HttpClient())
                        {
                            Message = "Запрос в обработке...";

                            var request = "https://localhost:44389/api/BonusCard/" + bonusCard.Id + "/Accrual/" + amount;

                            using (var response = await httpClient.PutAsync(request, null))
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    Message = "Готово...";
                                }
                                else
                                {
                                    Message = "Баланс карточки №" + Number + " не изменен";
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

        public ICommand WriteOffBonusCardBalance
        {
            get
            {
                return new DelegateCommand(async (obj) =>
                {

                    if (!String.IsNullOrWhiteSpace(balanceChanges) && Decimal.TryParse(balanceChanges, out decimal amount))
                    {
                        if(Balance - amount < 0)
                        {
                            Message = "Сумма на карточке не достаточна";
                            return;
                        }
                        using (var httpClient = new HttpClient())
                        {
                            Message = "Запрос в обработке...";
                            
                            var request = "https://localhost:44389/api/BonusCard/" + bonusCard.Id + "/WriteOff/" + amount;

                            using (var response = await httpClient.PutAsync(request, null))
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    Message = "Готово...";
                                }
                                else
                                {
                                    Message = "Баланс карточки №" + Number + " не изменен";
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
