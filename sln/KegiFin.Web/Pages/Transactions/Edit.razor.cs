using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Categories;
using KegiFin.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KegiFin.Web.Pages.Transactions;

public partial class EditTransactionPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public UpdateTransactionRequest InputModel { get; set; } = new();
    public List<Category> Categories { get; set; } = [];

    #endregion

    #region Parameters

    [Parameter]
    public string Id { get; set; } = string.Empty;

    #endregion

    #region Services

    [Inject]
    public ITransactionHandler TransactionHandler { get; set; } = null!;
    
    [Inject]
    public ICategoryHandler CategoryHandler { get; set; } = null!;
    
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject] 
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        
        await GetTransactionByIdAsync();
        await GetCategoriesAsync();

        IsBusy = false;
    }
    
    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        try
        {
            var result = await TransactionHandler.UpdateTransactionAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message ?? string.Empty, Severity.Success);
                NavigationManager.NavigateTo("/transactions/history");
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

    #region Private Methods

    private async Task GetTransactionByIdAsync()
    {
        IsBusy = true;
        try
        {
            if (!long.TryParse(Id, out var parsedId))
            {
                Snackbar.Add("Parameter invalid", Severity.Error);
                return;
            }

            var request = new GetTransactionByIdRequest { Id = parsedId };
            var result = await TransactionHandler.GetTransactionByIdAsync(request);

            if (result.IsSuccess && result.Data is not null)
            {
                InputModel = new UpdateTransactionRequest
                {
                    Amount = result.Data.Amount,
                    CategoryId = result.Data.CategoryId,
                    Id = result.Data.Id,
                    Name = result.Data.Name,
                    PaidOrReceivedAt = result.Data.PaidOrReceivedAt,
                    Type = result.Data.Type
                };
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

    private async Task GetCategoriesAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetAllCategoriesRequest();
            var result = await CategoryHandler.GetAllCategoriesAsync(request);
            
            if (result.IsSuccess)
                Categories = result.Data ?? [];
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