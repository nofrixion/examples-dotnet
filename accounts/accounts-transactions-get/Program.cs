//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API Accounts/{accounId}/transactions 
// GET method. It provides a convenient way to retrieve a list of transactions for the specified
// account.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful a list of your transactions and some metadata will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string baseUrl = "https://api-sandbox.nofrixion.com/api/v1/accounts";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_USER_TOKEN");

// Id of account to get transaction history from 
string accountID = "A120P0JR";

// by default each call will return 20 transactions, we can change this using a query parameter as shown below.
// a start date or page number can also be requested using query parameters
string queryParams = "?size=10";


var client = new HttpClient();
client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync($"{baseUrl}/{accountID}/transactions{queryParams}");
    response.EnsureSuccessStatusCode();

    // response body contains some transaction page metadata JSON array of transactions
    var page = await response.Content.ReadFromJsonAsync<TransactionPage>();
    if (page != null)
    {
        // User some of the page metadata
        Console.WriteLine($"Showing {page.page + 1} of {page.totalPages} ({page.size} of {page.totalSize} transactions)");
        // Show the returned transactions
        foreach (Transaction trans in page.transactions){
            Console.WriteLine(trans);
        }
    }
    else
    {
        Console.WriteLine($"No transactions returned.");
    }
}
catch (Exception e) {
    Console.WriteLine($"Error: {e.Message}");
}

// Type definitiions for returned data
// - This endpoint returns a "TransactionPageResponse": some metadata about the page and an array of transactions,
//   the length of which is specified by the 'size' query parameter
// - the example here uses a limited set of properties. A full list of properites for returned can be found at
//   https://api-sandbox.nofrixion.com/swagger/v1/swagger.json in the TransactionPageResponse schema.
record Transaction(decimal amount, string currency, string description, string id, string transactionDate);
record TransactionPage(List<Transaction> transactions, int page, double pageStartBalance, int size, int totalPages, int totalSize);


