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
using Microsoft.Win32;
using Midas.ViewModels;
using MahApps.Metro.Controls.Dialogs;

namespace Midas.Views
{
    /// <summary>
    /// Interaction logic for SecurityImportView.xaml
    /// </summary>
    public partial class SecurityImportView : UserControl
    {
        public SecurityImportView()
        {
            InitializeComponent();
            _viewModel= new ImportViewModel("Import Securities");
            this.DataContext = _viewModel;
            ImportDataGrid.ItemsSource = _viewModel.Securities;
            CountriesComboBox.ItemsSource = _viewModel.Countries;
            CountriesComboBox.SelectedIndex = 0;
            MarketCombobox.ItemsSource = _viewModel.Markets;
            CountriesComboBox.SelectedIndex = 0;
        }


        private readonly ImportViewModel _viewModel;

        private async void DownloadButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FileLocationTextBox.Text == string.Empty)
                {
                    await
                        ((MetroWindow) Window.GetWindow(this)).ShowMessageAsync("Securities Import",
                            "Please specify a file path.");
                    return;
                }
                bool success = await _viewModel.ImportSecurities();
                if (success)
                {
                    await
                        ((MetroWindow) Window.GetWindow(this)).ShowMessageAsync("Securities Import",
                            "Import successful!");
                }
                else
                {
                    await
                        ((MetroWindow) Window.GetWindow(this)).ShowMessageAsync("Securities Import",
                            "Importation failed.");

                }
            }
            catch (Exception exc)
            {
                await
                       ((MetroWindow)Window.GetWindow(this)).ShowMessageAsync("Securities Import",String.Format("Import Failed : {0}", exc.Message));
            }
            
        }

        private void BrowseFileLocationButton_OnClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".csv";
            var path = String.Empty;
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
                path = openFileDialog.FileName;
            _viewModel.PathFile = path;
        }

        private void MarketCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SelectedMarket=(string)MarketCombobox.SelectedItem;
        }
    }
}
