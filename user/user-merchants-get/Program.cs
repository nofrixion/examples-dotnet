//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API user/merchants 
// GET method. It provides a convenient way to retrieve a merchants authorised
// for the authenticated user.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful user the associated merchants will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string SANDBOX_USER_MERCHANTS_URL = "https://api-sandbox.nofrixion.com/api/v1/user/merchants";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync(SANDBOX_USER_MERCHANTS_URL);
    response.EnsureSuccessStatusCode();

    var userMerchants = await response.Content.ReadFromJsonAsync<UserMerchants>();
    if (userMerchants != null)
    {
        // can merchant currently associated with user context
        Console.WriteLine($"Current merchant: {userMerchants.CurrentMerchantName}");
        // Or view all merchants associated with the authenticated
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
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}


// Type declarations for returned data
record Merchant(string id, string name, bool enabled, string modulrMerchantID, string merchantCategoryCode);
record UserMerchants(string CurrentMerchantName, string currentMerchantId, List<Merchant> merchants);