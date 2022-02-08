//-----------------------------------------------------------------------------
// Description: Example of calling the NoFrixion MoneyMoov API Metadata WhoAmi
// method. It provides a convenient way to check that a JWT access token is valid.
//
// Usage:
// 1. Create a user access token in the sandbox portal at:
//    https://portal-sandbox.nofrixion.com.
// 2. Set the token as an environment variable in your console:
//    set NOFRIXION_SANDBOX_TOKEN=<JWT token from previous step>
// 3. Run the applicatio using:
//    dotnet run
// 4. If successful a GUID, representing your user ID, will be displayed on 
//    the console.
//-----------------------------------------------------------------------------

using System.Net.Http.Headers;

const string SANDBOX_METADATA_WHOAMI_URL = "https://api-sandbox.nofrixion.com/api/v1/metadata/whoami";

var jwtToken = Environment.GetEnvironmentVariable("NOFRIXION_SANDBOX_TOKEN");

var client = new HttpClient();
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
Console.WriteLine(await client.GetStringAsync(SANDBOX_METADATA_WHOAMI_URL));