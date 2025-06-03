using System.Security.Claims;
using KegiFin.Api.Common.Api;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models.Reports;
using KegiFin.Core.Requests.Reports;
using KegiFin.Core.Responses;

namespace KegiFin.Api.Endpoints.Reports;

public class GetIncomesByCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/incomes-by-category", HandlerAsync)
            .WithName("Reports: Get Incomes by Category")
            .WithSummary("Retrieves incomes grouped by category.")
            .WithDescription("Returns a report of all incomes grouped by category for the authenticated user")
            .Produces<Response<IncomesByCategory>?>();

    private static async Task<IResult> HandlerAsync(
        ClaimsPrincipal user,
        IReportHandler handler)
    {
        var request = new GetIncomesByCategoryRequest { UserId = user.Identity?.Name ?? string.Empty};
        var result = await handler.GetIncomesByCategoryReportAsync(request);
        
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}