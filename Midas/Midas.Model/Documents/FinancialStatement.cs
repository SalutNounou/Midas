using System;

namespace Midas.Model.Documents
{
    public class FinancialStatement
    {

        public int Id { get; set; }
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
        public bool CrossCalculated { get; set; }
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FinancialStatement) obj);
        }


        protected bool Equals(FinancialStatement other)
        {
            return string.Equals(PrimarySymbol, other.PrimarySymbol) && string.Equals(SicCode, other.SicCode) && string.Equals(Dcn, other.Dcn);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (PrimarySymbol != null ? PrimarySymbol.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SicCode != null ? SicCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Dcn != null ? Dcn.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}