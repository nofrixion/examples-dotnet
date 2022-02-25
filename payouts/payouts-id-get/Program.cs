//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API payouts/getbyid/{id} GET 
// method. It provides a convenient way to obtain a pending payout information, including
// the URL used for payout approval.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful the selected payout details will be displayed followed by the approval URL.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/payouts";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

// specify id of payout to return
string payoutId = "b5a68371-b561-4123-e832-08d9eb89cb53";

var client = new HttpClient();
client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync($"{baseUrl}/{payoutId}");
    if (response.IsSuccessStatusCode)
    {
        // returns requested payout
        var payout = await response.Content.ReadFromJsonAsync<Payout>();
        if (payout != null)
        {
            // displays payout data
            Console.WriteLine(payout);
        }
    }
    else
    {
        // HTTP error codes will return a MoneyMoov API problem object
        Console.WriteLine(await response.Content.ReadFromJsonAsync<ApiProblem>());
    }
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

// Type declarations for returned data
record Payout(string currentUserID, string currentUserRole, string approvePayoutUrl, string id,
                string accountID, string userID, string type, string description, string currency,
                decimal amount, string yourReference, string destinationIBAN, string destinationAccountID,
                string destinationAccountName, string theirReference);

record ApiProblem(string type, string title, int status, string detail);