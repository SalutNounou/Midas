using System;

namespace Midas.Model.Documents
{
    public class FinancialStatement
    {
        public BalanceSheet BalanceSheet { get; set; }
        public IncomeStatement IncomeStatement { get; set; }
        public CashFlowStatement CashFlowStatement { get; set; }

        #region MetaData
        public string Cik { get; set; }
        public string CompanyName { get; set; }
        public string EntityId { get; set; }
        public string PrimaryExchange { get; set; }
        public string PrimarySymbol { get; set; }
        public string SicCode { get; set; }
        public string SicDescription { get; set; }
        public decimal UsdConversionRate { get; set; }
        public bool Restated { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool Preliminary { get; set; }
        public string PeriodLengthCode { get; set; }
        public decimal PeriodLength { get; set; }
        public DateTime PeriodEnd { get; set; }
        public bool Original { get; set; }
        public string FormType { get; set; }
        public int FiscalYear { get; set; }
        public int FiscalQuarter { get; set; }
        public string Dcn { get; set; }
        public string CurrencyCode { get; set; }
        public bool CrossCalulated { get; set; }
        public bool Audited { get; set; }
        public bool Amended { get; set; }
        #endregion MetaData


        public FinancialStatement()
        {
            BalanceSheet = new BalanceSheet();
            IncomeStatement = new IncomeStatement();
            CashFlowStatement = new CashFlowStatement();
            ReceivedDate = new DateTime(1900,1,1);
            PeriodEnd = new DateTime(1900,1,1);
        }
    }
}