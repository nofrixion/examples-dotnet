//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API paymentrequests POST 
// method. It provides a way to initiate payment from third parties.
//
// Usage:
// 1. Create a MERCHANT access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_MERCHANT_TOKEN=<JWT token from previous step>
/// 3. Run the applicatio using:
//    dotnet run
// 4. If successful user status code "Created" will be displayed and JSON string
//    confirming the payment request data.
//-----------------------------------------------------------------------------

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/paymentrequests";

//Remember to keep the JWT token safe and secure.
var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_MERCHANT_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

var paymentRequest = new Dictionary<string, string>();
paymentRequest.Add("MerchantID", "AB4476A1-8364-4D13-91CE-F4C4CA4EE6BE");
paymentRequest.Add("Amount", "0.99");
paymentRequest.Add("Currency", "EUR");
paymentRequest.Add("OrderID", "Sample order");
paymentRequest.Add("PaymentMethodTypes", "card,pisp"); // BTC lightning payments coming soon!
paymentRequest.Add("Description", "API Payment request");
// URLs to integrate with merchant's site (required for card payments)
paymentRequest.Add("OriginUrl", "https://some.origin.url");
paymentRequest.Add("CallbackUrl", "https://some.callback.url");
// PISP specific fields
paymentRequest.Add("PispAccountID", "A120P0JR");
paymentRequest.Add("PispRecipientReference", "Recipient ref");
// Card specific fields
paymentRequest.Add("CardAuthorizeOnly", "true");
paymentRequest.Add("CardCreateToken", "false");
paymentRequest.Add("IgnoreAddressVerification", "true");
paymentRequest.Add("CardIgnoreCVN", "true");
// Shipping and billing address data can also be included in the payment request
// => see https://api-sandbox.nofrixion.com/swagger/index.html for a complete reference.

HttpContent postData = new FormUrlEncodedContent(paymentRequest);
try
{
    HttpResponseMessage response = await client.PostAsync(baseUrl, postData);

    // "created" on success
    Console.WriteLine(response.StatusCode);

    // JSON object with payment request details will be in the response body
    Console.WriteLine(await response.Content.ReadAsStringAsync());
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

