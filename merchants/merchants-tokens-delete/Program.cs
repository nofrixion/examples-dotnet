//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API merchants/tokens/{tokenId}  
// DELETE method. It provides a convenient way to delete a merchant token.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful HTTP status code "OK" will be displayed (and the token will
//    no longer be listed using the merchant/tokens GET method).
//-----------------------------------------------------------------------------

const string URL = "https://api-sandbox.nofrixion.com/api/v1/merchants/tokens";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

string tokenId = "ed2c1ca4-8c64-4d75-a64a-f3e6c00af500";

try
{
    var response = await client.DeleteAsync($"{URL}/{tokenId}");
    response.EnsureSuccessStatusCode();

    Console.WriteLine(response.StatusCode);
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

