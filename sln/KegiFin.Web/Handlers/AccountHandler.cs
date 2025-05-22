using System.Net.Http.Json;
using System.Text;
using KegiFin.Core.Handlers;
using KegiFin.Core.Requests.Accounts;
using KegiFin.Core.Responses;

namespace KegiFin.Web.Handlers;

public class AccountHandler(IHttpClientFactory httpClientFactory) : IAccountHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Response<string>> LoginAccountAsync(LoginAccountRequest request)
    {
        var result = await _client.PostAsJsonAsync(
            "v1/identity/login?useCookies=true", request);

        return result.IsSuccessStatusCode
            ? new Response<string>("Login successful!", "Login successful!", (int)result.StatusCode)
            : new Response<string>(null, "Invalid Login or Password", (int)result.StatusCode);
    }

    public async Task<Response<string>> RegisterAccountAsync(RegisterAccountRequest request)
    {
        var result = await _client.PostAsJsonAsync(
            "v1/identity/register", request);

        return result.IsSuccessStatusCode
            ? new Response<string>("User registered successfully!", "Register successful!", (int)result.StatusCode)
            : new Response<string>(null, "It was not possible to register the user.", (int)result.StatusCode);
    }

    public async Task LogoutAccountAsync()
    {
        var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
        await _client.PostAsync("v1/identity/logout", emptyContent);
    }
}