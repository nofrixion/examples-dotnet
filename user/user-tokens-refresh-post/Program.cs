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

var data = new Dictionary<string, string>();
data.Add("refreshToken", "v1.M-mv0QKNnuszGS96ZKxBHSb_.....");

HttpContent postData = new FormUrlEncodedContent(data);
try
{
    var response = await client.PostAsync(baseUrl, postData);
    if (response.IsSuccessStatusCode)
    {
        var newToken = await response.Content.ReadFromJsonAsync<UpdatedToken>();
        if (newToken != null)
        {
            // The response body contains a new user access and refresh tokens. 
            // These must be securely stored - they ARE NOT stored in the NoFrixion database.
            Console.WriteLine(newToken.accessToken);
            Console.WriteLine(newToken.refreshToken);
        }
        else
        {
            Console.WriteLine("No token returned.");
        }
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

// Type definitions for response data
record UpdatedToken(string accessToken, string refreshToken);
record ApiProblem(string type, string title, int status, string detail);
