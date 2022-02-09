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
// 4. If successful a list of your transactions will be displayed.
//-----------------------------------------------------------------------------

using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

// Id of account to get transaction history from 
string accountId = "A120P0JR";

// by default each call will return 20 transactions, we can change this using a query parameter as shown below.
// specific pages can also be requested using query parameters
string queryParams = "?size=10";

string SANDBOX_ACCOUNTS_TRANSACTIONS_GET_URL = $"https://api-sandbox.nofrixion.com/api/v1/accounts/{accountId}/transactions{queryParams}";

var client = new HttpClient();
client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");

try
{
    var response = await client.GetAsync(SANDBOX_ACCOUNTS_TRANSACTIONS_GET_URL);
    response.EnsureSuccessStatusCode();

    // the transactions is a fairly large data structure (see https://docs.nofrixion.com/reference/get_api-v1-accounts-accountid-transactions)
    // rather than create a large class in this example, we have used NewtonSoft Json to create dynamic object from string ( as .Net's 
    // ReadFromJsonAsync method doesn't correctly deserialize to <dynamic>
    
    string transStr = await response.Content.ReadAsStringAsync();
    dynamic transObj = JObject.Parse(transStr);
    // the returned JSON contains an "transactions" property which is an array of transaction details
    Console.WriteLine(transObj.transactions);
    // and also some summary fields such as the page number and total number of page and transactions
    Console.WriteLine($"Showing page {transObj.page + 1} of {transObj.totalPages + 1}. {transObj.totalSize} transactions in total.");
}
catch (Exception e) {
    Console.WriteLine($"Error: {e.Message}");
}


