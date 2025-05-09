using KegiFin.Api.Common.Api;
using KegiFin.Core;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Categories;
using KegiFin.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace KegiFin.Api.Endpoints.Categories;

public class GetAllCategoriesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandlerAsync)
            .WithName("Categories: Get all")
            .WithSummary("Get all categories")
            .WithDescription("Get all categories associated with the authenticated user.")
            .WithOrder(5)
            .Produces<PagedResponse<List<Category>?>>();
        
    private static async Task<IResult> HandlerAsync(
        ICategoryHandler handler,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetAllCategoriesRequest{UserId = "test1", PageNumber = pageNumber, PageSize = pageSize};
        var result = await handler.GetAllCategoriesAsync(request);
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}