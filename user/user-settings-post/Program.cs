//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API user/settings POST 
// method. It provides a convenient way to update the current merchant a settings for  
// the authenticated user.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful HTTP status "OK" will be displayed (and the new settings can be retrieved
//    using the user/settings GET method).
//-----------------------------------------------------------------------------

using System.Net.Http.Json;
using System.Text;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/user/settings";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

string settingName = "CurrentMerchantID";
string settingValue = "a234eb2e-1118-4a69-b550-e945961790ab";

var data = new StringContent($"userSettings[0].Name={settingName}&userSettings[0].Value={settingValue}&userSettings[0].Description=desc",
                        Encoding.UTF8, "application/x-www-form-urlencoded");

try
{
    var response = await client.PostAsync(baseUrl, data);

    if (response.IsSuccessStatusCode)
    {
        // Status "OK" on success
        Console.WriteLine(response.StatusCode);
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
record ApiProblem(string type, string title, int status, string detail);