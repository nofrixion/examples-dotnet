//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API paymentrequests/{id} 
// GET method. It returns the details of the specified payment request.
//
// Usage:
// 1. Create a MERCHANT access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_MERCHANT_TOKEN=<JWT token from previous step>
/// 3. Run the applicatio using:
//    dotnet run

// 4. If successful the JSON object containing the payment request data will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/paymentrequests";

// Payment requests use MERCHANT tokens (remember to keep these safe and secure).
var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_MERCHANT_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

string paymentRequestID = "18fc90ae-0086-4ef3-8216-08d9f1deec34";

try
{
    HttpResponseMessage response = await client.GetAsync($"{baseUrl}/{paymentRequestID}");
    if (response.IsSuccessStatusCode)
    {
        // returns a paymentRequest
        var paymentRequest = await response.Content.ReadFromJsonAsync<PaymentRequest>();
        if (paymentRequest != null)
        {
            // JSON object containing payment request details
            Console.WriteLine(paymentRequest);
        }
        else
        {
            Console.WriteLine("No paymentRequest returned.");
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
record PaymentRequest(string id, string merchantID, decimal amount, string currency, string customerID,
                string orderID, string paymentMethodTypes, string description, string pispAccountID, string shippingFirstName,
                string shippingLastName, string shippingAddressLine1, string shippingAddressLine2, string shippingAddressCity,
                string shippingAddressCounty, string shippingAddressPostCode, string shippingAddressCountryCode,
                string shippingPhone, string shippingEmail, string originUrl, string callbackUrl, bool cardAuthorizeOnly,
                bool cardCreateToken, bool ignoreAddressVerification, bool cardIgnoreCVN, string pispRecipientReference);
record ApiProblem(string type, string title, int status, string detail);