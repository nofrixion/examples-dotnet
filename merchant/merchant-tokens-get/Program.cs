//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API merchant/tokens 
// GET method. It provides a convenient way to retrieve information about  
// tokens issued to the specified merchant.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful a list of merchant tokens is displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string URL = "https://api-sandbox.nofrixion.com/api/v1/merchant/tokens";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

var merchantId = "a234eb2e-1118-4a69-b550-e945961790ab";

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync($"{URL}/{merchantId}");
    response.EnsureSuccessStatusCode();

    var tokens = await response.Content.ReadFromJsonAsync<List<MerchantToken>>();
    if (tokens != null && tokens.Count != 0)
    {
        foreach (var token in tokens)
        {
            // Display token information
            Console.WriteLine(token);
        }
    }
    else
    {
        // This should never run as a token is required for the API call.
        Console.WriteLine("No user tokens found.");
    }
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

// Type declarations for returned data
record MerchantToken(string id, string merchantId, string description, string inserted, string lastUpdated);
