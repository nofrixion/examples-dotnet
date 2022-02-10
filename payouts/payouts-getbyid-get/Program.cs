//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API payouts/getbyid/{id} GET 
// method. It provides a convenient way to obtain a pending payout information, including
// the URL used for payout approval.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful a the selected payout details will be displayed followed by the approval URL.
//-----------------------------------------------------------------------------

using System.Net.Http.Headers;
using System.Net.Http.Json;

const string SANDBOX_PAYOUTS_URL = "https://api-sandbox.nofrixion.com/api/v1/payouts";

string jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

// specify id of payout to return
string payoutId = "90ca721d-625f-4826-9f3b-7faec90a9832";

var client = new HttpClient();
client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync($"{SANDBOX_PAYOUTS_URL}/getbyid/{payoutId}");
    response.EnsureSuccessStatusCode();

    // returns requested payout
    Payout payout = await response.Content.ReadFromJsonAsync<Payout>();

    // displays data in the payout
    Console.WriteLine(payout);

    // for authorising a payout you want to use the approvePayoutUrl property
    Console.WriteLine("\nOr just access the approval Url:");
    Console.WriteLine(payout.approvePayoutUrl);
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

// record for returned payout fields (note this endpoint returns more data than the /payouts GET)
record Payout(string currentUserID, string currentUserRole, string approvePayoutUrl, string id,
                string accountID, string userID, string type, string description, string currency,
                decimal amount, string yourReference, string destinationIBAN, string destinationAccountID,
                string destinationAccountName, string theirReference);