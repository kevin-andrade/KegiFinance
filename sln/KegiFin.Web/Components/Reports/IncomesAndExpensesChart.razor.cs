using KegiFin.Core.Handlers;
using KegiFin.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KegiFin.Web.Components.Reports;

public partial class IncomesAndExpensesChartComponent : ComponentBase
{
    #region Properties

    public ChartOptions Options { get; set; } = new();
    public List<ChartSeries>? Series { get; set; } = [];
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
        await GetIncomesAndExpensesAsync();
    }

    #endregion

    #region Private Methods

    private async Task GetIncomesAndExpensesAsync()
    {
        try
        {
            var request = new GetIncomesAndExpensesRequest();
            var result = await ReportHandler.GetIncomesAndExpensesReportAsync(request);

            if (!result.IsSuccess || result.Data is null)
            {
                Snackbar.Add(result.Message ?? string.Empty, Severity.Error);
                return;
            }
            
            var incomes = new List<double>();
            var expenses = new List<double>();

            foreach (var item in result.Data)
            {
                incomes.Add((double)item.Incomes);
                expenses.Add(-(double)item.Expenses);
                Labels.Add(GetMonthName(item.Month));
            }
            
            Options.YAxisTicks = 1000;
            Options.LineStrokeWidth = 5;
            Options.ChartPalette = ["#76FF01", Colors.Red.Default];
            Series =
            [
                new ChartSeries { Name = "Incomes", Data = incomes.ToArray() },
                new ChartSeries { Name = "Expenses", Data = expenses.ToArray() }
            ];
            
            StateHasChanged();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    #endregion

    #region Static Methods

    private static string GetMonthName(int month)
        => new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM");

    #endregion
}