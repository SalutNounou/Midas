using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Midas.Model.MarketData;
using Midas.Source.Strategy;

namespace Midas.Source
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        private static ISourceEngine _engine;
        static void Main(string[] args)
        {
            BasicConfigurator.Configure();
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
            log.Info("Application Started");
            _engine = new PriceEngine(new SecurityPriceRetrieverStrategy(new YahooMarketDataPriceSource()));
            while (_engine.ShouldWork)
            {
                _engine.DoCycle();
            }
            log.Info("Exiting Application.");
        }

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            // Put your own handler here
            switch (ctrlType)
            {
                case CtrlTypes.CTRL_C_EVENT:
                    log.Info("CTRL+C received! Stopping Engine.");
                    _engine.StopEngine();
                    break;

                case CtrlTypes.CTRL_BREAK_EVENT:

                    log.Info("CTRL+BREAK received! Stopping Engine.");
                    _engine.StopEngine();
                    break;

                case CtrlTypes.CTRL_CLOSE_EVENT:

                    log.Info("Program being closed! Stopping Engine.");
                    _engine.StopEngine();
                    break;

                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                    log.Info("User is logging off! Stopping Engine.");
                    _engine.StopEngine();
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