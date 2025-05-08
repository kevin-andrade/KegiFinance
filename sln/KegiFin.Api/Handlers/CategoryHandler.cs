using KegiFin.Api.Data;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Categories;
using KegiFin.Core.Responses;
using ILogger = Serilog.ILogger;

namespace KegiFin.Api.Handlers;

public class CategoryHandler(AppDbContext context, ILogger<CategoryHandler> logger) : ICategoryHandler
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
            
            logger.LogInformation($"Category created successfully Id: {category.Id}");
            
            return new Response<Category>(category, "Category created successfully");
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Error creating category Id: {userId} | Name: {name}",
                request.UserId, request.Name);
            
            return new Response<Category>(null, "Error creating category");
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