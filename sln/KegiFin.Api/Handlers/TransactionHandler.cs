using KegiFin.Api.Data;
using KegiFin.Core.Common.Extensions;
using KegiFin.Core.Enums;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Transactions;
using KegiFin.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace KegiFin.Api.Handlers;

public class TransactionHandler(AppDbContext context, ILogger<TransactionHandler> logger) : ITransactionHandler
{
    public async Task<Response<Transaction?>> CreateTransactionAsync(CreateTransactionRequest request)
    {
        if (request is {Type: ETransactionType.Withdraw, Amount: >=0})
            request.Amount *= -1;
        
        try
        {
            var transaction = new Transaction
            {
                UserId = request.UserId,
                Name = request.Name,
                Type = request.Type,
                Amount = request.Amount,
                CategoryId = request.CategoryId,
                PaidOrReceivedAt = request.PaidOrReceivedAt
            };
            
            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Transaction created successfully Id: {transaction.Id}");
            
            return new Response<Transaction?>(transaction, "Transaction created successfully", 201);
            
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Error creating transaction userId: {userId} | Name: {name}",
                request.UserId, request.Name);
            
            return new Response<Transaction?>(null, "Error creating Transaction", 500);
        }
    }

    public async Task<Response<Transaction?>> UpdateTransactionAsync(UpdateTransactionRequest request)
    {
        if (request is {Type: ETransactionType.Withdraw, Amount: >0})
            request.Amount *= -1;
        
        try
        {
            var transaction = await context
                .Transactions
                .FirstOrDefaultAsync(
                    (x => x.Id == request.Id && x.UserId == request.UserId));
            
            if (transaction is null)
                return new Response<Transaction?>(null, "Transaction not found", 404);
            
            transaction.Name = request.Name;
            transaction.Type = request.Type;
            transaction.Amount = request.Amount;
            transaction.CategoryId = request.CategoryId;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;
            
            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Transaction successfully updated Id: {transaction.Id}");
            
            return new Response<Transaction?>(transaction, "Transaction created successfully");
            
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Error update transaction userId: {userId} | Name: {name}",
                request.UserId, request.Name);
            
            return new Response<Transaction?>(null, "Error creating Transaction", 500);
        }
    }

    public async Task<Response<Transaction?>> DeleteTransactionAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .FirstOrDefaultAsync(
                    (x => x.Id == request.Id && x.UserId == request.UserId));
            
            if (transaction is null)
                return new Response<Transaction?>(null, "Transaction not found", 404);
            
            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Transaction successfully delete Id: {transaction.Id}");
            
            return new Response<Transaction?>(transaction, "Transaction deleted successfully");
            
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Error delete transaction userId: {userId}",
                request.UserId);
            
            return new Response<Transaction?>(null, "Error creating Transaction", 500);
        }
    }

    public async Task<Response<Transaction?>> GetTransactionByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && x.UserId == request.UserId);

            return transaction is null
                ? new Response<Transaction?>(null, "Transaction not found", 404)
                : new Response<Transaction?>(transaction);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Error load transaction by Id: {userId} | Id: {name}",
                request.UserId, request.Id);
            
            return new Response<Transaction?>(null, "Error load category", 500);
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetTransactionsByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        try
        {
            request.StartDate ??= DateTime.Now.GetFirstDay();
            request.EndDate ??= DateTime.Now.GetLastDay();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error load transactions by period");
        }

        try
        {
            var query = context
                .Transactions
                .AsNoTracking()
                .Where(
                    x => x.CreatedAt >= request.StartDate && x.CreatedAt <= request.EndDate
                                                          && x.UserId == request.UserId)
                .OrderBy(x => x.CreatedAt);
        
            var transactions = await query
                .Skip((request.PageNumber - 1)  * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();;
        
            var count = await query.CountAsync();
        
            return new PagedResponse<List<Transaction>?>(
                transactions,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error load transactions by period");
            return new PagedResponse<List<Transaction>?>(null, "Error load transactions by period", 500);
        }
    }
}