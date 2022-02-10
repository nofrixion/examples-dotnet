//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API payouts PUT 
// method. It provides a convenient way to modify a previously created payout.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful the payout Id of the updated payout is displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Headers;
using System.Net.Http.Json;

const string SANDBOX_PAYOUTS_URL = "https://api-sandbox.nofrixion.com/api/v1/payouts";

string jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

// specify id of payout to update
string payoutId = "d6ce03c7-d850-43f1-1cfe-08d9eb8a1950";

var client = new HttpClient();
client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

HttpContent payoutData = new FormUrlEncodedContent(
    new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("AccountID", "A120P0JR"),
                new KeyValuePair<string,string>("Currency","EUR"),
                new KeyValuePair<string,string>("Amount","1.99"),
                new KeyValuePair<string,string>("YourReference", "Updated Payment"),
                new KeyValuePair<string, string>("DestinationIBAN", "GB94BARC10201530093459"),
                new KeyValuePair<string, string>("DestinationAccountName", "Dest Name"),
                new KeyValuePair<string, string>("TheirReference", "Their Ref")
    });

try
{
    var response = await client.PutAsync($"{SANDBOX_PAYOUTS_URL}/{payoutId}", payoutData);
    response.EnsureSuccessStatusCode();

    // On success, returns the payout ID of the updated payout
    Console.WriteLine(await response.Content.ReadAsStringAsync());
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

