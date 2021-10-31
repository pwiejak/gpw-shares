# gpw-shares
.net library to access polish GPW shares and instruments.

## Usage
The library has simple two methods for now, one to get all the companies and second to get the shares for a date.
Example:
```
// Get shares for given date for both Main and NewConnect market.
var shares = await GPWShares.GPW.GetSharesForDateAsync(date, GPWShares.Market.All);
	
// Get list of companies for NewConnect market.
var market = Market.NewConnect;
var companies = GPW.GetCompanies(market);

```
