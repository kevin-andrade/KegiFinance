using System.ComponentModel.DataAnnotations;
using KegiFin.Core.Enums;

namespace KegiFin.Core.Requests.Transactions;

public class UpdateTransactionRequest : BaseRequest
{
    public long Id { get; set; }
    
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name must be at most 100 characters long.")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Transaction type is required.")]
    [EnumDataType(typeof(ETransactionType), ErrorMessage = "Invalid transaction type.")]
    public ETransactionType Type { get; set; } = ETransactionType.Withdraw;
    
    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }
    
    [Required(ErrorMessage = "Category is required.")]
    [Range(1, long.MaxValue, ErrorMessage = "Invalid category ID.")]
    public long CategoryId { get; set; }
    
    [Required(ErrorMessage = "Date invalid")]
    public DateTime? PaidOrReceivedAt { get; set; }
}