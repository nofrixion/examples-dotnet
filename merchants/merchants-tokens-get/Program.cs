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
string merchantId = "a234eb2e-1118-4a69-b550-e945961790ab";

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync($"{baseUrl}/{merchantId}/tokens");
    if (response.IsSuccessStatusCode)
    {
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
record MerchantToken(string id, string merchantId, string description, string inserted,
            string lastUpdated);

record ApiProblem(string type, string title, int status, string detail);