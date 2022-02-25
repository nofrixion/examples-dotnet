//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API payouts PUT 
// method. It provides a convenient way to modify a previously created payout.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful "OK", followed by the updated payout object, will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/payouts";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

// specify id of payout to update
string payoutId = "53aad9c1-0a2e-43ba-0029-08d9f815df7c";

var client = new HttpClient();
client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

var data = new Dictionary<string, string>();
data.Add("AccountID", "A120P0JR");
data.Add("Currency", "EUR");
data.Add("Amount", "1.99");
data.Add("YourReference", "Updated Payment");
data.Add("DestinationIBAN", "GB94BARC10201530093459");
data.Add("DestinationAccountName", "Dest Name");
data.Add("TheirReference", "Their Ref");

HttpContent postData = new FormUrlEncodedContent(data);

try
{
    var response = await client.PutAsync($"{baseUrl}/{payoutId}", postData);
    if (response.IsSuccessStatusCode)
    {
        // "OK" on success
        Console.WriteLine(response.StatusCode);

        // The newly created payout object will be returned in the response body
        Console.WriteLine(await response.Content.ReadFromJsonAsync<Payout>());
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

// Type definitions for returned data
record Payout(string currentUserID, string currentUserRole, string approvePayoutUrl, string id,
                string accountID, string userID, string type, string description, string currency,
                decimal amount, string yourReference, string destinationAccountID, string destinationIBAN,
                string destinationAccountName, string theirReference);

record ApiProblem(string type, string title, int status, string detail);