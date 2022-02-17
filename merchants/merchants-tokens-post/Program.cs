﻿//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API merchants/tokens POST 
// method. It provides a convenient way to create a merchant token.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful the Merchant token is returned in the response body 
//    (save this in a safe place, it isn't stored in the NoFrixion systems).
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string URL = "https://api-sandbox.nofrixion.com/api/v1/merchants/tokens";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

HttpContent data = new FormUrlEncodedContent(
    new List<KeyValuePair<string, string>> {
                new KeyValuePair<string,string>("MerchantId","ab4476a1-8364-4d13-91ce-f4c4ca4ee6be"),
                new KeyValuePair<string,string>("Description","API created token")
    });

try
{
    var response = await client.PostAsync(URL, data);
    response.EnsureSuccessStatusCode();

    var responseBody = await response.Content.ReadFromJsonAsync<MerchantToken>();
    if (responseBody != null)
    {
        // Resposne body contains merchant token - SAVE THIS! (it isn't stored in the MoneyMoov system)
        Console.WriteLine(responseBody.token);

    }
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

// Type declarations for returned data
record MerchantToken(string id, string merchantId, string description, string inserted,
            string lastUpdated, string token);