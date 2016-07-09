using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Midas.DAL;
using Midas.DAL.SecuritiesDal;
using Midas.Model;
using Midas.Model.Constants;
using Midas.Model.SecuritiesImport;

namespace Midas.ViewModels
{
    public class ImportViewModel:ItemViewModel
    {
        public ObservableCollection<Security> Securities = new ObservableCollection<Security>();


        public ObservableCollection<string> Markets = new ObservableCollection<string>
        {
            Midas.Model.Constants.MarketConstants.Nyse,
            Midas.Model.Constants.MarketConstants.Amex,
            Midas.Model.Constants.MarketConstants.Nasdaq,
        };


        private string _selectedMarket;

        public string SelectedMarket
        {
            get
            {
                return _selectedMarket;
            }
            set
            {
                if (_selectedMarket != value)
                {
                    _selectedMarket = value;
                    OnPropertyChanged("SelectedMarket");
                }
            }
        }

        private string _pathFile;
        public string PathFile
        {
            get
            {
                return _pathFile;
            }
            set
            {
                if (_pathFile != value)
                {
                    _pathFile = value;
                    OnPropertyChanged("PathFile");
                }
            }
        }


        public ObservableCollection<string> Countries = new ObservableCollection<string> {"USA"};

        private string _selectedCountry;

        public string SelectedCountry
        {
            get
            {
                return _selectedCountry;
            }
            set
            {
                if (_selectedCountry == value) return;
                _selectedCountry = value;
                OnPropertyChanged("SelectedCountry");
            }
        }

        public ImportViewModel(string tabName):base(tabName)
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<MidasContext>());
            //using (var unitOfWork = new UnitOfWork(new MidasContext()))
            //{
            //    int count = unitOfWork.Securities.GetAll().Count();
            //    var security = unitOfWork.Securities.Find(x => x.Ticker == "ZAYO");
            //    int test = 0;

            //    // unitOfWork.Securities.Add(new Security { Currency = "USD", Ticker = "AAPL", Name = "APPLE INC", Market = "NASDAQ", /*DateOfLatestFinancialStatement = new DateTime(2016,1,1)*/});
            //    //unitOfWork.Complete();
            //    //foreach (var security in unitOfWork.Securities.GetAll())
            //    //{
            //    //    string result = String.Format("{0}-{1}-{2}", security.Ticker, security.Name, security.Ticker);
            //    //    int test = 0;
            //    //}

            //}
            RefreshSecurities();
        }

        public async Task<bool> ImportSecurities()
        {
            var factory = new SecurityImporterFactory();
            var importer = factory.GetSecurityImporter(SelectedMarket);
            var securities = await importer.ImportSecuritiesAsync(PathFile);
            var success =  SecurityDalFactory.GetInstance().GetSecurityDal().ImportSecurities(securities);
            RefreshSecurities();
            return success;
        }

        public void RefreshSecurities()
        {
            try
            {
                Securities.Clear();
                SecurityDalFactory.GetInstance().GetSecurityDal().GetAllSecurities().ToList().ForEach(Securities.Add);
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }


    }
}