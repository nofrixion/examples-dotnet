//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API transfers POST 
// method. It provides a way to transfer funds between merchant accounts
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful status OK is returned followed by the transfer details.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

//Remember to keep the JWT token safe and secure.
var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/payouts/transfer";

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

var data = new Dictionary<string, string>();
data.Add("Amount", "1.00");
data.Add("Currency", "EUR");
data.Add("SourceAccount", "A120P0JR");
data.Add("DestinationAccount", "A120R2Y3");
data.Add("Reference", "My Reference");
data.Add("ExternalReference", "Ext Reference");

HttpContent postData = new FormUrlEncodedContent(data);

try
{
    var response = await client.PostAsync(baseUrl, postData);
    if (response.IsSuccessStatusCode)
    {
        // Status "OK" on success
        Console.WriteLine(response.StatusCode);
        // and JSON object confirming transfer details
        Console.WriteLine(await response.Content.ReadFromJsonAsync<Transfer>());
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
record TransferDestination(string type, string id);
record TransferDetails(string sourceAccountId, string destinationId, string destinationType,
            TransferDestination destination, string currency, decimal amount, string reference,
            string externalReference);
record Transfer(bool isEmpty, string approvalStatus, string status, string id,
            string createdDate, string externalReference, TransferDetails details);

record ApiProblem(string type, string title, int status, string detail);