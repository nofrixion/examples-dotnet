//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API payouts POST 
// method. It provides a way to initiate funds transfer to third party accounts
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
/// 3. Run the applicatio using:
//    dotnet run
// 4. If successful user the newly created payout ID will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http;

//Remember to keep the JWT token safe and secure.
string jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "text/plain");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

string url = "https://api-sandbox.nofrixion.com/api/v1/payouts";

HttpContent paymentData = new FormUrlEncodedContent(
    new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("AccountID", "A120P0JR"),
                new KeyValuePair<string,string>("Currency","EUR"),
                new KeyValuePair<string,string>("Amount","19.99"),
                new KeyValuePair<string,string>("YourReference", "My Ref"),
                new KeyValuePair<string, string>("DestinationIBAN", "GB94BARC10201530093459"),
                new KeyValuePair<string, string>("DestinationAccountName", "Dest Name"),
                new KeyValuePair<string, string>("TheirReference", "Their Ref")
    });

HttpResponseMessage response = await client.PostAsync(url, paymentData);

// 200 on success
Console.WriteLine(response.StatusCode);

// Or the payout Id of the newly created payout will be in the response body
Console.WriteLine(await response.Content.ReadAsStringAsync());