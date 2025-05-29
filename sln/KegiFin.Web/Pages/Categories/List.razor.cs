using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KegiFin.Web.Pages.Categories;

public partial class ListCategoriesPage : ComponentBase
{
    #region Propperties

    public bool IsBusy { get; set; } = false;
    protected List<Category> Categories = [];
    protected string SearchTerm { get; set; } = string.Empty;

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    
    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;
    
    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetAllCategoriesRequest();
            var result = await Handler.GetAllCategoriesAsync(request);

            if (result.IsSuccess)
            {
                Categories = result.Data ?? [];
                Snackbar.Add(result.Message ?? string.Empty, Severity.Success);
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

    #region Methods

    public Func<Category, bool> Filter
        => category =>
        {
            if (string.IsNullOrEmpty(SearchTerm))
                return true;

            if (category.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;
            
            if (category.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;
            
            if (category.Description is not null &&  category.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;
            
            return false;
        };

    protected async Task OnDeleteButtonClickAsync(long id, string name)
    {
        var result = await DialogService.ShowMessageBox(
            "WARNING",
            $"Are you sure you want to delete the category {name}?",
            "DELETE",
            cancelText:"Cancel");

        if (result is true)
            await OnDeleteAsync(id, name);
        
        StateHasChanged();
    }

    private async Task OnDeleteAsync(long id, string name)
    {
        try
        {
            var request = new DeleteCategoryRequest { Id = id };
            var result = await Handler.DeleteCategoryAsync(request);
            if (result.IsSuccess)
            {
                Categories.RemoveAll(x => x.Id == id);
                Snackbar.Add($"Category {name} delete successfully!", Severity.Success);
            }
            else
                Snackbar.Add(result.Message ?? $"Unable to delete the category \"{name}\". Please try again or contact support.", Severity.Error);
            
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
    }

    #endregion
}