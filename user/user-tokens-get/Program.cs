//-----------------------------------------------------------------------------
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

// Note optional url paramaters for paging the token list are exposed in the API
// - see https://api-sandbox.nofrixion.com/swagger/index.html for full details

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync(baseUrl);
    if (response.IsSuccessStatusCode)
    {
        var userTokens = await response.Content.ReadFromJsonAsync<UserTokensPage>();
        if (userTokens != null && userTokens.content != null)
        {
            foreach (var token in userTokens.content)
            {
                // Display token information
                Console.WriteLine(token);
            }
        }
        else
        {
            Console.WriteLine("No user tokens found.");
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


// Type definitions for returned data
record UserToken(string id, string userID, string type, string description, string accessTokenHash,
            string refreshTokenHash, string inserted, string lastUpdated, string approveTokenUrl);

record UserTokensPage(List<UserToken> content, int pageNumber, int pageSize, int totalPages, int totalSize);
record ApiProblem(string type, string title, int status, string detail);