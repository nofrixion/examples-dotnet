//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API user/tokens 
// POST method. It provides a convenient way to intiate the creation of a new  
// user access token.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful the pre-token will be returned and the token approval URL will
//    displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/user/tokens";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

HttpContent postData = new FormUrlEncodedContent(
    new List<KeyValuePair<string, string>> {
                new KeyValuePair<string,string>("MerchantID","6f80138d-870b-4b07-8bc4-a4fd33a0d30f"),
                new KeyValuePair<string,string>("Description","API created user token")
    });
try
{
    var response = await client.PostAsync(baseUrl, postData);
    response.EnsureSuccessStatusCode();

    var preToken = await response.Content.ReadFromJsonAsync<UserToken>();
    if (preToken != null)
    {
        // The response body contains a JSON 'pre-token'. Redirect the user to the approveTokenUrl
        // where they will be asked to perform strong authentication and then redirected back
        // to the NoFrixion portal where their token and refresh token will be visible.
        Console.WriteLine(preToken.approveTokenUrl);
    }
    else
    {
        // This should never run as a token is required for the API call.
        Console.WriteLine("No user tokens created.");
    }
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}


// Type definition for response data
record UserToken(string id, string userID, string type, string description, string accessTokenHash,
            string refreshTokenHash, string inserted, string lastUpdated, string approveTokenUrl);
