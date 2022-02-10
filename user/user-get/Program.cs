//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API user 
// GET method. It provides a convenient way to retrieve profile information
// for the authenticated user.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful user the user's profile details will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Headers;
using System.Net.Http.Json;

const string SANDBOX_USER_URL = "https://api-sandbox.nofrixion.com/api/v1/user";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync(SANDBOX_USER_URL);
    response.EnsureSuccessStatusCode();

    UserProfile userProfile = await response.Content.ReadFromJsonAsync<UserProfile>();

    // View contents of user file, can also access as userProfile.firstName etc.
    Console.WriteLine(userProfile.ToString());
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}


// Type declarations for returned data
record UserProfile(string firstName, string lastName, string emailAddress);
