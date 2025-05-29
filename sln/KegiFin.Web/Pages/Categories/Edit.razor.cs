using KegiFin.Core.Handlers;
using KegiFin.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KegiFin.Web.Pages.Categories;

public partial class EditCategoryPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public UpdateCategoryRequest InputModel { get; set; } = new();

    #endregion

    #region Parameters

    [Parameter]
    public string Id { get; set; } = string.Empty;

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        if (!long.TryParse(Id, out var parsedId))
        {
            Snackbar.Add("Parameter invalid", Severity.Error);
            return;
        }
        var request = new GetCategoryByIdRequest { Id = parsedId };
        
        IsBusy = true;
        try
        {
            var response = await Handler.GetCategoryByIdAsync(request);

            if (response.IsSuccess && response.Data is not null)
            {
                InputModel = new UpdateCategoryRequest
                {
                    Id = response.Data.Id,
                    Name = response.Data.Name,
                    Description = response.Data.Description
                };
                Snackbar.Add(response.Message ?? string.Empty, Severity.Success);
            }
            else
            {
                Snackbar.Add(response.Message ?? string.Empty, Severity.Error);
            }
                
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

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        try
        {
            var result = await Handler.UpdateCategoryAsync(InputModel);
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