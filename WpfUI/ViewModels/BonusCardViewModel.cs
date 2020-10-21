using BonusCardManager.WpfUI.Commands;
using BonusCardManager.WpfUI.Models;
using System;
using System.Net;
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

        private void RefreshViewProperties()
        {
            OnPropertyChanged(nameof(ExpirationUTCDate));
            OnPropertyChanged(nameof(Balance));
            OnPropertyChanged(nameof(Number));
            OnPropertyChanged(nameof(CustomerFullName));
            OnPropertyChanged(nameof(CustomerPhoneNumber));
        }

        public ICommand AccrualBonusCardBalance
        {
            get
            {
                return new DelegateCommand(async (obj) =>
                {

                    if (!String.IsNullOrWhiteSpace(balanceChanges) && Decimal.TryParse(balanceChanges, out decimal amount))
                    {
                        Message = "Запрос в обработке...";
                        var responseCode = await BonusCardModelService.AccrualBalanceAsync(bonusCard.Id, amount);
                        if (responseCode == HttpStatusCode.OK)
                        {
                            Message = "Готово. Обновление данных...";
                            var result = await BonusCardModelService.GetBonusCardByCardNumber(bonusCard.Number);
                            if(result.ResponseCode == HttpStatusCode.OK)
                            {
                                bonusCard = result.BonusCard;
                                RefreshViewProperties();
                                Message = "Готово";
                            }
                            else
                            {
                                Message = "Ну удалось обновить данные";
                            }
                        }
                        else
                        {
                            Message = "Баланс карточки №" + Number + " не изменен";
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
                        Message = "Запрос в обработке...";

                        var responseCode = await BonusCardModelService.WriteOffBalanceAsync(bonusCard.Id, amount);
                        if (responseCode == HttpStatusCode.OK)
                        {
                            Message = "Готово. Обновление данных...";
                            var result = await BonusCardModelService.GetBonusCardByCardNumber(bonusCard.Number);
                            if (result.ResponseCode == HttpStatusCode.OK)
                            {
                                bonusCard = result.BonusCard;
                                RefreshViewProperties();
                                Message = "Готово";
                            }
                            else
                            {
                                Message = "Ну удалось обновить данные";
                            }
                        }
                        else
                        {
                            Message = "Баланс карточки №" + Number + " не изменен";
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
