using KegiFin.Core.Models;
using KegiFin.Core.Requests.Categories;

namespace KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Helpers;

public static class CategoryRequestFactory
{
    private const string DefaultUserId = "123";

    #region Create

    public static CreateCategoryRequest GetValidCreateRequest(string name = "Category Mock", string description = "Test Mock")
        => new() { Name = name, Description = description };

    #endregion

    #region Update

    public static UpdateCategoryRequest GetValidUpdateRequest(int id = 1, string userId = DefaultUserId)
        => new()
        {
            Id = id,
            UserId = userId,
            Name = $"Category Mock Update {id}",
            Description = $"Description for category {id}"
        };

    #endregion

    #region Delete

    public static DeleteCategoryRequest GetValidDeleteRequest(int id = 1, string userId = DefaultUserId)
        => new() { Id = id, UserId = userId };

    #endregion

    #region Get

    public static GetCategoryByIdRequest GetValidCategoryByIdRequest(int id = 1, string userId = DefaultUserId)
        => new() { Id = id, UserId = userId };

    public static GetAllCategoriesRequest GetValidAllCategoriesRequest(string userId = DefaultUserId, int pageNumber = 1, int pageSize = 10)
        => new() { UserId = userId, PageNumber = pageNumber, PageSize = pageSize };

    #endregion

    #region Categories

    public static List<Category> GetSampleCategories(string userId = DefaultUserId) 
        =>
        [
            new() { Id = 1, UserId = userId, Name = "Category One" },
            new() { Id = 2, UserId = userId, Name = "Category Two" },
            new() { Id = 3, UserId = userId, Name = "Category Three" }
        ];

    public static List<Category> GetEmptyCategoryList() => [];

    public static Category GetCategory(int id = 1, string userId = DefaultUserId)
        => new() { Id = id, UserId = userId, Name = $"Category {id}", Description = $"Description {id}" };

    #endregion
}