using System.Security.Claims;
using KegiFin.Api.Common.Api;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models.Reports;
using KegiFin.Core.Requests.Reports;
using KegiFin.Core.Responses;

namespace KegiFin.Api.Endpoints.Reports;

public class GetFinancialSummaryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/financial-summary", HandlerAsync)
            .WithName("Reports: Get Financial Summary")
            .WithSummary("Retrieves the financial summary for the authenticated user.")
            .WithDescription(
                "Returns a summary including total income and expenses for the current month, grouped by transaction type.")
            .Produces<Response<FinancialSummary>?>();

    private static async Task<IResult> HandlerAsync(
        ClaimsPrincipal user,
        IReportHandler handler)
    {
        var request = new GetFinancialSummaryRequest { UserId = user.Identity?.Name ?? string.Empty };
        var result = await handler.GetFinancialSummaryReportAsync(request);
        
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}