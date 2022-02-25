//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API payouts DELETE 
// method. It provides a way to delete a pending payout
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
/// 3. Run the applicatio using:
//    dotnet run
// 4. If successful the HTTP status code 200 will be returned.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

//Remember to keep the JWT token safe and secure.
var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/payouts";

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");


// need to specify payout Id
string payoutId = "d6ce03c7-d850-43f1-1cfe-08d9eb8a1950";

HttpResponseMessage response = await client.DeleteAsync($"{baseUrl}/{payoutId}");
if (response.IsSuccessStatusCode)
{
    // HTTP status code OK on success
    Console.WriteLine(response.StatusCode);
}
else
{
    // HTTP error codes will return a MoneyMoov API problem object
    Console.WriteLine(await response.Content.ReadFromJsonAsync<ApiProblem>());
}

record ApiProblem(string type, string title, int status, string detail);