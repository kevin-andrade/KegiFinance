using KegiFin.Core.Enums;
using KegiFin.Core.Models;

namespace KegiFin.Tests.Unit.Api.Handlers.Reports.TestUtils.Seeds;

public static class TransactionSeed
{
    public static List<Transaction> GetTransactions(string userId = "user-1")
        =>
        [
            new()
            {
                Id = 1,
                Name = "Salário",
                PaidOrReceivedAt = DateTime.Now, // dentro do mês atual
                Type = ETransactionType.Deposit,
                Amount = 5000,
                CategoryId = 1,
                Category = new Category { Id = 1, Name = "Renda" },
                UserId = userId
            },
            new()
            {
                Id = 2,
                Name = "Aluguel",
                PaidOrReceivedAt = DateTime.Now,
                Type = ETransactionType.Withdraw,
                Amount = 1200,
                CategoryId = 2,
                Category = new Category { Id = 2, Name = "Moradia" },
                UserId = userId
            },
            new()
            {
                Id = 3,
                Name = "Mercado",
                PaidOrReceivedAt = DateTime.Now,
                Type = ETransactionType.Withdraw,
                Amount = 800,
                CategoryId = 3,
                Category = new Category { Id = 3, Name = "Alimentação" },
                UserId = userId
            }
        ];
    
    public static List<Transaction> GetTransactionsWithOtherUserOnly(string userId = "user-2")
        =>
        [
            new()
            {
                Id = 1,
                Name = "Salário",
                PaidOrReceivedAt = DateTime.Now,
                Type = ETransactionType.Deposit,
                Amount = 5000,
                CategoryId = 1,
                Category = new Category { Id = 1, Name = "Renda" },
                UserId = userId
            },
            new()
            {
                Id = 2,
                Name = "Aluguel",
                PaidOrReceivedAt = DateTime.Now,
                Type = ETransactionType.Withdraw,
                Amount = 1200,
                CategoryId = 2,
                Category = new Category { Id = 2, Name = "Moradia" },
                UserId = userId
            },
            new()
            {
                Id = 3,
                Name = "Mercado",
                PaidOrReceivedAt = DateTime.Now,
                Type = ETransactionType.Withdraw,
                Amount = 800,
                CategoryId = 3,
                Category = new Category { Id = 3, Name = "Alimentação" },
                UserId = userId
            }
        ];
    
    public static List<Transaction> GetOldTransactionsForUser(string userId = "user-1")
    =>
        [
            new()
            {
                Id = 1,
                Name = "Salário",
                PaidOrReceivedAt = DateTime.Now.AddYears(-1),
                Type = ETransactionType.Deposit,
                Amount = 5000,
                CategoryId = 1,
                Category = new Category { Id = 1, Name = "Renda" },
                UserId = userId
            },
            new()
            {
                Id = 2,
                Name = "Aluguel",
                PaidOrReceivedAt = DateTime.Now.AddMonths(-2),
                Type = ETransactionType.Withdraw,
                Amount = 1200,
                CategoryId = 2,
                Category = new Category { Id = 2, Name = "Moradia" },
                UserId = userId
            },
        ];
}