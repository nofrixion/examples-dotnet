//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API 
// paymentrequests/{id}/pisp POST method. It submits a payment initiation  
// request.
//
// Usage:
// 1. Create a MERCHANT access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_MERCHANT_TOKEN=<JWT token from previous step>
/// 3. Run the applicatio using:
//    dotnet run
// 4. If successful user the PISP response object containing the payment initiation 
//    ID and redirect URL will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;
using System.Text;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/paymentrequests";

// Payment requests use MERCHANT tokens (remember to keep these safe and secure).
var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_MERCHANT_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

string paymentRequestID = "18fc90ae-0086-4ef3-8216-08d9f1deec34";
string providerID="H120000002";

var postData = new StringContent($"providerID={providerID}", Encoding.UTF8, "application/x-www-form-urlencoded");

try
{
    HttpResponseMessage response = await client.PostAsync($"{baseUrl}/{paymentRequestID}/pisp", postData);
    response.EnsureSuccessStatusCode();

    // returns a list of paymentRequest
    var pispResponse = await response.Content.ReadFromJsonAsync<PispResponse>();
    if (pispResponse != null)
    {
        // Response is an JSON object containing PISP response details
        Console.WriteLine(pispResponse);

    }
    else
    {
        Console.WriteLine("No response returned.");
    }
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

// type definition for response data
record PispResponse(string paymentInitiationId, string redirectUrl, string plaidLinkToken);
