//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API 
// paymentrequests/{id}/card/capture POST method. It captures payment for a   
// previously authorised card payment.
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

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/paymentrequests";

// Payment requests use MERCHANT tokens (remember to keep these safe and secure).
var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_MERCHANT_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");


// Specify the payment request ID (URL param)
string paymentRequestID = "187ec02c-860f-4414-ccb5-08da00f4d66d";

// Specify the authorizationID and amount to be captured from the card in the request body.
var postData = new Dictionary<string, string>();
postData.Add("authorizationID", "6467848603196559404005");
postData.Add("amount", "0.10");

try
{
    HttpResponseMessage response = await client.PostAsync($"{baseUrl}/{paymentRequestID}/card/capture", new FormUrlEncodedContent(postData));
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