//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API transfers POST 
// method. It provides a way to transfer funds between merchant accounts
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
/// 3. Run the applicatio using:
//    dotnet run
// 4. If successful JSON string confirming transfer detais is returned.
//-----------------------------------------------------------------------------

using System.Net.Http;

//Remember to keep the JWT token safe and secure.
var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

string SANDBOX_TRANSFERS_URL = "https://api-sandbox.nofrixion.com/api/v1/transfers";

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "text/plain");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");


HttpContent transferData = new FormUrlEncodedContent(
    new List<KeyValuePair<string, string>> {
                new KeyValuePair<string,string>("Amount","1.00"),
                new KeyValuePair<string,string>("Currency","EUR"),
                new KeyValuePair<string, string>("SourceAccount", "A120P0JR"),
                new KeyValuePair<string, string>("DestinationAccount", "A120R2Y3"),
                new KeyValuePair<string,string>("Reference", "My Reference"),
                new KeyValuePair<string, string>("ExternalReference", "Ext Reference")
    });

HttpResponseMessage response = await client.PostAsync(SANDBOX_TRANSFERS_URL, transferData);

// Status "Created" on success
Console.WriteLine(response.StatusCode);

// Or JSON string containing transfer details
Console.WriteLine(await response.Content.ReadAsStringAsync());