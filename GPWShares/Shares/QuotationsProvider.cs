using Flurl.Http;
using GPWShares.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace GPWShares.Shares
{
    public class QuotationsProvider
    {
        private Dictionary<Market, string> _dailyQuotationsBaseUrls = new Dictionary<Market, string>
        {
            { Market.Main, "https://biznes.pap.pl/pl/quotations/continuousQuotations/xml/wszystkie," },
            { Market.NewConnect, "https://biznes.pap.pl/pl/quotations/newConnect/xml/" }
        };

        private string GetDailyQuotationsLinkForDate(DateTime date, Market market)
        {
            return $"{_dailyQuotationsBaseUrls[market]}{date.ToString("yyyy-MM-dd")}";
        }

        public async Task<DailyShares> GetQuotationsForDate(DateTime date, Market market)
        {
            if(market != Market.All)
                return await GetDailySharesForMarket(date, market).ConfigureAwait(false);

            var mainShares = await GetDailySharesForMarket(date, Market.Main).ConfigureAwait(false);
            var newConnectShares = await GetDailySharesForMarket(date, Market.NewConnect).ConfigureAwait(false);

            mainShares.Instruments.AddRange(newConnectShares.Instruments);

            return mainShares;
        }

        private async Task<DailyShares> GetDailySharesForMarket(DateTime date, Market market)
        {
            var url = GetDailyQuotationsLinkForDate(date, market);
            var result = new DailyShares(date.Date);

            using (var s = await GetResponseStreamAsync(url).ConfigureAwait(false))
            using (var sr = new StreamReader(s))
            {
                var xmldoc = new XmlDocument();
                XmlNodeList nodesList;

                xmldoc.Load(sr);
                nodesList = xmldoc.GetElementsByTagName("INSTRUMENT");

                for (int i = 0; i <= nodesList.Count - 1; i++)
                {
                    var node = nodesList[i];
                    var ticker = node["TICKER"].InnerText;
                    if (!IsTickerValid(ticker))
                        continue;

                    var instrument = new InstrumentShares(
                        market,
                        node["NAME"].InnerText,
                        ticker,
                        DateTime.Parse(node["date"].InnerText),
                        decimal.Parse(node["OPENRATE"].InnerText, CultureInfo.InvariantCulture),
                        decimal.Parse(node["RATE"].InnerText, CultureInfo.InvariantCulture),
                        int.Parse(node["VOLUME"].InnerText),
                        ParseDecimal(node["TOURNOVER"].InnerText),
                        node["ISIN"].InnerText
                        );

                    result.Instruments.Add(instrument);
                }
            }

            return result;
        }

        private bool IsTickerValid(string ticker)
        {
            if (string.IsNullOrWhiteSpace(ticker) || string.Equals(ticker, "null", StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }
        private decimal? ParseDecimal(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || string.Equals(text, "null", StringComparison.InvariantCultureIgnoreCase))
                return null;

            return decimal.Parse(text, CultureInfo.InvariantCulture);
        }

        private async Task<Stream> GetResponseStreamAsync(string url, CancellationToken token = default(CancellationToken))
           => await url
               .GetAsync(token)
               .ReceiveStream()
               .ConfigureAwait(false);
    }
}
