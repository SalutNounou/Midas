using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Midas.DAL;
using Midas.DAL.SecuritiesDal;
using Midas.Model;
using Midas.Model.Documents;

namespace Midas.Engines.Engines
{
    public class NcavEngine : ISourceEngine
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NcavEngine));

        private const int BufferSecuritySize = 50;

        public NcavEngine()
        {
            ShouldWork = true;
        }


        public bool ShouldWork { get; private set; }

        public void DoCycle()
        {
            Log.Info("Starting Ncav Engine Cycle");
            var securitiesToHandle = SecurityDalFactory.GetInstance().GetSecurityDal().GetAllSecurities().Where(x => x.IsNotADuplicate()).Where(x => x.HasStatements()).Where(x=>x.HasNotNullMarketCap()).Where(x=>x.IsCalculusOnNcavOutDated()).Take(BufferSecuritySize);
            var toHandle = securitiesToHandle as IList<Security> ?? securitiesToHandle.ToList();
            if (!toHandle.Any())
            {
                Log.Info("Securities up to date. Stopping the Engine.");
                ShouldWork = false;
            }
            securitiesToHandle = CalculateNcavOnSecurities(toHandle);
            RefreshSecurities(securitiesToHandle);
            Log.Info("Price Ncav Cycle Ended.");
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
                            securityDb.DebtRatio = security.DebtRatio;
                            securityDb.AcquirersMultiple = security1.AcquirersMultiple;
                            securityDb.DateOfLatestCalculusOnAcquirersMultiple =
                                security1.DateOfLatestCalculusOnAcquirersMultiple;
                            securityDb.OperatingEarnings = security1.OperatingEarnings;
                            securityDb.EnterpriseValue = security1.EnterpriseValue;
                        }
                        Log.Info(String.Format("Saving Security {0} with Ncav per share {1}, Discount on Ncav {2} and Acquirer's Multiple {3}", security.Ticker, security.NcavPerShare, security.DiscountOnNcav,security.AcquirersMultiple));
                        unitOfWork.Complete();
                    }
                }
            }
            catch (Exception exc)
            {
                Log.Error(string.Format("{0}-{1}-{2}", exc.Message, exc.StackTrace, exc.InnerException));
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
                        var financialStatements = statements as IList<FinancialStatement> ?? statements.ToList();
                        var latestStatement = financialStatements.OrderBy(s => s.PeriodEnd).Last();

                        CalculateNcavAndDiscount(latestStatement, security);

                        //Acquirer's multiple

                        CalculateAcquirersMultiple(financialStatements, security);
                    }
                }
                return securitiesToCalculate;
            }
            catch (Exception exc)
            {
                Log.Error(string.Format("{0}-{1}-{2}", exc.Message, exc.StackTrace, exc.InnerException));
            }
            return new List<Security>();
        }

        private static void CalculateNcavAndDiscount(FinancialStatement latestStatement, Security security)
        {
            var ncav = latestStatement.BalanceSheet.TotalCurrentAssets -
                       latestStatement.BalanceSheet.TotalLiabilities;

            //Ncav
            var totalAsset = latestStatement.BalanceSheet.TotalAssets;
            var totalEquity = latestStatement.BalanceSheet.TotalStockHolderEquity;
            var cashAndEquivalent = latestStatement.BalanceSheet.CashAndCashEquivalent;
            if ((totalAsset - cashAndEquivalent) != 0)
                security.DebtRatio = totalEquity/(totalAsset - cashAndEquivalent);

            var nbShares = security.NbSharesOutstanding != 0
                ? security.NbSharesOutstanding
                : security.MarketCapitalisation/security.Last;
            if (nbShares <= 0)
                throw new Exception(string.Format("Number of shares is null or negative for Security : {0}", security.Ticker));
            security.NcavPerShare = ncav/nbShares;
            if (security.NcavPerShare > 0)
            {
                security.DiscountOnNcav = (ncav/nbShares - security.Last)/(ncav/nbShares);
            }
            security.DateOfLatestCalculusOnNav = DateTime.Today;
        }

        private static void CalculateAcquirersMultiple(IList<FinancialStatement> financialStatements, Security security)
        {
            //if (financialStatements.All(s => s.FormType == "10-Q" || s.FormType.StartsWith("S-1") ||s.FormType=="8-K" || s.FormType.StartsWith("424")|| s.FormType.StartsWith("10-KT")))  return;
            if (!financialStatements.Any(s => s.FormType == "10-K" || s.FormType == "40-F" || s.FormType == "20-F")) return;
              var latestAnnualStatement =
                financialStatements.Where(s => s.FormType == "10-K" || s.FormType == "40-F" || s.FormType == "20-F")
                    .OrderBy(s => s.PeriodEnd)
                    .Last();
            
            var nbShares = security.NbSharesOutstanding != 0
                          ? security.NbSharesOutstanding
                          : security.MarketCapitalisation / security.Last;
            var marketCap = security.Last*nbShares;
            var cashAndEquivalent = latestAnnualStatement.BalanceSheet.CashAndCashEquivalent;
           

            var preferredEquity = latestAnnualStatement.BalanceSheet.PreferredStock;
            var nonControllingInterest = latestAnnualStatement.BalanceSheet.OtherEquity;
            var totalLiabilities = latestAnnualStatement.BalanceSheet.TotalLiabilities;
            security.EnterpriseValue = marketCap + preferredEquity + nonControllingInterest +
                                       totalLiabilities - cashAndEquivalent;
            var revenue = latestAnnualStatement.IncomeStatement.TotalRevenue;
            var costOgfGoodsSold = latestAnnualStatement.IncomeStatement.CostOfRevenue;
            var sga = latestAnnualStatement.IncomeStatement.SellingGeneralAdministrativeExpense;
            var depreciationAndAmortization = latestAnnualStatement.CashFlowStatement.CfDepreciationAmortization;
            security.OperatingEarnings = revenue - (costOgfGoodsSold + sga + depreciationAndAmortization);
            if (Math.Abs((decimal) security.OperatingEarnings) > 0)
            {
                security.AcquirersMultiple = security.EnterpriseValue/security.OperatingEarnings;
            }
            security.DateOfLatestCalculusOnAcquirersMultiple = DateTime.Today;
        }


        public void StopEngine()
        {
            ShouldWork = false;
        }
    }
}