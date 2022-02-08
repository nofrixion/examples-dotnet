//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API Metadata Verion
// method. It provides a convenient way to check the current version of the API.
//
// Usage:
// 1. Run the applicatio using:
//    dotnet run
// 2. If successful a string with the current version of the version will be
// displayed.
//-----------------------------------------------------------------------------

const string SANDBOX_METADATA_VERSION_URL = "https://api-sandbox.nofrixion.com/api/v1/metadata/version";

var client = new HttpClient();
Console.WriteLine(await client.GetStringAsync(SANDBOX_METADATA_VERSION_URL));