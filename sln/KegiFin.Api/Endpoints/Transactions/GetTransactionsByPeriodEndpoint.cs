using System.Security.Claims;
using KegiFin.Api.Common.Api;
using KegiFin.Core;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Transactions;
using KegiFin.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace KegiFin.Api.Endpoints.Transactions;

public class GetTransactionsByPeriodEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandlerAsync)
            .WithName("Transactions: Get transactions by period")
            .WithSummary("Get all transactions by period")
            .WithDescription("Get all transactions by period associated with the authenticated user.")
            .WithOrder(5)
            .Produces<PagedResponse<List<Category>?>>();
        
    private static async Task<IResult> HandlerAsync(
        ClaimsPrincipal user,
        ITransactionHandler handler,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetTransactionsByPeriodRequest
        {
            UserId = user.Identity?.Name ?? string.Empty,
            PageNumber = pageNumber,
            PageSize = pageSize,
            StartDate = startDate,
            EndDate = endDate,
        };
        var result = await handler.GetTransactionsByPeriodAsync(request);
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}