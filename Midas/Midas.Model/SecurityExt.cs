using System;
using System.Linq;

namespace Midas.Model
{
    public static class SecurityExt
    {
        public static bool IsCalculusOnNcavOutDated(this Security sec)
        {
            var today = DateTime.Today;
            return (today.ToOADate() - sec.DateOfLatestCalculusOnNav.ToOADate()) >= 1;
        }


        public static bool StatementsAreUpToDate(this Security sec)
        {
            var today = DateTime.Today;
            if (!sec.Has10K && !sec.Has10Q &&!sec.Has20F && !sec.Has40F) return false;
            if(sec.Has10Q)
                if (sec.DateOfLatest10QFinancialStatement.ToOADate() - today.ToOADate() < 100) return true;
            if (sec.Has10K)
                if (sec.DateOfLatest10KFinancialStatement.ToOADate() - today.ToOADate() < 366) return true;
            if (sec.Has20F)
                if (sec.DateOfLatest20FFinancialStatement.ToOADate() - today.ToOADate() < 366) return true;
            if (!sec.Has40F) return false;
            return sec.DateOfLatest40FFinancialStatement.ToOADate() - today.ToOADate() < 366;
        }


        public static bool HasStatements(this Security sec)
        {
            return sec.Has10K || sec.Has10Q || sec.Has20F || sec.Has40F;
        }


        public static bool IsNotADuplicate(this Security security)
        {
            return !security.Ticker.Contains('^') && !security.Ticker.Contains('.') && !security.Ticker.Contains('$');
        }


        public static bool LastPriceIsTooOld(this Security security)
        {
            var today = DateTime.Today;
            var dateStatement = security.DateOfLatestPrice;
            return today.ToOADate() - dateStatement.ToOADate() > 3; // 
        }

        public static bool HasNotTooManyFailedAttempts(this Security security)
        {
            return security.NbOfFailedAttemptsToGetPrices < 10;
        }

        public static bool HasNotANullPrice(this Security security)
        {
            return security.Last != 0;
        }

        public static bool HasNotNullMarketCap(this Security security)
        {
            return security.MarketCapitalisation != 0 || security.NbSharesOutstanding != 0;
        }

    }




}