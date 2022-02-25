//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API payouts POST 
// method. It provides a way to initiate funds transfer to third party accounts
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
/// 3. Run the applicatio using:
//    dotnet run
// 4. If successful "Created", followed by the new payout object, will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

//Remember to keep the JWT token safe and secure.
var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/payouts";

var client = new HttpClient();
client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

var data = new Dictionary<string, string>();
data.Add("AccountID", "A120P0JR");
data.Add("Currency", "EUR");
data.Add("Amount", "19.99");
data.Add("YourReference", "My Ref");
data.Add("DestinationIBAN", "GB94BARC10201530093459");
data.Add("DestinationAccountName", "Dest Name");
data.Add("TheirReference", "Their Ref");

HttpContent postData = new FormUrlEncodedContent(data);

HttpResponseMessage response = await client.PostAsync(baseUrl, postData);
if (response.IsSuccessStatusCode)
{
    // "Created" on success
    Console.WriteLine(response.StatusCode);

    // The newly created payout object will be returned in the response body
    Console.WriteLine(await response.Content.ReadFromJsonAsync<Payout>());
}
else
{
    // HTTP error codes will return a MoneyMoov API problem object
    Console.WriteLine(await response.Content.ReadFromJsonAsync<ApiProblem>());
}

// Type definitions for returned data
record Payout(string currentUserID, string currentUserRole, string approvePayoutUrl, string id,
                string accountID, string userID, string type, string description, string currency,
                decimal amount, string yourReference, string destinationAccountID, string destinationIBAN,
                string destinationAccountName, string theirReference);

record ApiProblem(string type, string title, int status, string detail);