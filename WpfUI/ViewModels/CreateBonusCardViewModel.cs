using BonusCardManager.WpfUI.Commands;
using BonusCardManager.WpfUI.Models;
using BonusCardManager.WpfUI.Services;
using BonusCardManager.WpfUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private string customerSearch;
        public string CustomerSearch
        {
            get { return customerSearch; }
            set 
            { 
                customerSearch = value; 
                OnPropertyChanged(nameof(CustomerSearch));
                SortedCustomers = new ObservableCollection<CustomerModel>(SearchCustomers(customers, customerSearch));
            }
        }

        private IEnumerable<CustomerModel> customers;
        private ObservableCollection<CustomerModel> sortedCustomers;
        public ObservableCollection<CustomerModel> SortedCustomers 
        { 
            get { return sortedCustomers; }
            set
            {
                sortedCustomers = value;
                OnPropertyChanged(nameof(SortedCustomers));
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
            Message = "Загрузка списка клиентов...";
            try
            {
                var nonCardCustomers = await customerService.GetNonCardCustomers();
                if (nonCardCustomers == null)
                {
                    Message = "Клиенты без карты не найдены";
                    return;
                }

                customers = nonCardCustomers;
                SortedCustomers = new ObservableCollection<CustomerModel>(nonCardCustomers);
                Message = null;
            }
            catch
            {
                Message = "Ошибка при обращении к серверу";
            }
           
        }

        public IEnumerable<CustomerModel> SearchCustomers(IEnumerable<CustomerModel> customers, string searchValue)
        {
            return customers.Where(c => (c.FullName.Contains(searchValue) || c.PhoneNumber.StartsWith(searchValue)));
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
                        Message = "Идет создание карты...";
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
                        Message = "Ошибка при обращении к серверу";
                    }
                }); 
            }
        }
    }
}
