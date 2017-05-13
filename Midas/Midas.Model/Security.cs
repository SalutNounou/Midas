using System;

namespace Midas.Model
{
    public class Security
    {

        public String Ticker { get; set; }
        public String Name { get; set; }
        public String Market { get; set; }
        public String Currency { get; set; }
        public Decimal Last { get; set; }
        public Decimal NcavPerShare { get; set; }
        public Decimal DebtRatio { get; set; }
        public Decimal EarningYield { get; set; }
        public Decimal ReturnOnCapital { get; set; }
        public int NbOfFailedAttemptsToGetPrices { get; set; }
        public int NbOfFailedAttemptsToGetStatements { get; set; }
       // public bool OK { get; set; }

        public Decimal NbSharesOutstanding { get; set; }
        public Decimal OperatingEarnings { get; set; }
        public Decimal EnterpriseValue { get; set; }
        public Decimal AcquirersMultiple { get; set; }

        public Security()
        {
            DateOfLatest10QFinancialStatement = new DateTime(1900, 1, 1);
            DateOfLatest10KFinancialStatement = new DateTime(1900, 1, 1);
            DateOfLatestPrice = new DateTime(1900,1,1);
            DateOfLatest20FFinancialStatement = new DateTime(1900,1,1);
            DateOfLatest40FFinancialStatement = new DateTime(1900, 1, 1);
            DateOfLatestAttemptToGetStatements = new DateTime(1900,1,1);
            DateOfLatestCalculusOnNav = new DateTime(1900,1,1);
            DateOfLatestCalculusOnAcquirersMultiple = new DateTime(1900,1,1);
        }

        public Decimal MarketCapitalisation { get; set; }
        public DateTime DateOfLatestCalculusOnNav { get; set; }
        public DateTime DateOfLatest10QFinancialStatement { get; set; }
        public DateTime DateOfLatest10KFinancialStatement { get; set; }
        public DateTime DateOfLatest20FFinancialStatement { get; set; }
        public DateTime DateOfLatest40FFinancialStatement { get; set; }
        public DateTime DateOfLatestPrice { get; set; }
        public DateTime DateOfLatestAttemptToGetStatements { get; set; }
        public DateTime DateOfLatestCalculusOnAcquirersMultiple { get; set; }

        public bool Has10K { get; set; }
        public bool Has10Q { get; set; }
        public bool Has20F { get; set; }
        public bool Has40F { get; set; }

        public bool IsNew { get; set; }
        public Decimal DiscountOnNcav { get; set; }
        
        public byte[] RowVersion { get; set; }



        public void RefreshFrom(Security other)
        {
            this.Ticker = other.Ticker;
            this.Name = other.Name;
            this.Market = other.Market;
            this.Currency = other.Currency;
            this.Last = other.Last;
            this.NcavPerShare = other.NcavPerShare;
            this.DebtRatio = other.DebtRatio;
            this.EarningYield = other.EarningYield;
            this.ReturnOnCapital = other.ReturnOnCapital;
            this.NbOfFailedAttemptsToGetPrices = other.NbOfFailedAttemptsToGetPrices;
            this.MarketCapitalisation = other.MarketCapitalisation;
            this.NbSharesOutstanding = other.NbSharesOutstanding;
            this.DateOfLatest10QFinancialStatement = other.DateOfLatest10QFinancialStatement;
            this.DateOfLatestPrice = other.DateOfLatestPrice;
            this.DiscountOnNcav = other.DiscountOnNcav;
            this.RowVersion = other.RowVersion;
        }
    }
}
