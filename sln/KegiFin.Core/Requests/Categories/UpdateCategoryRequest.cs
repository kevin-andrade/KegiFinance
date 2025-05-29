using System.ComponentModel.DataAnnotations;

namespace KegiFin.Core.Requests.Categories;

public class UpdateCategoryRequest : BaseRequest
{
    public long Id { get; set; }
    [Required(ErrorMessage = "Name invalid")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description invalid")]
    [StringLength(250, ErrorMessage = "The description must be a maximum of 250 characters.")]   
    public string? Description { get; set; }
}