//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API merchants 
// GET method. It provides a convenient way to retrieve a list of merchants authorised
// for the authenticated user.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful the merchants associated with the user token will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/merchants";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync(baseUrl);
    if (response.IsSuccessStatusCode)
    {
        var userMerchants = await response.Content.ReadFromJsonAsync<UserMerchants>();
        if (userMerchants != null)
        {
            // View all merchants associated with the authenticated user
            foreach (Merchant merchant in userMerchants.merchants)
            {
                Console.WriteLine(merchant);
            }
        }
        else
        {
            Console.WriteLine("User is not associated with any merchants.");
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
record Merchant(string id, string name, bool enabled, string modulrMerchantID, string merchantCategoryCode);
record UserMerchants(string CurrentMerchantName, string currentMerchantId, List<Merchant> merchants);

record ApiProblem(string type, string title, int status, string detail);