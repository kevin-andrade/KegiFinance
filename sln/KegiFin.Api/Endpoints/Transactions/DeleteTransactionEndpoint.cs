using KegiFin.Api.Common.Api;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Transactions;
using KegiFin.Core.Responses;

namespace KegiFin.Api.Endpoints.Transactions;

public class DeleteTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandlerAsync)
            .WithName("Transactions: Delete")
            .WithSummary("Delete a transaction")
            .WithDescription("Delete a transaction associated with the authenticated user.")
            .WithOrder(3)
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandlerAsync(
        ITransactionHandler handler,
        long id)
    {
        var request = new DeleteTransactionRequest{Id = id, UserId = "test1"};
        var result = await handler.DeleteTransactionAsync(request);
        
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}