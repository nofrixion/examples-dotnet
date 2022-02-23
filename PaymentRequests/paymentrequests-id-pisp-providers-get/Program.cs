//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API 
// paymentrequests/{id}/pisp/providers GET method. It returns a list  
// of PISP payment providers.
//
// Usage:
// 1. Create a MERCHANT access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_MERCHANT_TOKEN=<JWT token from previous step>
/// 3. Run the applicatio using:
//    dotnet run
// 4. If successful user a list of PISP providers will be displayed.
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
    HttpResponseMessage response = await client.GetAsync($"{baseUrl}/{paymentRequestID}/pisp/providers");
    if (response.IsSuccessStatusCode)
    {
        // returns a list of PISP providers
        var providers = await response.Content.ReadFromJsonAsync<List<PispProvider>>();
        if (providers != null)
        {
            // Response is an array JSON objects containing PISP provider details
            foreach (PispProvider provider in providers)
            {
                Console.WriteLine(provider);
            }
        }
        else
        {
            Console.WriteLine("No providers returned.");
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
record PispProvider(string id, string name, string imageUrl, bool singleImmediateEnabled, bool standingOrderEnabled);
record ApiProblem(string type, string title, int status, string detail);