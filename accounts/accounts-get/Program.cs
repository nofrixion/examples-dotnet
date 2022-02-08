//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API Accounts Get
// method. It provides a convenient way to retrieve a list of your payment
// accounts.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful a list of your accounts will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Headers;
using System.Net.Http.Json;

const string SANDBOX_ACCOUNTS_GET_URL = "https://api-sandbox.nofrixion.com/api/v1/accounts";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

var client = new HttpClient();
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
var accounts = await client.GetFromJsonAsync<List<Account>>(SANDBOX_ACCOUNTS_GET_URL);

if (accounts != null)
{
    foreach (var account in accounts)
    {
        Console.WriteLine($"Name {account.Name}, Currency {account.Currency}, Balance {account.Balance}.");
    }
}
else
{
    Console.WriteLine(($"You do not have any accounts."));
}

// The full list of properites can be found at https://api-sandbox.nofrixion.com/swagger/v1/swagger.json
// and the MerchantAccount type.
record Account(string Name, string Currency, string Balance);
