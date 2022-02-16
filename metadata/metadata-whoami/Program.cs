//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API Metadata WhoAmi
// method. It provides a convenient way to check that a JWT access token is valid.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful a JSON object containing the user profile will be printed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string url = "https://api-sandbox.nofrixion.com/api/v1/metadata/whoami";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();
client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
try
{
    var response = await client.GetAsync(url);
    response.EnsureSuccessStatusCode();

    // returns user profile object
    var user = await response.Content.ReadFromJsonAsync<UserProfile>();
    if (user != null)
    {
        Console.WriteLine(user);
    }
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

// Type declarations for returned data
record UserProfile(string id, string firstName, string lastName, string emailAddress);