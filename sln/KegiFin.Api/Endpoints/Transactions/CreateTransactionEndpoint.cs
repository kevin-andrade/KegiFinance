using KegiFin.Api.Common.Api;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Transactions;
using KegiFin.Core.Responses;

namespace KegiFin.Api.Endpoints.Transactions;

public class CreateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandlerAsync)
            .WithName("Transaction: Create")
            .WithSummary("Create a transaction")
            .WithOrder(1)
            .Produces<Response<Transaction>>();

    private static async Task<IResult> HandlerAsync(
        ITransactionHandler handler,
        CreateTransactionRequest request)
    {
        var result = await handler.CreateTransactionAsync(request);
        return result.IsSuccess
            ? Results.Created($"/{result.Data?.Id}", result)
            : Results.BadRequest(result);
    }
}