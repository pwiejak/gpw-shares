namespace GPWShares.Model
{
    public class CompanyInfo
    {
        public string Name { get; }
        public string Issuer { get; }
        public string Ticker { get; }

        public CompanyInfo(string name, string issuer, string ticker)
        {
            Name = name;
            Issuer = issuer;
            Ticker = ticker;
        }
    }
}
