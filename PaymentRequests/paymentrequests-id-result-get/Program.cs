//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API paymentrequests/{id}/result 
// GET method. It returns the result from the processing of a payment request.
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
    HttpResponseMessage response = await client.GetAsync($"{baseUrl}/{paymentRequestID}/result");
    if (response.IsSuccessStatusCode)
    {
        // JSON object containing payment request results
        var result = await response.Content.ReadFromJsonAsync<PaymentRequestResult>();
        if (result != null)
        {
            // do something with the result object
            var resultString = JsonSerializer.Serialize<PaymentRequestResult>(result);
            Console.WriteLine(resultString);
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
record PaymentRequestPayment(string paymentRequestID, string occuredAt, string paymentMethod, decimal amount,
                string currency, string cardTokenCustomerID, string cardTransactionID, string cardAuthorizationID,
                decimal cardCaptureAmount, bool cardIsVoided);
record PaymentRequestResult(string paymentRequestID, decimal amount, string currency,
                string result, List<PaymentRequestPayment> payments);

record ApiProblem(string type, string title, int status, string detail);