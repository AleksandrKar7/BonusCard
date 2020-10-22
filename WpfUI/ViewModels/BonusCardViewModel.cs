using BonusCardManager.WpfUI.Commands;
using BonusCardManager.WpfUI.Models;
using BonusCardManager.WpfUI.Services;
using BonusCardManager.WpfUI.Services.Interfaces;
using System;
using System.Globalization;
using System.Windows.Input;

namespace BonusCardManager.WpfUI.ViewModels
{
    class BonusCardViewModel : BaseViewModel
    {
        private BonusCardModel bonusCard;
        private IBonusCardService bonusCardService;

        public BonusCardViewModel(BonusCardModel bonusCard)
        {
            this.bonusCard = bonusCard;
            bonusCardService = new HTTPBonusCardService();
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
                    var separator = new NumberFormatInfo { CurrencyDecimalSeparator = "," };
                    if (String.IsNullOrWhiteSpace(balanceChanges) 
                        || !Decimal.TryParse(balanceChanges.Replace(".", ","), NumberStyles.Currency, separator, out decimal amount))
                    {
                        Message = "Неверный формат числа";
                        return;
                    }

                    try
                    {
                        Message = "Запрос в обработке...";
                        var isOk = await bonusCardService.AccrualBalanceAsync(bonusCard.Id, amount);
                        if (!isOk)
                        {
                            Message = "Баланс карты не изменен";
                            return;
                        }

                        Message = "Обновление данных...";
                        var updatedBonusCard = await bonusCardService.GetBonusCardByCardNumber(bonusCard.Number);
                        if (updatedBonusCard != null)
                        {
                            bonusCard = updatedBonusCard;
                            RefreshViewProperties();
                            Message = "Готово";
                        }
                        else
                        {
                            Message = "Не удалось обновить данные";
                        }
                    }
                    catch
                    {
                        Message = "Ошибка при обращении к серверу";
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
                    var separator = new NumberFormatInfo { CurrencyDecimalSeparator = "," };
                    if (String.IsNullOrWhiteSpace(balanceChanges) 
                        || !Decimal.TryParse(balanceChanges.Replace(".", ","), NumberStyles.Currency, separator, out decimal amount))
                    {
                        Message = "Неверный формат числа";
                        return;
                    }
                    if (Balance - amount < 0)
                    {
                        Message = "Сумма на карте не достаточна";
                        return;
                    }

                    try
                    {
                        Message = "Запрос в обработке...";
                        var isOk = await bonusCardService.WriteOffBalanceAsync(bonusCard.Id, amount);
                        if (!isOk)
                        {
                            Message = "Баланс карты не изменен";
                            return;
                        }

                        Message = "Обновление данных...";
                        var updatedBonusCard = await bonusCardService.GetBonusCardByCardNumber(bonusCard.Number);
                        if (updatedBonusCard != null)
                        {
                            bonusCard = updatedBonusCard;
                            RefreshViewProperties();
                            Message = "Готово";
                        }
                        else
                        {
                            Message = "Ну удалось обновить данные";
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
