using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using log4net.Util;
using Midas.DAL;
using Midas.DAL.SecuritiesDal;
using Midas.Model;

namespace Midas.Source
{
    public class NcavEngine : ISourceEngine
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NcavEngine));

        private const int BufferSecuritySize = 50;

        public NcavEngine()
        {
            ShouldWork = true;
        }


        public bool ShouldWork { get; private set; }

        public void DoCycle()
        {
            log.Info("Starting Ncav Engine Cycle");
            var securitiesToHandle = SecurityDalFactory.GetInstance().GetSecurityDal().GetAllSecurities().Where(x => x.IsNotADuplicate()).Where(x => x.HasStatements()).Where(x=>x.IsCalculusOnNcavOutDated()).Take(BufferSecuritySize);
            var toHandle = securitiesToHandle as IList<Security> ?? securitiesToHandle.ToList();
            if (!toHandle.Any())
            {
                log.Info("Securities up to date. Stopping the Engine.");
                ShouldWork = false;
            }
            securitiesToHandle = CalculateNcavOnSecurities(toHandle);
            RefreshSecurities(securitiesToHandle);
            log.Info("Price Ncav Cycle Ended.");
        }

        private void RefreshSecurities(IEnumerable<Security> securities)
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
                            securityDb.NcavPerShare = security1.NcavPerShare;
                            securityDb.DiscountOnNcav = security1.DiscountOnNcav;
                            securityDb.DateOfLatestCalculusOnNav = security.DateOfLatestCalculusOnNav;
                        }
                        log.Info(String.Format("Saving Security {0} with Ncav per share {1} and Discount on Ncav {2}", security.Ticker, security.NcavPerShare, security.DiscountOnNcav));
                        unitOfWork.Complete();
                    }
                }
            }
            catch (Exception exc)
            {
                log.Error(string.Format("{0}-{1}-{2}", exc.Message, exc.StackTrace, exc.InnerException));
            }   
        }

        private IEnumerable<Security> CalculateNcavOnSecurities(IEnumerable<Security> securities)
        {
            try
            {
                var securitiesToCalculate = securities as IList<Security> ?? securities.ToList();
                foreach (var security in securitiesToCalculate)
                {
                    using (var unitOfWork = new UnitOfWork(new MidasContext()))
                    {
                        var security1 = security;
                        var statements = unitOfWork.FinancialStatements.Find(s => s.PrimarySymbol == security1.Ticker);
                        var latestStatement = statements.OrderBy(s => s.PeriodEnd).Last();
                        var ncav = latestStatement.BalanceSheet.TotalCurrentAssets -
                                   latestStatement.BalanceSheet.TotalCurrentLiabilities;
                        
                        var nbShares = security.NbSharesOutstanding != 0
                            ? security.NbSharesOutstanding
                            : security.MarketCapitalisation / security.Last;
                        if(nbShares<=0) throw new Exception(string.Format("Number of shares is null or negative for Security : {0}", security.Ticker));
                        var ncavPerShare = ncav / nbShares;
                        security.NcavPerShare = ncav / nbShares;
                        if (security.NcavPerShare > 0)
                        {
                            security.DiscountOnNcav = (ncav/nbShares - security.Last)/(ncav/nbShares);
                        }
                        security.DateOfLatestCalculusOnNav = DateTime.Today;

                    }
                }
                return securitiesToCalculate;
            }
            catch (Exception exc)
            {
                log.Error(string.Format("{0}-{1}-{2}", exc.Message, exc.StackTrace, exc.InnerException));
            }
            return new List<Security>();
        }


        public void StopEngine()
        {
            ShouldWork = false;
        }
    }
}