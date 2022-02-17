//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API transfers POST 
// method. It provides a way to transfer funds between merchant accounts
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
/// 3. Run the applicatio using:
//    dotnet run
// 4. If successful status OK and transfer details are returned.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

//Remember to keep the JWT token safe and secure.
var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

string url = "https://api-sandbox.nofrixion.com/api/v1/payouts/transfer";

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
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

try
{
    var response = await client.PostAsync(url, transferData);

    // Status "OK" on success
    Console.WriteLine(response.StatusCode);

    // Or JSON object confirming transfer details
    Console.WriteLine(await response.Content.ReadFromJsonAsync<Transfer>());
}
catch (Exception e){
    Console.WriteLine($"Error: {e.Message}");
}

// Type declarations for returned data
record TransferDestination(string type, string id);
record TransferDetails(string sourceAccountId, string destinationId, string destinationType,
            TransferDestination destination, string currency, decimal amount, string reference,
            string externalReference);
record Transfer(bool isEmpty, string approvalStatus, string status, string id,
            string createdDate, string externalReference, TransferDetails details);
