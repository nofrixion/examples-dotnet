//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API paymentrequests/{id}/events 
// GET method. It returns a list of payment request events.
//
// Usage:
// 1. Create a MERCHANT access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_MERCHANT_TOKEN=<JWT token from previous step>
/// 3. Run the applicatio using:
//    dotnet run
// 4. If successful the JSON object containing the payment request result data will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;
using System.Text.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/paymentrequests";

// Payment requests use MERCHANT tokens (remember to keep these safe and secure).
var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_MERCHANT_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

string paymentRequestID = "b2d3c9b2-e5f0-4074-988b-08d9f65a6611";

try
{
    HttpResponseMessage response = await client.GetAsync($"{baseUrl}/{paymentRequestID}/events");
    if (response.IsSuccessStatusCode)
    {
        // returns JSON objects aray of payment request events, do something with this....
        var events = await response.Content.ReadFromJsonAsync<List<PaymentRequestEvent>>();
        if (events != null)
        {
            foreach (PaymentRequestEvent ev in events) {
            var eventString = JsonSerializer.Serialize<PaymentRequestEvent>(ev);
            Console.WriteLine(eventString);

            }
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

// type definition for response data
record PaymentRequestEvent(string id, string paymentRequestID, string inserted, string eventType, decimal amount, 
                string currency, string status, string errorReason, string errorMessage, string cardRequestId, 
                string cardTransactionID, string cardTokenCustomerID, string cardAuthorizationResponseID,
                string lightningInvoice, string pispPaymentServiceProviderID, string pispPaymentInitiationID,
                string pispRedirectUrl);

record ApiProblem(string type, string title, int status, string detail);