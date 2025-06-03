using System.Security.Claims;
using KegiFin.Api.Common.Api;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models.Reports;
using KegiFin.Core.Requests.Reports;
using KegiFin.Core.Responses;

namespace KegiFin.Api.Endpoints.Reports;

public class GetExpensesByCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/expenses-by-category", HandlerAsync)
            .WithName("Reports: Get Expenses by Category")
            .WithSummary("Retrieves expenses grouped by category.")
            .WithDescription("Returns an expense report grouped by category for the authenticated user, based on transactions from the past year.")
            .Produces<Response<ExpensesByCategory>?>();

    private static async Task<IResult> HandlerAsync(
        ClaimsPrincipal user,
        IReportHandler handler)
    {
        var request = new GetExpensesByCategoryRequest{ UserId = user.Identity?.Name ?? string.Empty};
        var result = await handler.GetExpensesByCategoryReportAsync(request);
        
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}