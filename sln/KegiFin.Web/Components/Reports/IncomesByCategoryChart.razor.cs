using KegiFin.Core.Handlers;
using KegiFin.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KegiFin.Web.Components.Reports;

public partial class IncomesByCategoryChartComponent : ComponentBase
{
    #region Properties

    public List<double> Data { get; set; } = [];
    public List<string> Labels { get; set; } = [];

    #endregion

    #region Services

    [Inject]
    public IReportHandler ReportHandler { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await GetIncomesByCategoryAsync();
    }

    #endregion

    #region Private Methods

    private async Task GetIncomesByCategoryAsync()
    {
        try
        {
            var request = new GetIncomesByCategoryRequest();
            var result = await ReportHandler.GetIncomesByCategoryReportAsync(request);

            if (!result.IsSuccess || result.Data is null)
            {
                Snackbar.Add(result.Message ?? string.Empty, Severity.Error);
                return;
            }
            
            Labels.Clear();
            Labels.Clear();

            foreach (var item in result.Data)
            {
                Labels.Add($"{item.Category} ({item.Incomes.ToString("C")})");
                Data.Add((double)item.Incomes);
            }
        }
        catch (Exception)
        {
            Snackbar.Add("An unexpected error occurred while loading the data. Please try again later.", Severity.Error);
        }
    }

    #endregion
}