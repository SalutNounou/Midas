using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Midas.ViewModels;

namespace Midas.Views
{
    /// <summary>
    /// Interaction logic for acquirersMultipleView.xaml
    /// </summary>
    public partial class AcquirersMultipleView : UserControl
    {
        public AcquirersMultipleView()
        {
            _viewModel = new AcquirersMultipleViewModel("Acquirer's Multiple");
            InitializeComponent();
            this.DataContext = _viewModel;
            TextBoxCap.Text = _viewModel.SelectedMarketCapitalization.ToString();
            AcquirersMultipleGrid.ItemsSource = _viewModel.AcquirersMultipleSecurityViewModels;
        }

        private readonly AcquirersMultipleViewModel _viewModel;

        private void GoButton_OnClick(object sender, RoutedEventArgs e)
        {
           _viewModel.RefreshSecurities();
        }

        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Int64 value = 0;
            try
            {
               value= Convert.ToInt64(TextBoxCap.Text);
            }
            catch (Exception)
            {

                await ((MetroWindow)Window.GetWindow(this)).ShowMessageAsync("Acquirer's Multiple", "Unable to understand Market Capitalization Value");
                return;
            }
            _viewModel.SelectedMarketCapitalization = value;
        }
    }
}
