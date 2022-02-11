//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API merchant/tokens DELETE 
// method. It provides a convenient way to delete a merchant token.
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

using System.Net.Http;
using System.Text;

const string URL = "https://api-sandbox.nofrixion.com/api/v1/merchant/tokens";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/text");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

string tokenId = "a7069d1a-5581-4eb7-8ffe-169bc94203e9";

try
{
    var response = await client.DeleteAsync($"{URL}/{tokenId}");
    response.EnsureSuccessStatusCode();

    // Resposne body contains merchant token - SAVE THIS! (it isn't stored in the MoneyMoov system)
    Console.WriteLine(response.StatusCode);
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

