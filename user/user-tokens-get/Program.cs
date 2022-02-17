﻿//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API user/tokens 
// GET method. It provides a convenient way to retrieve information about  
// access tokens issued to the authenticated user.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful the user's API access tokens will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/user/tokens";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync(baseUrl);
    response.EnsureSuccessStatusCode();

    var userTokens = await response.Content.ReadFromJsonAsync<List<UserToken>>();
    if (userTokens != null)
    {
        foreach (var token in userTokens)
        {
            // Display token information
            Console.WriteLine(token);
        }
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
