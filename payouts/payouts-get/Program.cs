//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API payouts GET
// method. It provides a convenient way to retrieve a list of pending payouts
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_USER_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful a list of selected payout details will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/payouts";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

var client = new HttpClient();
client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

// Note optional paramaters for paging the payout list are exposed in the API
// - see https://api-sandbox.nofrixion.com/swagger/index.html for full details
try
{
    var response = await client.GetAsync(baseUrl);
    if (response.IsSuccessStatusCode)
    {
        // returns a list of payouts
        var payoutsPage = await response.Content.ReadFromJsonAsync<PayoutsPage>();
        if (payoutsPage != null)
        {
            // Do something with response...
            Console.WriteLine($"Showing page {payoutsPage.pageNumber} of {payoutsPage.totalPages}.");
            foreach (var payout in payoutsPage.content)
            {
                Console.WriteLine($"Send {payout.currency} {payout.amount:0.00} to {payout.destinationIban} ({payout.yourReference})");
            }
        }
        else
        {
            Console.WriteLine("No payouts returned.");
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

// type definitions for returned data
record Payout(string id, string accountID, string userID, string type, string description,
            string currency, decimal amount, string yourReference, string destinationIban,
            string destinationAccountName, string theirReference);
record PayoutsPage(List<Payout> content, int pageNumber, int pageSize, int totalPages, int totalSize);
record ApiProblem(string type, string title, int status, string detail);