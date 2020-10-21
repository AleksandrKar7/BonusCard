using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace BonusCardManager.WpfUI.Views
{
    /// <summary>
    /// Interaction logic for SearchByPhoneNumberView.xaml
    /// </summary>
    public partial class SearchByPhoneNumberView : UserControl
    {
        public SearchByPhoneNumberView()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = (regex.IsMatch(e.Text));
        }
    }
}
