using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Midas.DAL;
using Midas.DAL.SecuritiesDal;
using Midas.Engines.Strategy;
using Midas.Model;

namespace Midas.Engines.Engines
{
    public class PriceEngine : ISourceEngine
    {
        private readonly IPriceRetrieverStrategy _priceStrategy;

        public PriceEngine(IPriceRetrieverStrategy priceStrategy)
        {
            _priceStrategy = priceStrategy;
            ShouldWork = true;
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(PriceEngine));

        private const int BufferSecuritySize = 100;

        public bool ShouldWork { get; private set; }

        public void DoCycle()
        {
            Log.Info("Starting Price Engine Cycle");
            var securitiesToHandle = SecurityDalFactory.GetInstance().GetSecurityDal().GetAllSecurities().Where(x=>x.IsNotADuplicate()).Where(x=>x.LastPriceIsTooOld()).Where(x=>x.HasNotTooManyFailedAttempts()).Take(BufferSecuritySize);
            var toHandle = securitiesToHandle as IList<Security> ?? securitiesToHandle.ToList();
            if (!toHandle.Any())
            {
                Log.Info("Securities up to date. Stopping the Engine.");
                ShouldWork = false;
            }
            securitiesToHandle=_priceStrategy.RetrievePriceForSecurities(toHandle);
            Log.Info("Prices retrieved. Saving.");
            RefreshSecurities(securitiesToHandle);
            Log.Info("Price Engine Cycle Ended.");
        }

        private static void RefreshSecurities(IEnumerable<Security> securities)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new MidasContext()))
                {
                    foreach (var security in securities)
                    {
                        var security1 = security;
                        var securityDb = unitOfWork.Securities.Find(s => s.Ticker == security1.Ticker).FirstOrDefault();
                        if (securityDb != null)
                        {
                           
                            if (security1.Last == 0)
                                securityDb.NbOfFailedAttemptsToGetPrices++;
                            else if (DateTime.Today.ToOADate() - securityDb.DateOfLatestPrice.ToOADate() > 1 && securityDb.DateOfLatestPrice > new DateTime(1900, 1, 1))
                                securityDb.NbOfFailedAttemptsToGetPrices = 0;
                            securityDb.Last = security1.Last;
                            securityDb.NbSharesOutstanding = security1.NbSharesOutstanding;
                            securityDb.DateOfLatestPrice = security1.DateOfLatestPrice;
                            securityDb.MarketCapitalisation = security1.MarketCapitalisation;
                        }
                        Log.Info(String.Format("Saving Security {0} with price {1} and Nb of outstanding shares {2} and Market Cap {3}", security.Ticker, security.Last, security.NbSharesOutstanding, security.MarketCapitalisation));
                        unitOfWork.Complete();
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(string.Format("{0} - {1}", exception.Message, exception.StackTrace));
            }
        }

        public void StopEngine()
        {
            ShouldWork = false;
        }
    }




}