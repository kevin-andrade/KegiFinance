using KegiFin.Core.Common.Extensions;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KegiFin.Web.Pages.Transactions;

public partial class ListTransactionsPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    protected List<Transaction> Transactions { get; set; } = [];
    protected string SearchTerm { get; set; } = string.Empty;
    public int CurrentMonth { get; set; } = DateTime.Now.Month;
    public int CurrentYear { get; set; } = DateTime.Now.Year;
    
    public int[] Years { get; set; } =
    [
        DateTime.Now.Year,
        DateTime.Now.Year - 1,
        DateTime.Now.Year - 2,
        DateTime.Now.Year - 3
    ];


    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public IDialogService DialogService { get; set; } = null!;
    
    [Inject]
    public ITransactionHandler TransactionHandler { get; set; } = null!;
    

    #endregion

    #region Functions

    public Func<Transaction, bool> Filter => transaction =>
    {
        if (string.IsNullOrEmpty(SearchTerm))
            return true;

        return transaction.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
               || transaction.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
               || transaction.Type.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
    };

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync() => await GetTransactionsAsync();

    #endregion

    #region Methods

    public async Task OnClearFiltersAsync()
    {
        CurrentMonth = DateTime.Now.Month;
        CurrentYear = DateTime.Now.Year;
        await GetTransactionsAsync();
    }

    
    public async Task OnSearchButtonClickAsync() => await GetTransactionsAsync();
    
    protected async Task OnDeleteButtonClickAsync(long id, string name)
    {
        var result = await DialogService.ShowMessageBox(
            "WARNING",
            $"Are you sure you want to delete the transaction {name}?",
            "DELETE",
            cancelText:"Cancel");

        if (result is true)
         await DeleteAsync(id, name);
        
        StateHasChanged();
    }

    private async Task DeleteAsync(long id, string name)
    {
        IsBusy = true;   
        try
        {
            var request = new DeleteTransactionRequest { Id = id };
            var result = await TransactionHandler.DeleteTransactionAsync(request);
            if (result.IsSuccess)
            {
                Transactions.RemoveAll(x => x.Id == id);
                Snackbar.Add($"Transaction {name} delete successfully!", Severity.Success);
            }
            else
                Snackbar.Add(
                    result.Message ??
                    $"Unable to delete the transaction \"{name}\". Please try again or contact support.",
                    Severity.Error);
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

    private async Task GetTransactionsAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetTransactionsByPeriodRequest
            {
                StartDate = DateTime.Now.GetFirstDay(CurrentYear, CurrentMonth),
                EndDate = DateTime.Now.GetLastDay(CurrentYear, CurrentMonth),
                PageNumber = Core.Configuration.DefaultPageNumber,
                PageSize = Core.Configuration.DefaultPageSize
            };
            var result = await TransactionHandler.GetTransactionsByPeriodAsync(request);
            
            if (result.IsSuccess)
                Transactions = result.Data ?? [];
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