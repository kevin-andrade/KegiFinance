using KegiFin.Core.Requests.Accounts;
using KegiFin.Core.Responses;

namespace KegiFin.Core.Handlers;

public interface IAccountHandler
{
    Task<Response<string>> LoginAccountAsync(LoginAccountRequest request);
    Task<Response<string>> RegisterAccountAsync(RegisterAccountRequest request);
    Task LogoutAccountAsync();
}