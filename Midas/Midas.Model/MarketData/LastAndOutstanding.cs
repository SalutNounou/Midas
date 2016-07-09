using System;

namespace Midas.Model.MarketData
{
    public class LastAndOutstanding
    {
        public string Ticker { get; set; }
        public Decimal Last { get; set; }
        public Decimal SharesOutstanding { get; set; }
        public Decimal MarketCapitalisation { get; set; }
    }
}