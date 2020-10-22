using System.Windows.Controls;
using System.Windows.Input;

namespace BonusCardManager.WpfUI.Views
{
    /// <summary>
    /// Interaction logic for ShowBonusCardView.xaml
    /// </summary>
    public partial class ShowBonusCardView : UserControl
    {
        public ShowBonusCardView()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == "." || e.Text == ",")
            {
                if (!((TextBox)sender).Text.Contains(".") && !((TextBox)sender).Text.Contains(","))
                {
                    approvedDecimalPoint = true;
                }
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
            {
                e.Handled = true;
            }
        }
    }
}
