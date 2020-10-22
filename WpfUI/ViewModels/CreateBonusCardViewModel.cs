using BonusCardManager.WpfUI.Commands;
using BonusCardManager.WpfUI.Models;
using BonusCardManager.WpfUI.Services;
using BonusCardManager.WpfUI.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BonusCardManager.WpfUI.ViewModels
{
    class CreateBonusCardViewModel : BaseViewModel
    {
        private DateTime expirationDate;
        public DateTime ExpirationDate
        {
            get { return expirationDate; }
            set
            {
                expirationDate = value;
                OnPropertyChanged(nameof(ExpirationDate));
            }
        }

        private CustomerModel selectedCustomer;
        public CustomerModel SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(nameof(Message)); }
        }

        private ObservableCollection<CustomerModel> customers;
        public ObservableCollection<CustomerModel> Customers 
        { 
            get { return customers; }
            set
            {
                customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        private readonly NavigationViewModel navigationViewModel;
        private IBonusCardService bonusCardService;
        private ICustomerService customerService;
        public CreateBonusCardViewModel(NavigationViewModel navigationViewModel)
        {
            this.navigationViewModel = navigationViewModel;
            bonusCardService = new HTTPBonusCardService();
            customerService = new HTTPCustomerService();

            expirationDate = DateTime.Now.Date;
        }

        public async void InitCustomers()
        {
            Message = "Загрузка списка клиентов";
            var nonCardCustomers = await customerService.GetNonCardCustomers();
            if(nonCardCustomers == null)
            {
                Message = "Клиенты без карты не найдены";
                return;
            }

            Customers = new ObservableCollection<CustomerModel>(nonCardCustomers);
            Message = null;
        }

        public ICommand CreateBonusCard
        {
            get
            {
                return new DelegateCommand(async (obj) =>
                {
                    Message = null;
                    if (expirationDate == null || expirationDate.Date < DateTime.Now.Date)
                    {
                        Message = "Дата некорректна";
                        return;
                    }
                    if (selectedCustomer == null)
                    {
                        Message = "Клиент не выбран";
                        return;
                    }

                    var bonusCard = new BonusCardModel()
                    {
                        ExpirationUTCDate = expirationDate,
                        CustomerId = selectedCustomer.Id,
                        CustomerFullName = selectedCustomer.FullName,
                        CustomerPhoneNumber = selectedCustomer.PhoneNumber
                    };

                    try
                    {
                        Message = "Идет создание карты";
                        var newBonusCard = await bonusCardService.CreateBonusCard(bonusCard);
                        if (newBonusCard != null)
                        {
                            navigationViewModel.SelectedViewModel = new BonusCardViewModel(newBonusCard);
                        }
                        else
                        {
                            Message = "Ошибка. Карта не была создана";
                        }
                    } 
                    catch
                    {
                        Message = "Ошибка при обращению к серверу";
                    }
                }); 
            }
        }
    }
}
