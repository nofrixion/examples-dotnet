//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API user/tokens/refresh 
// POST method. It provides a convenient way to refresh a user access token.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful the new user access and refresh tokens will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/user/tokens/refresh";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

HttpContent postData = new FormUrlEncodedContent(
    new List<KeyValuePair<string, string>> {
                new KeyValuePair<string,string>("refreshToken","AFNmCy_fXXQPViPaHNb5GQSpeMrKNAfLwIfzZl_BzGCZv")
    });
    
try
{
    var response = await client.PostAsync(baseUrl, postData);
    response.EnsureSuccessStatusCode();

    var userToken = await response.Content.ReadFromJsonAsync<UpdatedToken>();
    if (userToken != null)
    {
        // The response body contains a new user access and refresh token. 
        // These must be securely stored - they ARE NOT stored in the NoFrixion database.
        Console.WriteLine(userToken);
    }
    else
    {
        // This should never run as a token is required for the API call.
        Console.WriteLine("No token returned.");
    }
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

// Type definition for response data
record UpdatedToken(string accessToken, string refreshToken);
