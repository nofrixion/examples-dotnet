//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API merchants/{merchantId}/tokens 
// GET method. It provides a convenient way to retrieve information about  
// tokens issued to the specified merchant.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful a list of merchant tokens is displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/merchants";
string merchantId = "6f80138d-870b-4b07-8bc4-a4fd33a0d30f";

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync($"{baseUrl}/{merchantId}/tokens");
    response.EnsureSuccessStatusCode();

    var merchantTokens = await response.Content.ReadFromJsonAsync<List<MerchantToken>>();
    if (merchantTokens != null && merchantTokens.Count != 0)
    {
        foreach (var merchantToken in merchantTokens)
        {
            // Display merchant tokens token information
            Console.WriteLine(merchantToken);
        }
    }
    else
    {
        Console.WriteLine("No merchant tokens found.");
    }
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

// Type declarations for returned data
record MerchantToken(string id, string merchantId, string description, string inserted,
            string lastUpdated);
