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

using System.Net.Http.Json;

const string url = "https://api-sandbox.nofrixion.com/api/v1/accounts";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();
client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync(url);
    response.EnsureSuccessStatusCode();

    // response body contains JSON array of merchant accounts
    var accounts = await response.Content.ReadFromJsonAsync<List<Account>>();
    if (accounts != null)
    {
        foreach (var account in accounts)
        {
            Console.WriteLine(account);
        }
    }
    else
    {
        Console.WriteLine(($"You do not have any accounts."));
    }
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

// Type definitiions for returned data
// - The "Account" record is a parital list of properties returned by the API.
// - the full list of properites for MerchantAccount can be found at https://api-sandbox.nofrixion.com/swagger/v1/swagger.json
//   and the MerchantAccount schema.
record Account(string customerID, string customerName, string name, string displayName, string currency, string balance);
