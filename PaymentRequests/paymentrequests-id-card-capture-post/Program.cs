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
using System.Text;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/paymentrequests";

// Payment requests use MERCHANT tokens (remember to keep these safe and secure).
var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_MERCHANT_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");


// Specify the payment request ID and amount to be captured from the card.
string paymentRequestID = "e111f205-e966-4f2f-988a-08d9f65a6611";
string paymentAmount = "0.50";

var postData = new StringContent($"amount={paymentAmount}", Encoding.UTF8, "application/x-www-form-urlencoded");

try
{
    HttpResponseMessage response = await client.PostAsync($"{baseUrl}/{paymentRequestID}/card/capture", postData);
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