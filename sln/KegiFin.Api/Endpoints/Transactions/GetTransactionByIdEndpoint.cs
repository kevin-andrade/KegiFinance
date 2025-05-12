using KegiFin.Api.Common.Api;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Transactions;
using KegiFin.Core.Responses;

namespace KegiFin.Api.Endpoints.Transactions;

public class GetTransactionByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandlerAsync)
            .WithName("Transactions: Get by Id")
            .WithSummary("Return a transaction")
            .WithDescription("Return a transaction associated with the authenticated user.")
            .WithOrder(4)
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandlerAsync(
        ITransactionHandler handler,
        long id)
    {
        var request = new GetTransactionByIdRequest{ Id = id, UserId = "test1" };
        var result = await handler.GetTransactionByIdAsync(request);
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}