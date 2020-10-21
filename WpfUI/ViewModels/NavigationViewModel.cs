using BonusCardManager.WpfUI.Commands;
using System.Windows.Input;

namespace BonusCardManager.WpfUI.ViewModels
{
    class NavigationViewModel : BaseViewModel
    {
        private BaseViewModel selectedViewModel;
        public BaseViewModel SelectedViewModel 
        { 
            get { return selectedViewModel; }
            set { 
                selectedViewModel = value;
                OnPropertyChanged(nameof(selectedViewModel));
            } 
        }

        public ICommand UpdateViewCommand { 
            get 
            {
                return new DelegateCommand((param) =>
                {
                    if (param.ToString() == "SearchByCardNumber")
                    {
                        this.SelectedViewModel = new SearchByCardNumberViewModel(this);
                    }
                    if (param.ToString() == "SearchByPhoneNumber")
                    {
                        this.SelectedViewModel = new SearchByPhoneNumberViewModel(this);
                    }
                });
            } 
        }
    }
}
