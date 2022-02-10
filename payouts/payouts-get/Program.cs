//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API payouts GET
// method. It provides a convenient way to retrieve a list of pending payouts
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful a list of selected payout details will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Headers;
using System.Net.Http.Json;

const string SANDBOX_PAYOUTS_URL = "https://api-sandbox.nofrixion.com/api/v1/payouts";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

var client = new HttpClient();
client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync(SANDBOX_PAYOUTS_URL);
    response.EnsureSuccessStatusCode();

    // returns a list of payouts
    var payouts = await response.Content.ReadFromJsonAsync<List<Payout>>();

    // do something with response, e.g. print some relevant payout fields for each payout
    foreach (var payout in payouts)
    {
        Console.WriteLine($"Send {payout.currency} {payout.amount:0.00} to {payout.destinationIban} ({payout.yourReference})");
    }

}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

// record datastructure for returned payout fields
record Payout(string id, string accountId, string userId, string type, string currency,
                decimal amount, string yourReference, string destinationIban,
                string destinationAccountName, string theirReference);
