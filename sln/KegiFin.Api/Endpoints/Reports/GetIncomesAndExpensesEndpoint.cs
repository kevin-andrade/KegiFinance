using System.Security.Claims;
using KegiFin.Api.Common.Api;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models.Reports;
using KegiFin.Core.Requests.Reports;
using KegiFin.Core.Responses;

namespace KegiFin.Api.Endpoints.Reports;

public class GetIncomesAndExpensesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/incomes-and-expenses", HandlerAsync)
            .WithName("Reports: Get Incomes and Expenses")
            .WithSummary("Retrieves a monthly report of incomes and expenses.")
            .WithDescription(
                "Returns a list of total incomes and expenses grouped by month and year for the authenticated user")
            .Produces<Response<IncomesAndExpenses>>();

    private static async Task<IResult> HandlerAsync(
        ClaimsPrincipal user,
        IReportHandler handler)
    {
        var request = new GetIncomesAndExpensesRequest { UserId = user.Identity?.Name ?? string.Empty};
        var result = await handler.GetIncomesAndExpensesReportAsync(request);
        
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}