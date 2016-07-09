using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using Midas.Model.Documents;

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
        
        public Decimal NbSharesOutstanding { get; set; }

        public Security()
        {
            DateOfLatestFinancialStatement = new DateTime(1900, 1, 1);
            DateOfLatestPrice = new DateTime(1900,1,1);
        }

        public Decimal MarketCapitalisation { get; set; }
        public DateTime DateOfLatestFinancialStatement { get; set; }
        public DateTime DateOfLatestPrice { get; set; }


        public Decimal DiscountOnNcav { get; set; }
        public virtual ObservableCollection<FinancialStatement> FinancialStatements { get; set; }
        public byte[] RowVersion { get; set; }



        public void RefreshFrom(Security other)
        {
            //this.Ticker = other.Ticker;
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
            this.DateOfLatestFinancialStatement = other.DateOfLatestFinancialStatement;
            this.DateOfLatestPrice = other.DateOfLatestPrice;
            this.DiscountOnNcav = other.DiscountOnNcav;
            this.FinancialStatements = other.FinancialStatements;
            this.RowVersion = other.RowVersion;
        }
    }

    
}
