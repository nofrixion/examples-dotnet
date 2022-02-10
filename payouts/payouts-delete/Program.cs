//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API payouts DELETE 
// method. It provides a way to delete a pending payout
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
/// 3. Run the applicatio using:
//    dotnet run
// 4. If successful user HTTP status code 200 will be returned.
//-----------------------------------------------------------------------------

using System.Net.Http;

//Remember to keep the JWT token safe and secure.
var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "text/plain");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

string url = "https://api-sandbox.nofrixion.com/api/v1/payouts";

// need to specify payout Id
string payoutId = "68a8abb6-3912-469e-685a-08d9eb7203ba";

HttpResponseMessage response = await client.DeleteAsync($"{url}/{payoutId}");

// HTTP status code 200 on success
Console.WriteLine(response.StatusCode);