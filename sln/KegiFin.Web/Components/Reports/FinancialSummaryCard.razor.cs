using KegiFin.Core.Handlers;
using KegiFin.Core.Models.Reports;
using KegiFin.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KegiFin.Web.Components.Reports;

public partial class FinancialSummaryCardComponent : ComponentBase
{
    #region Properties

    public FinancialSummary? Summary { get; set; }

    #endregion

    #region Parameters

    [Parameter]
    public bool ShowValues { get; set; }

    #endregion

    #region Services

    [Inject] public IReportHandler ReportHandler { get; set; } = null!;

    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await GetFinancialSummaryAsync();
    }

    #endregion

    #region Private Methods

    private async Task GetFinancialSummaryAsync()
    {
        try
        {
            var request = new GetFinancialSummaryRequest();
            var result = await ReportHandler.GetFinancialSummaryReportAsync(request);

            if (result.IsSuccess && result.Data is not null)
                Summary = result.Data;
            else
                Snackbar.Add(result.Message ?? string.Empty, Severity.Error);
        }
        catch (Exception)
        {
            Snackbar.Add("An unexpected error occurred while loading the data. Please try again later.",
                Severity.Error);
        }
    }

    protected string FormatValue(decimal value)
        => ShowValues ? value.ToString("C") : "R$: *******";

    #endregion
}