//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API user 
// GET method. It provides a convenient way to retrieve profile information
// for the authenticated user.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful user the user's profile details will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/user";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync(baseUrl);
    if (response.IsSuccessStatusCode)
    {
        var userProfile = await response.Content.ReadFromJsonAsync<UserProfile>();
        // View contents of user profile, can also access as userProfile.firstName etc.
        Console.WriteLine(userProfile);
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
record UserProfile(string firstName, string lastName, string emailAddress);

record ApiProblem(string type, string title, int status, string detail);