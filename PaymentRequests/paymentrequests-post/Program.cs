//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API paymentrequests POST 
// method. It provides a convenient way to initiate receipt of payment from third parties.
//
// Usage:
// 1. Create a MERCHANT access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_MERCHANT_TOKEN=<JWT token from previous step>
/// 3. Run the applicatio using:
//    dotnet run
// 4. If successful user status code "Created" will be displayed followed by a
//    JSON string confirming the payment request data.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/paymentrequests";

// Payment requests use MERCHANT tokens (remember to keep these safe and secure).
var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_MERCHANT_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

var paymentRequest = new Dictionary<string, string>();
paymentRequest.Add("MerchantID", "AB4476A1-8364-4D13-91CE-F4C4CA4EE6BE");
paymentRequest.Add("Amount", "0.99");
paymentRequest.Add("Currency", "EUR");
paymentRequest.Add("CustomerID", "C202202024158");
// If set the OrderID must be unique.
//paymentRequest.Add("OrderID", "abc1234");
paymentRequest.Add("PaymentMethodTypes", "card");
// The methods below allow the purchaser to choose from those listed - BTC lightning payments coming soon!
//paymentRequest.Add("PaymentMethodTypes", "card,pisp,lightning");
paymentRequest.Add("Description", "API Payment request");
// URLs to integrate with merchant's site (required for card payments)
paymentRequest.Add("OriginUrl", "https://some.origin.url");
paymentRequest.Add("CallbackUrl", "https://some.callback.url");
// PISP specific fields
//paymentRequest.Add("PispAccountID", "A120P0JR");
//paymentRequest.Add("PispRecipientReference", "Recipient ref");
// Optional Card specific fields
// paymentRequest.Add("CardAuthorizeOnly", "true");
// paymentRequest.Add("CardCreateToken", "false");
// paymentRequest.Add("IgnoreAddressVerification", "true");
// paymentRequest.Add("CardIgnoreCVN", "true");
// Shipping and billing address data can also be included in the payment request
// => see https://api-sandbox.nofrixion.com/swagger/index.html for a complete reference.

HttpContent postData = new FormUrlEncodedContent(paymentRequest);
try
{
    HttpResponseMessage response = await client.PostAsync(baseUrl, postData);
    if (response.IsSuccessStatusCode)
    {
        // "Created" on success
        Console.WriteLine(response.StatusCode);
        // JSON object with payment request details will be in the response body
        Console.WriteLine(await response.Content.ReadFromJsonAsync<PaymentRequest>());
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
