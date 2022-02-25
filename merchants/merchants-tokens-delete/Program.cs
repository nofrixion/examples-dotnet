//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API merchants/tokens/{tokenId}  
// DELETE method. It provides a convenient way to delete a merchant token.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful HTTP status code "OK" will be displayed (and the token will
//    no longer be listed using the merchant/tokens GET method).
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/merchants/tokens";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

string tokenId = "eae5022d-40f8-4811-9d4f-49243795cf7c";

try
{
    var response = await client.DeleteAsync($"{baseUrl}/{tokenId}");
    if (response.IsSuccessStatusCode)
    {   
        //HTTP staus "OK" on success.
        Console.WriteLine(response.StatusCode);
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

// Type definitions for returned data
record ApiProblem(string type, string title, int status, string detail);