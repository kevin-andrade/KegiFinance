using KegiFin.Core.Handlers;
using KegiFin.Core.Requests.Accounts;
using KegiFin.Web.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KegiFin.Web.Pages.Identity;

public class LogoutPage : ComponentBase
{
    #region DependencyInjection

    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    [Inject] public IAccountHandler AccountHandler { get; set; } = null!;

    [Inject] public ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        if (await AuthenticationStateProvider.CheckAuthenticatedAsync())
        {
            await AccountHandler.LogoutAccountAsync();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            AuthenticationStateProvider.NotifyAuthenticationStateChanged();
        }
        
        await base.OnInitializedAsync();
    }

    #endregion
}