using System.ComponentModel.DataAnnotations;
using KegiFin.Core.Requests.Accounts;

namespace KegiFin.Tests.Unit.Core.Requests.Accounts;

public class LoginAccountRequestTests
{
    public const string Password = "P@ssw0rd!;";
    
    private static List<ValidationResult> ValidateModel(LoginAccountRequest model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, context, results, true);
        return results;
    }
    
    [Theory]
    [InlineData("", false, "E-mail is required")] // Invalid email
    [InlineData(null, false, "E-mail is required")]
    [InlineData("invalid-email", false, "E-mail invalid")]
    [InlineData("invalid email@", false, "E-mail invalid")]
    [InlineData("user@example.com", true, null)] // Valid email
    [InlineData("user.name+tag+sorting@example.com", true, null)]
    [InlineData("user_name@example.co.uk", true, null)]
    [InlineData("user-name@example.org", true, null)]
    [InlineData("user123@example.io", true, null)]
    [InlineData("user@example.museum", true, null)]
    public void Email_WithDifferentInputs_ShouldPassOrFailValidation(string? email, bool expectedIsValid,
        string? expectedErrorMessage)
    {
        var request = new LoginAccountRequest
        {
            Email = email ?? string.Empty,
            Password = Password
        };
        
        var validationResults = ValidateModel(request);
        var isValid = validationResults.Count == 0;

        Assert.Equal(expectedIsValid, isValid);
        
        if (!expectedIsValid)
            Assert.Contains(validationResults, v => v.ErrorMessage == expectedErrorMessage);
    }
    
    [Theory]
    [InlineData("", false, "Password invalid")]        // Invalid password
    [InlineData(null, false, "Password invalid")]
    [InlineData(Password, true, null)]             // Valid password
    [InlineData("123456", true, null)]
    public void Password_WithDifferentInputs_ShouldPassOrFailValidation(string? password, bool expectedIsValid, string? expectedErrorMessage)
    {
        var request = new LoginAccountRequest
        {
            Email = "user@example.com",
            Password = password ?? string.Empty
        };

        var validationResults = ValidateModel(request);
        var isValid = validationResults.Count == 0;

        Assert.Equal(expectedIsValid, isValid);

        if (!expectedIsValid)
        {
            Assert.Contains(validationResults, v => v.ErrorMessage == expectedErrorMessage);
        }
    }
}