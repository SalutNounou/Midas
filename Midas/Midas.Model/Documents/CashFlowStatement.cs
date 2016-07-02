using System;

namespace Midas.Model.Documents
{
    public class CashFlowStatement
    {
        public Decimal AccountingChange { get; set; }
        public Decimal InvestmentChangesNet { get; set; }
        public Decimal TotalAdjustments { get; set; }
        public Decimal CfDepreciationAmortization { get; set; }
        public Decimal ChangeInAccountReceivable { get; set; }
        public Decimal ChangeInCurrentAsset { get; set; }
        public Decimal ChangeInCurrentLiabilities { get; set; }
        public Decimal ChangeInInventories { get; set; }
        public Decimal DividendsPaid { get; set; }
        public Decimal EffectOfExchangeRateOnCash { get; set; }
        public Decimal CapitalExpanditure { get; set; }
        public Decimal NetChangeInCash { get; set; }
        public Decimal CashFromFinancingActivities { get; set; }
        public Decimal CashFromInvestingActivities { get; set; }
        public Decimal CashFromOperatingActivities { get; set; }
    }
}