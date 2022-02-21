//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API user/tokens/{id} 
// PUT method. It provides a convenient way to update information about  
// the specified access token.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful the updated details of the specified user access token will be displayed.
//-----------------------------------------------------------------------------

using System;
using System.Net.Http.Json;
const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/user/tokens";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

string tokenID = "dad3ef3f-6732-4448-b363-6434a256d5ac";

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

var uptdatedToken = new Dictionary<string, string>();
uptdatedToken.Add("UserID", "b9300b17-d00f-43d0-baec-c571a0d4350c");
uptdatedToken.Add("Type", "test");
uptdatedToken.Add("Description", $"Update {DateTime.UtcNow.ToString()}");
//uptdatedToken.Add("AccessTokenHash", "...");
//uptdatedToken.Add("RefreshTokenHash", "..."); 

HttpContent putData = new FormUrlEncodedContent(uptdatedToken);

try
{
    var response = await client.PutAsync($"{baseUrl}/{tokenID}", putData);
    response.EnsureSuccessStatusCode();

    var userToken = await response.Content.ReadFromJsonAsync<UserToken>();
    if (userToken != null)
    {
            // Display token information
            Console.WriteLine(userToken);
    }
    else
    {
        // This should never run as a token is required for the API call.
        Console.WriteLine("No user tokens found.");
    }
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}


// Type definition for returned data
record UserToken(string id, string userID, string type, string description, string accessTokenHash,
            string refreshTokenHash, string inserted, string lastUpdated, string approveTokenUrl);
