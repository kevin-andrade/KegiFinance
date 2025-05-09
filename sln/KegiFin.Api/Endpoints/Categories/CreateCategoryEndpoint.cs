using KegiFin.Api.Common.Api;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Categories;
using KegiFin.Core.Responses;

namespace KegiFin.Api.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandlerAsync)
            .WithName("Categories: Create")
            .WithSummary("Create a category")
            .WithDescription("Creates a new category associated with the authenticated user. Requires a name and optional description. Category names must be unique per user.")
            .WithOrder(1)
            .Produces<Response<Category>>();
        
        private static async Task<IResult> HandlerAsync(
            ICategoryHandler handler,
            CreateCategoryRequest request)
        {
            var result = await handler.CreateCategoryAsync(request);
            return result.IsSuccess
                ? Results.Created($"/{result.Data?.Id}", result)
                : Results.BadRequest(result);
        }
}