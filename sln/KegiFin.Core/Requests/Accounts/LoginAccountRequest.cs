using System.ComponentModel.DataAnnotations;

namespace KegiFin.Core.Requests.Accounts;

public class LoginAccountRequest : BaseRequest
{
    [Required(ErrorMessage = "E-mail is required")]
    [EmailAddress(ErrorMessage = "E-mail invalid")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password invalid")]
    public string Password { get; set; } = string.Empty;
}