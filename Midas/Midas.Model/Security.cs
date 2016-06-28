using System;
using System.Collections.ObjectModel;
using MarketData.Source.FinancialStatementSource;

namespace Midas.Model
{
    public class Security
    {
        public String Ticker { get; set; }
        public String Name { get; set; }
        public String Market { get; set; }
        public String Currency { get; set; }
        //public Decimal Last { get; set; }
        //public Decimal NcavPerShare { get; set; }
        //public Decimal DebtRatio { get; set; }
        //public DateTime DateOfLatestFinancialStatement { get; set; }
        //public Decimal DiscountOnNcav { get; set; }
        //public virtual ObservableCollection<FinancialStatement> FinancialStatements { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
