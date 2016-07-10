using System.Runtime.InteropServices;
using log4net;
using log4net.Config;
using Midas.Model.DataSources;
using Midas.Model.MarketData;
using Midas.Source.Strategy;
using System.Configuration;
using System.Linq;
using Midas.DAL;

namespace Midas.Source
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        private static ISourceEngine _priceEngine;
        private static ISourceEngine _statementEngine;
        static void Main(string[] args)
        {
            BasicConfigurator.Configure();
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
            log.Info("Application Started");
            _priceEngine = new PriceEngine(new SecurityPriceRetrieverStrategy(new YahooMarketDataPriceSource()));
            string apiKey = ConfigurationManager.AppSettings["EdgarApiKey"];
            _statementEngine = new StatementEngine(new StatementRetrieverStrategy(new EdgarMarketDataFinancialStatementSource(apiKey)));
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
            log.Info("Exiting Application.");
            using (var unitOfWork = new UnitOfWork(new MidasContext()))
            {
                var rell = unitOfWork.Securities.Find(s => s.Ticker == "RELL").First();
                var statements = unitOfWork.FinancialStatements.Find(s => s.PrimarySymbol == "RELL");
                var latestStatement = statements.OrderBy(s => s.PeriodEnd).Last();
                var lastReceivedDate = statements.Max(s => s.ReceivedDate);
                var ncav = latestStatement.BalanceSheet.TotalCurrentAssets -
                           latestStatement.BalanceSheet.TotalCurrentLiabilities;

                var nbShares = rell.NbSharesOutstanding != 0
                    ? rell.NbSharesOutstanding
                    : rell.MarketCapitalisation/rell.Last;
                var ncavPerShare = ncav/nbShares;
                var discountOnNcav = (ncavPerShare - rell.Last)/ncavPerShare;

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
                    break;

                case CtrlTypes.CTRL_BREAK_EVENT:

                    log.Info("CTRL+BREAK received! Stopping Engine.");
                    _priceEngine.StopEngine();
                    _statementEngine.StopEngine();
                    break;

                case CtrlTypes.CTRL_CLOSE_EVENT:

                    log.Info("Program being closed! Stopping Engine.");
                    _priceEngine.StopEngine();
                    _statementEngine.StopEngine();
                    break;

                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                    log.Info("User is logging off! Stopping Engine.");
                    _priceEngine.StopEngine();
                    _statementEngine.StopEngine();
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