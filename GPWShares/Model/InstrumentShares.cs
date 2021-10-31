using System;

namespace GPWShares.Model
{
    public class InstrumentShares
    {
        public Market Market { get; set; }
        public string Name { get; }
        public string Ticker { get; set; }
        public DateTime Date { get; set; }
        public decimal OpenRate { get; set; }
        public decimal Rate { get; set; }
        public int Volume { get; set; }
        public decimal? Turnover { get; set; }
        public string Isin { get; set; }

        public InstrumentShares(Market market, string name, string ticker, DateTime date, decimal openRate, decimal rate, int volume, decimal? turnover, string isin)
        {
            Market = market;
            Name = name;
            Ticker = ticker;
            Date = date;
            OpenRate = openRate;
            Rate = rate;
            Volume = volume;
            Turnover = turnover;
            Isin = isin;
        }        
    }
}
