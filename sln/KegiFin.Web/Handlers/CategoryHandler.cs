using System.Net.Http.Json;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Categories;
using KegiFin.Core.Responses;

namespace KegiFin.Web.Handlers;

public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    public async Task<Response<Category?>> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/categories", request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, "Error creating category", (int)result.StatusCode);
    }

    public async Task<Response<Category?>> UpdateCategoryAsync(UpdateCategoryRequest request)
    {
        var result = await _client.PutAsJsonAsync($"v1/categories/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, "Error update category", (int)result.StatusCode);
    }

    public async Task<Response<Category?>> DeleteCategoryAsync(DeleteCategoryRequest request)
    {
        var result = await _client.DeleteAsync($"v1/categories/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, "Error delete category", (int)result.StatusCode);
    }
    public async Task<Response<Category?>> GetCategoryByIdAsync(GetCategoryByIdRequest request)
        => await _client.GetFromJsonAsync<Response<Category?>>($"v1/categories/{request.Id}")
               ?? new Response<Category?>(null, "Error getting category by Id", 400);

    public async Task<PagedResponse<List<Category>>> GetAllCategoriesAsync(GetAllCategoriesRequest request)
        => await _client.GetFromJsonAsync<PagedResponse<List<Category>>>("v1/categories")
            ?? new PagedResponse<List<Category>>(null, "Error getting all categories", 400);
}