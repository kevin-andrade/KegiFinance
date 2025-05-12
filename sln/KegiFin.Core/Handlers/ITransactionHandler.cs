using KegiFin.Core.Models;
using KegiFin.Core.Requests.Transactions;
using KegiFin.Core.Responses;

namespace KegiFin.Core.Handlers;

public interface ITransactionHandler
{
    Task<Response<Transaction?>> CreateTransactionAsync(CreateTransactionRequest request);
    Task<Response<Transaction?>> UpdateTransactionAsync(UpdateTransactionRequest request);
    Task<Response<Transaction?>> DeleteTransactionAsync(DeleteTransactionRequest request);
    Task<Response<Transaction?>> GetTransactionByIdAsync(GetTransactionByIdRequest request);
    Task<PagedResponse<List<Transaction>?>> GetTransactionsByPeriodAsync(GetTransactionsByPeriodRequest request);
}