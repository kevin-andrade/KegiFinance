using KegiFin.Api.Data;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Categories;
using KegiFin.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace KegiFin.Api.Handlers;

public class CategoryHandler(IAppDbContext context, ILogger<CategoryHandler> logger) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateCategoryAsync(CreateCategoryRequest request)
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
            
            return new Response<Category?>(category, "Category created successfully", 201);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Error creating category Id: {userId} | Name: {name}",
                request.UserId, request.Name);
            
            return new Response<Category?>(null, "Error creating category", 500);
        }
    }

    public async Task<Response<Category?>> UpdateCategoryAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && x.UserId == request.UserId);

            if (category is null)
            {
                logger.LogWarning($"Category not found Id: {request.Id} UserId: {request.UserId}");
                return new Response<Category?>(null, "Category not found", 404);
            }
            
            category.Name = request.Name;
            category.Description = request.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Category successfully update Id: {category.Id}");
            
            return new Response<Category?>(category, "Category successfully update");
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Error updating category userId: {userId} | Name: {name}",
                request.UserId, request.Name);
            
            return new Response<Category?>(null, "Error updating category", 500);
        }
    }

    public async Task<Response<Category?>> DeleteCategoryAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && x.UserId == request.UserId);

            if (category is null)
            {
                logger.LogWarning($"Category not found Id: {request.Id} UserId: {request.UserId}");
                return new Response<Category?>(null, "Category not found", 404);
            }
            
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Category successfully deleted Id: {category.Id}");
            
            return new Response<Category?>(category, "Category successfully deleted");
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Error deleting category Id: {id} | UserId: {userId}",
                request.Id, request.UserId);
            
            return new Response<Category?>(null, "Error category deleted", 500);
        }
    }

    public async Task<Response<Category?>> GetCategoryByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && x.UserId == request.UserId);

            return category is null
                ? new Response<Category?>(null, "Category not found", 404)
                : new Response<Category?>(category);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Error load category by Id: {Id} | UserId: {UserId}",
                request.Id, request.UserId);
            
            return new Response<Category?>(null, "Error load category", 500);
        }
    }

    public async Task<PagedResponse<List<Category>>> GetAllCategoriesAsync(GetAllCategoriesRequest request)
    {
        try
        {
            var query = context
                .Categories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Name);

            var categories = await query
                .Skip((request.PageNumber - 1)  * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
            
            var totalCount = await query.CountAsync();
            
            return new PagedResponse<List<Category>>(
                categories,
                totalCount,
                request.PageNumber,
                request.PageSize);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Error load all categories userId: {UserId}",
                request.UserId);
            
            return new PagedResponse<List<Category>>(null, "Error load all categories", 500);
        }
    }
}