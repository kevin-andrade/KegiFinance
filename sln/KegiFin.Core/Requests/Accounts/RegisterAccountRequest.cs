using System.ComponentModel.DataAnnotations;

namespace KegiFin.Core.Requests.Accounts;

public class RegisterAccountRequest : BaseRequest
{
    [Required(ErrorMessage = "E-mail")]
    [EmailAddress(ErrorMessage = "E-mail invalid")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password invalid")]
    public string Password { get; set; } = string.Empty;
}