//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API user/settings POST 
// method. It provides a convenient way to update the current merchant a settings for  
// the authenticated user.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful HTTP status "OK" will be displayed (and the new settings can be retrieved
//    using the user/settings GET method).
//-----------------------------------------------------------------------------

using System.Net.Http;
using System.Text;

const string SANDBOX_USER_SETTINGS_URL = "https://api-sandbox.nofrixion.com/api/v1/user/settings";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/text");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

string settingName = "CurrentMerchantID";
string settingValue = "6f80138d-870b-4b07-8bc4-a4fd33a0d30f";


var data = new StringContent($"userSettings[0].Name={settingName}&userSettings[0].Value={settingValue}&userSettings[0].Description=desc",
                        Encoding.UTF8, "application/x-www-form-urlencoded");

try
{
    var response = await client.PostAsync(SANDBOX_USER_SETTINGS_URL, data);
    response.EnsureSuccessStatusCode();

    // Status "OK" on success
    Console.WriteLine(response.StatusCode);
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

