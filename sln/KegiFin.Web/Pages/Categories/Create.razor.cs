using KegiFin.Core.Handlers;
using KegiFin.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KegiFin.Web.Pages.Categories;

public partial class CreateCategoryPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    protected CreateCategoryRequest InputModel { get; set; } = new();

    #endregion

    #region Services

    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;
    
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    
    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        var result = await Handler.CreateCategoryAsync(InputModel);

        try
        {
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message ?? string.Empty, Severity.Success);
                NavigationManager.NavigateTo("/categories");   
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