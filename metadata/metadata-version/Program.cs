//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API Metadata Verion
// method. It provides a convenient way to check the current version of the API.
//
// Usage:
// 1. Run the applicatio using:
//    dotnet run
// 2. If successful a JSON object containing the current API version will be
// displayed.
//-----------------------------------------------------------------------------

using System.Net.Http.Json;

const string url = "https://api-sandbox.nofrixion.com/api/v1/metadata/version";

var client = new HttpClient();

try
{
    var response = await client.GetAsync(url);
    if (response.IsSuccessStatusCode)
    {
        // returns MoneyMoov api version object
        var apiVersion = await response.Content.ReadFromJsonAsync<ApiVersion>();
        if (apiVersion != null)
        {
            Console.WriteLine(apiVersion);
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

// Type definitions for returned data
record ApiVersion(int majorVersion, int minorVersion, int buildVersion, string releaseName);
record ApiProblem(string type, string title, int status, string detail);