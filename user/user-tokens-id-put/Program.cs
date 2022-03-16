//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API user/tokens/{id} 
// PUT method. It provides a convenient way to update a user access token's description.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful the updated details of the updated user access token will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/user/tokens";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

string tokenID = "042a5ab9-3f36-4d6e-a7c8-fcdc901c7e2d";

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

// Currently only the token description can be updated.
var uptdatedToken = new Dictionary<string, string>();
uptdatedToken.Add("Description", $"Updated on {DateTime.UtcNow.ToString()}");

HttpContent putData = new FormUrlEncodedContent(uptdatedToken);

try
{
    var response = await client.PutAsync($"{baseUrl}/{tokenID}", putData);
    if (response.IsSuccessStatusCode)
    {
        // Display updated token information
        var userToken = await response.Content.ReadFromJsonAsync<UserToken>();
        Console.WriteLine(userToken);
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
record UserToken(string id, string userID, string type, string description, string accessTokenHash,
            string refreshTokenHash, string inserted, string lastUpdated, string approveTokenUrl);

record ApiProblem(string type, string title, int status, string detail);