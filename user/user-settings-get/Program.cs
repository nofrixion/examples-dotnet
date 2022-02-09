﻿//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API user/settings GET
// method. It provides a convenient way to check that a JWT access token is valid.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful user settings will be displayed on 
//    the console.
//-----------------------------------------------------------------------------

using System.Net.Http.Headers;
using System.Net.Http.Json;

const string SANDBOX_USER_SETTINGS_URL = "https://api-sandbox.nofrixion.com/api/v1/user/settings";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

var userSettings = await client.GetFromJsonAsync<List<UserSetting>>(SANDBOX_USER_SETTINGS_URL);

if (userSettings != null)
{

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
record UserSetting(string name, string value, string description);
// Example UserSettings JSON array:
// [{"name":"CurrentMerchantID","value":"6f80138d-870b-4b07-8bc4-a4fd33a0d30f","description":"Used to store the current Merchant for a User"}]