using GPWShares.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPWShares.Companies
{
    internal class CompaniesProvider
    {
        private static IDictionary<Market, string> _marketCompaniesScrapingLinks = new Dictionary<Market, string>
        {
            { Market.All, "https://infostrefa.com/infostrefa/pl/spolki" },
            { Market.Main, "https://infostrefa.com/infostrefa/pl/spolki?market=mainMarket" },
            { Market.NewConnect, "https://infostrefa.com/infostrefa/pl/spolki?market=newconnect" }
        };

        private static readonly string _companiesInfoSelector = "//div[@id='companiesList']//tbody/tr";

        public static IEnumerable<CompanyInfo> GetCompanies(Market market)
        {
            var url = _marketCompaniesScrapingLinks[market];            

            var web = new HtmlWeb();
            var doc = web.Load(url);

            var nodes = doc.DocumentNode
                .SelectNodes(_companiesInfoSelector)
                .ToList();

            var companies = new List<CompanyInfo>();
            foreach (var node in nodes)
            {
                var props = node.ChildNodes.Select(c => c.InnerText).ToList();
                var issuer = props[1];
                var name = props[3];
                var ticker = props[5];

                if (!string.IsNullOrWhiteSpace(ticker))
                {
                    companies.Add(new CompanyInfo(name, issuer, ticker));
                }
            }

            if (companies == null || companies.Count == 0)
                throw new Exception("There was an error when loading companies");

            return companies;
        }
    }
}
