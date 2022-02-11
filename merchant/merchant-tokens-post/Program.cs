//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API merchant/tokens POST 
// method. It provides a convenient way to create a merchant token.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful the Merchant token is returned in the response body 
//    (save this in a safe place, it isn't stored in the NoFrixion systems).
//-----------------------------------------------------------------------------

using System.Net.Http;
using System.Text;

const string URL = "https://api-sandbox.nofrixion.com/api/v1/merchant/tokens";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/text");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

HttpContent data = new FormUrlEncodedContent(
    new List<KeyValuePair<string, string>> {
                new KeyValuePair<string,string>("MerchantId","a234eb2e-1118-4a69-b550-e945961790ab"),
                new KeyValuePair<string,string>("Description","API created token")
    });

try
{
    var response = await client.PostAsync(URL, data);
    response.EnsureSuccessStatusCode();

    // Resposne body contains merchant token - SAVE THIS! (it isn't stored in the MoneyMoov system)
    Console.WriteLine(await response.Content.ReadAsStringAsync());
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

