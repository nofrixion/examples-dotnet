//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API user/tokens/{id} 
// DELETE method. It provides a convenient way to delete the specified access token.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful the HTTP status code "OK" will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/user/tokens";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

string tokenID = "532cfdd2-a5f3-4628-8398-fb2e0931557a";

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.DeleteAsync($"{baseUrl}/{tokenID}");

    // Status Code "OK" on successful delete (check using user/tokens GET action)
    Console.WriteLine(response.StatusCode);
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}
