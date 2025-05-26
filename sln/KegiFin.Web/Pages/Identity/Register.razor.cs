using KegiFin.Core.Handlers;
using KegiFin.Core.Requests.Accounts;
using KegiFin.Web.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace KegiFin.Web.Pages.Identity;

public partial class RegisterPage : ComponentBase
{
    #region DependencyInjection
    
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public IAccountHandler AccountHandler { get; set; } = null!;
    
    [Inject]
    public ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    
    #endregion

    #region Properties

    public bool IsBusy { get; set; } = false;
    public RegisterAccountRequest InputModel { get; set; } = new();

    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        
        if (user.Identity is not null && user.Identity.IsAuthenticated)
            NavigationManager.NavigateTo("/login");
    }

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await AccountHandler.RegisterAccountAsync(InputModel);

            if (result.IsSuccess)
            {
                Snackbar.Add(
                    result.Message ?? "An error occurred during registration.",
                    Severity.Success);
                NavigationManager.NavigateTo("/login");
            }
            else
                Snackbar.Add(result.Message ?? string.Empty, Severity.Error);
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
    
}