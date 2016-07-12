using System;
using System.Runtime.InteropServices;
using log4net;
using log4net.Config;
using Midas.Model.DataSources;
using Midas.Model.MarketData;
using Midas.Source.Strategy;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Midas.DAL;
using Midas.DAL.SecuritiesDal;

namespace Midas.Source
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        private static ISourceEngine _priceEngine;
        private static ISourceEngine _statementEngine;
        private static ISourceEngine _ncavEngine;
        static void Main(string[] args)
        {
            BasicConfigurator.Configure();
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
            log.Info("Application Started");
            _priceEngine = new PriceEngine(new SecurityPriceRetrieverStrategy(new YahooMarketDataPriceSource()));
            string apiKey = ConfigurationManager.AppSettings["EdgarApiKey"];
            _statementEngine = new StatementEngine(new StatementRetrieverStrategy(new EdgarMarketDataFinancialStatementSource(apiKey)));
            _ncavEngine = new NcavEngine();
            log.Info("Starting to retrieve prices");
            while (_priceEngine.ShouldWork)
            {
                _priceEngine.DoCycle();
            }
            log.Info("Prices retrieved.");
            log.Info("Starting to retrieve financial statements");
            while (_statementEngine.ShouldWork)
            {
                _statementEngine.DoCycle();
            }
            log.Info("Financial statements retrieved.");
            log.Info("Starting to Update Ncav and discounts on Ncav");
            while (_ncavEngine.ShouldWork)
            {
                _ncavEngine.DoCycle();
            }
            log.Info("Calculus on Net current asset value done.");
            log.Info("Exiting Application.");
            using (var unitOfWork = new UnitOfWork(new MidasContext()))
            {
                var securitiesToInvest =
                    SecurityDalFactory.GetInstance()
                        .GetSecurityDal()
                        .GetAllSecurities()
                        .Where(x => x.DiscountOnNcav > (Decimal)0.33).OrderBy(x=>x.DiscountOnNcav).Reverse()
                        .ToList();
                foreach (var security in securitiesToInvest)
                {
                    log.Info(string.Format("Found security {0} with discount on Nav : {1}", security.Ticker, security.DiscountOnNcav));
                }
                log.Info(string.Format("Total : {0}", securitiesToInvest.Count));
                Console.ReadLine();
            }
            
        }

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            // Put your own handler here
            switch (ctrlType)
            {
                case CtrlTypes.CTRL_C_EVENT:
                    log.Info("CTRL+C received! Stopping Engine.");
                    _priceEngine.StopEngine();
                    _statementEngine.StopEngine();
                    _ncavEngine.StopEngine();
                    break;

                case CtrlTypes.CTRL_BREAK_EVENT:

                    log.Info("CTRL+BREAK received! Stopping Engine.");
                    _priceEngine.StopEngine();
                    _statementEngine.StopEngine();
                    _ncavEngine.StopEngine();
                    break;

                case CtrlTypes.CTRL_CLOSE_EVENT:

                    log.Info("Program being closed! Stopping Engine.");
                    _priceEngine.StopEngine();
                    _statementEngine.StopEngine();
                    _ncavEngine.StopEngine();
                    break;

                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                    log.Info("User is logging off! Stopping Engine.");
                    _priceEngine.StopEngine();
                    _statementEngine.StopEngine();
                    _ncavEngine.StopEngine();
                    break;
            }
            return true;
        }
        
        #region unmanaged
        // Declare the SetConsoleCtrlHandler function
        // as external and receiving a delegate.

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        // A delegate type to be used as the handler routine
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // An enumerated type for the control messages
        // sent to the handler routine.
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        #endregion

    }


}