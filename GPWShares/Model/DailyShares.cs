using System;
using System.Collections.Generic;

namespace GPWShares.Model
{
    public class DailyShares
    {
        public DateTime Date { get; }
        public List<InstrumentShares> Instruments { get; }

        public DailyShares(DateTime date)
        {
            Date = date;
            Instruments = new List<InstrumentShares>();
        }      
    }
}
