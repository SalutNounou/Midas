using System;

namespace Midas.Model.Documents
{
    public class BalanceSheet
    {
        public Decimal CashAndCashEquivalent { get; set; }
        public Decimal CashCashEquivalentAndShortTermInvestments { get; set; }
        public Decimal Goodwill { get; set; }
        public Decimal IntangibleAssets { get; set; }
        public Decimal InventoriesNet { get; set; }
        public Decimal OtherAssets { get; set; }
        public Decimal OtherCurrentAssets { get; set; }
        public Decimal OtherCurrentLiabilities { get; set; }
        public Decimal OtherEquity { get; set; }
        public Decimal OtherLiabilities { get; set; }
        public Decimal PreferredStock { get; set; }
        public Decimal PropertyPlantEquipmentNet { get; set; }
        public Decimal RetainedEarnings { get; set; }
        public Decimal TotalAssets { get; set; }
        public Decimal TotalCurrentAssets { get; set; }
        public Decimal TotalCurrentLiabilities { get; set; }
        public Decimal TotalLiabilities { get; set; }
        public Decimal TotalLongTermDebt { get; set; }
        public Decimal TotalReceivableNet { get; set; }
        public Decimal TotalShortTermDebt { get; set; }
        public Decimal TotalStockHolderEquity { get; set; }
        public Decimal TreasuryStock { get; set; }
        public Decimal CommonStock { get; set; }
        public Decimal DeferredCHarges { get; set; }
    }
}