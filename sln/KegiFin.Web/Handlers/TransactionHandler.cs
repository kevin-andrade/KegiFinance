using System.Net.Http.Json;
using KegiFin.Core.Common.Extensions;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Transactions;
using KegiFin.Core.Responses;

namespace KegiFin.Web.Handlers;

public class TransactionHandler(IHttpClientFactory httpClientFactory) : ITransactionHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Response<Transaction?>> CreateTransactionAsync(CreateTransactionRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/transactions", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, "Error creating transaction", (int)result.StatusCode);
    }

    public async Task<Response<Transaction?>> UpdateTransactionAsync(UpdateTransactionRequest request)
    {
        var result = await _client.PutAsJsonAsync($"v1/transactions/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, "Error update transaction", (int)result.StatusCode);
    }

    public async Task<Response<Transaction?>> DeleteTransactionAsync(DeleteTransactionRequest request)
    {
        var result = await _client.DeleteAsync($"v1/transactions/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, "Error delete transaction", (int)result.StatusCode);
    }

    public async Task<Response<Transaction?>> GetTransactionByIdAsync(GetTransactionByIdRequest request)
        => await _client.GetFromJsonAsync<Response<Transaction?>>($"v1/transactions/{request.Id}")
            ?? new Response<Transaction?>(null, "Error getting transaction by Id", 400);

    public async Task<PagedResponse<List<Transaction>?>> GetTransactionsByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        const string formatDate = "yyyy-MM-dd";
        var startDate = request.StartDate is not null
            ? request.StartDate.Value.ToString(formatDate)
            : DateTime.Now.GetFirstDay().ToString(formatDate);
        
        var endDate = request.EndDate is not null
            ? request.EndDate.Value.ToString(formatDate)
            : DateTime.Now.GetLastDay().ToString(formatDate);
        
        var url = $"v1/transactions?startDate={startDate}&endDate={endDate}";
        return await _client.GetFromJsonAsync<PagedResponse<List<Transaction>?>>($"{url}")
            ?? new PagedResponse<List<Transaction>?>(null, "Error getting transactions by period", 400);
    }
}