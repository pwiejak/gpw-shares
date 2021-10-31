using System;

namespace GPWShares.Exceptions
{
    public class DailySharesException : Exception
    {
        public DailySharesException(string message) : base($"There was an error when loading daily shares: {message}")
        {
        }        
    }
}
