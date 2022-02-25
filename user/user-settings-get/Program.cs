//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API user/settings GET
// method. The current merchant context for the authenticated user is stored in the 
// user settings and can be used to determine which merchant's data is processed 
// when using other API endpoints.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful user settings (CurrentMerchantID) will be displayed on the console.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/user/settings";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync(baseUrl);

    if (response.IsSuccessStatusCode)
    {
        var userSettings = await response.Content.ReadFromJsonAsync<List<UserSetting>>();
        if (userSettings != null)
        {
            // As of MoneyMoov v1.1.9 there is only one user setting (CurrentMerchantID), but more may be added in the future
            foreach (UserSetting setting in userSettings)
            {
                Console.WriteLine($"Name: {setting.name}");
                Console.WriteLine($"Value: {setting.value}");
                Console.WriteLine($"Description: {setting.description}");
            }
        }
        else
        {
            Console.WriteLine("No user settings returned");
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


// Type definition for returned data
record UserSetting(string name, string value, string description);
record ApiProblem(string type, string title, int status, string detail);