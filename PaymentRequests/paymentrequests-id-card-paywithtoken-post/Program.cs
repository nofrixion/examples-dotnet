//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API 
// paymentrequests/{id}/card/paywithtoken POST method. It submits payment request   
// using a tokenised card to the payment gateway.
//
// Usage:
// 1. Create a MERCHANT access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_MERCHANT_TOKEN=<JWT token from previous step>
/// 3. Run the applicatio using:
//    dotnet run
// 4. If successful JSON object containing the card payment respone
//    model will be displayed
//-----------------------------------------------------------------------------

using System.Net.Http.Json;
using System.Text;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/paymentrequests";

// Payment requests use MERCHANT tokens (remember to keep these safe and secure).
var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_MERCHANT_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

// Specify the payment request ID
string paymentRequestID = "b2d3c9b2-e5f0-4074-988b-08d9f65a6611";

var paymentData = new Dictionary<string, string>();
paymentData.Add("CustomerID", "D95D20B09846BDCEE053A2598D0A378B");
paymentData.Add("PaymentInitiator", "Customer");
paymentData.Add("MerchantStandardReason", "None");
paymentData.Add("CommerceIndicator", "internet");
paymentData.Add("PreviousTransactionID", "123456789012345");

try
{
    HttpResponseMessage response = await client.PostAsync($"{baseUrl}/{paymentRequestID}/card/paywithtoken", new FormUrlEncodedContent(paymentData));
    if (response.IsSuccessStatusCode)
    {
        // The card payment response model will be returned in JSON object.
        Console.WriteLine(await response.Content.ReadFromJsonAsync<CardResponse>());
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

// type definitions for response data
record CardResponse(string authorizedAmount, string currencyCode, string responseCode, string status,
                string requestID, string transactionID, CardError error, string payerAuthenticationUrl,
                string payerAuthenticationAccessToken, int payerAuthenticationWindowWidth,
                int payerAuthenticationWindowHeight );

record CardError(int errorCode, string id, string message, string reason, string status, string[] details, string rawResponse);
record ApiProblem(string type, string title, int status, string detail);