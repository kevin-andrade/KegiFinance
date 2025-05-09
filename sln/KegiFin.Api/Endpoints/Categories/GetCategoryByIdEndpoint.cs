using KegiFin.Api.Common.Api;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Categories;
using KegiFin.Core.Responses;

namespace KegiFin.Api.Endpoints.Categories;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandlerAsync)
            .WithName("Categories: Get by Id")
            .WithSummary("Return a category")
            .WithDescription("Return a category associated with the authenticated user.")
            .WithOrder(4)
            .Produces<Response<Category?>>();
        
    private static async Task<IResult> HandlerAsync(
        ICategoryHandler handler,
        long id)
    {
        var request = new GetCategoryByIdRequest{ Id = id, UserId = "test1" };
        var result = await handler.GetCategoryByIdAsync(request);
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}