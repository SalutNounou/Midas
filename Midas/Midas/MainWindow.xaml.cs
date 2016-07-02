using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using Midas.DAL;
using Midas.Model;
using Midas.Model.DataSources;
using Midas.Model.Documents;

namespace Midas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DoSomething();
        }


        private void DoSomething()
        {
            try
            {
                Database.SetInitializer(new DropCreateDatabaseAlways<MidasContext>());
                using (var unitOfWork = new UnitOfWork(new MidasContext()))
                {
                    

                   // unitOfWork.Securities.Add(new Security { Currency = "USD", Ticker = "AAPL", Name = "APPLE INC", Market = "NASDAQ", /*DateOfLatestFinancialStatement = new DateTime(2016,1,1)*/});
                    unitOfWork.Complete();
                    foreach (var security in unitOfWork.Securities.GetAll())
                    {
                        string result = String.Format("{0}-{1}-{2}", security.Ticker, security.Name, security.Ticker);
                        int test = 0;
                    }

                }
            }
            catch (Exception exception)
            {
                string mess = exception.Message;
                throw;
            }
           
        }


        

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var apikey = "279kg2utnbdvzztup2vjygte";
            var source = new EdgarMarketDataFinancialStatementSource(apikey);
            var results = await source.GetAnnualFinancialStatementsAsync("MSFT");//.ConfigureAwait(false);
            int test = 0;
        }
    }
}
