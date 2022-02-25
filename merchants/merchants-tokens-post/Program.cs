//-----------------------------------------------------------------------------
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

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/merchants/tokens";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

var data = new Dictionary<string, string>();
data.Add("MerchantID","a234eb2e-1118-4a69-b550-e945961790ab");
data.Add("Description","API created token");

HttpContent postData = new FormUrlEncodedContent(data);

try
{
    var response = await client.PostAsync(baseUrl, postData);
    if (response.IsSuccessStatusCode)
    {
        var responseBody = await response.Content.ReadFromJsonAsync<MerchantToken>();
        if (responseBody != null)
        {
            // Resposne body JSON contains merchant token - SAVE THIS! (it isn't stored in the MoneyMoov system)
            Console.WriteLine(responseBody);
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

// Type declarations for returned data
record MerchantToken(string id, string merchantId, string description, string inserted,
            string lastUpdated, string token);

record ApiProblem(string type, string title, int status, string detail);