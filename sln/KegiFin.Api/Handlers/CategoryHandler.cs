using KegiFin.Api.Data;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Categories;
using KegiFin.Core.Responses;

namespace KegiFin.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<Response<Category>> CreateCategoryAsync(CreateCategoryRequest request)
    {
        try
        {
            var category = new Category
            {
                UserId = request.UserId,
                Name = request.Name,
                Description = request.Description
            };
            
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return new Response<Category>(category, "Category created successfully");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public Task<Response<Category>> UpdateCategoryAsync(UpdateCategoryRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Category>> DeleteCategoryAsync(DeleteCategoryRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Category>> GetCategoryByIdAsync(GetCategoryByIdRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<List<Category>>> GetAllCategoriesAsync(GetAllCategoriesRequest request)
    {
        throw new NotImplementedException();
    }
}