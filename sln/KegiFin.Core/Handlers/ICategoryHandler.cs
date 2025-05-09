using KegiFin.Core.Models;
using KegiFin.Core.Requests.Categories;
using KegiFin.Core.Responses;

namespace KegiFin.Core.Handlers;

public interface ICategoryHandler
{
    Task<Response<Category?>> CreateCategoryAsync(CreateCategoryRequest request);
    Task<Response<Category?>> UpdateCategoryAsync(UpdateCategoryRequest request);
    Task<Response<Category?>> DeleteCategoryAsync(DeleteCategoryRequest request);
    Task<Response<Category?>> GetCategoryByIdAsync(GetCategoryByIdRequest request);
    Task<PagedResponse<List<Category>>> GetAllCategoriesAsync(GetAllCategoriesRequest request);
}
