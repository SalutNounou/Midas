using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Midas.ViewModels;

namespace Midas
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        protected override void OnActivated(EventArgs e)
        {
            var mainWindowViewModel = new MainWindowViewModel();
            MainWindow.DataContext = mainWindowViewModel;
           
        }

      
    }
}
    

