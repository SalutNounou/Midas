using System;

namespace Midas.Model.Documents
{
    public class IncomeStatement
    {
        public Decimal EquityEarnings { get; set; }
        public Decimal MinorityInterest { get; set; }
        public Decimal ResearchDevelopementExpense { get; set; }
        public Decimal NetIncome { get; set; }
        public Decimal InterestExpense { get; set; }
        public Decimal IncomeBeforeTaxes { get; set; }
        public Decimal ExtraordinaryItems { get; set; }
        public Decimal Ebit { get; set; }
        public Decimal CostOfRevenue { get; set; }
        public Decimal TotalRevenue { get; set; }
        public Decimal GrossProfit { get; set; }
        public Decimal DiscontinuedOperation { get; set; }
        public Decimal SellingGeneralAdministrativeExpense { get; set; }
        public Decimal NetIncomeApplicableToCommon { get; set; }
    }
}