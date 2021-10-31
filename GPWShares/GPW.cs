using GPWShares.Companies;
using GPWShares.Exceptions;
using GPWShares.Model;
using GPWShares.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPWShares
{
    public class GPW
    {
        /// <summary>
        /// Get companies for given stock market, or all if market not provided.
        /// </summary>       
        /// <param name="market" cref="Market">Type of market (GPW, NewConnect, All)</param>
        /// <returns>List of companies for given market.</returns>
        public static IEnumerable<CompanyInfo> GetCompanies(Market market)
        {
            var companies = CompaniesProvider.GetCompanies(market).ToList();

            if (companies == null || companies.Count == 0)
                throw new Exception("There were no companies returned.");

            return companies;
        }

        /// <summary>
        /// Returns list of daily ratings for instruments from GPW for given date.
        /// </summary>
        /// <param name="date">Quotations date.</param>
        /// <returns>List of ratings.</returns>
        public static async Task<DailyShares> GetSharesForDateAsync(DateTime date, Market market)
        {
            var provider = new QuotationsProvider();
            try
            {
                return await provider.GetQuotationsForDate(date, market);
            }
            catch(Exception e)
            {
                throw new DailySharesException(e.Message);
            }            
        }
    }
}
