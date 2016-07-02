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

        public Security()
        {
            DateOfLatestFinancialStatement = new DateTime(1900, 1, 1);
        }

        public DateTime DateOfLatestFinancialStatement { get; set; }


        public Decimal DiscountOnNcav { get; set; }
        public virtual ObservableCollection<FinancialStatement> FinancialStatements { get; set; }
        public byte[] RowVersion { get; set; }
    }

    
}
