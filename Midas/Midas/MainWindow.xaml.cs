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
using Microsoft.Win32;
using Midas.DAL;
using Midas.DAL.SecuritiesDal;
using Midas.Model;
using Midas.Model.Constants;
using Midas.Model.DataSources;
using Midas.Model.Documents;
using Midas.Model.SecuritiesImport;
using Midas.ViewModels;
using Midas.Views;

namespace Midas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MidasContext>());
            //this.DataContext = new MainWindowViewModel();
            //DoSomething();
        }


        private void DoSomething()
        {
            try
            {
               //// 
               // using (var unitOfWork = new UnitOfWork(new MidasContext()))
               // {
               //     int count = unitOfWork.Securities.GetAll().Count();
               //     var security = unitOfWork.Securities.Find(x=>x.Ticker=="ZAYO");
               //     int test = 0;

               //     // unitOfWork.Securities.Add(new Security { Currency = "USD", Ticker = "AAPL", Name = "APPLE INC", Market = "NASDAQ", /*DateOfLatest10QFinancialStatement = new DateTime(2016,1,1)*/});
               //     //unitOfWork.Complete();
               //     //foreach (var security in unitOfWork.Securities.GetAll())
               //     //{
               //     //    string result = String.Format("{0}-{1}-{2}", security.Ticker, security.Name, security.Ticker);
               //     //    int test = 0;
               //     //}

               // }
              

            }
            catch (Exception exception)
            {
                string mess = exception.Message;
                throw;
            }

        }




        //private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        //{
        //    //var apikey = "279kg2utnbdvzztup2vjygte";
        //    //var source = new EdgarMarketDataFinancialStatementSource(apikey);
        //    //var results = await source.GetAnnualFinancialStatementsAsync("MSFT");//.ConfigureAwait(false);
        //    //int test = 0;
        //    var factory = new SecurityImporterFactory();
        //    var importer = factory.GetSecurityImporter(MarketConstants.Nyse);
        //    Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.DefaultExt = ".csv";
        //    var path = String.Empty;
        //    bool? result = openFileDialog.ShowDialog();
        //    if (result == true)
        //        path = openFileDialog.FileName;
        //    var securities = await importer.ImportSecuritiesAsync(path);
        //    if (securities == null)
        //        MessageBox.Show("Problem while importing securities.");
        //    var securitiesDal = new StatementDalFactory().GetSecurityDal().ImportSecurities(securities);

        //    MessageBox.Show("Import Finished");
        //}


      
    }
}
