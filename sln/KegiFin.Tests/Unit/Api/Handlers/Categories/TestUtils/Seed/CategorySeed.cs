using KegiFin.Core.Models;

namespace KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Seed;

public static class CategorySeed
{
    public static List<Category> GetCategories() 
        =>
        [
            new Category { Id = 1, UserId = "123", Name = "Category One" },
            new Category { Id = 2, UserId = "123", Name = "Category Two" },
            new Category { Id = 3, UserId = "123", Name = "Category Three" }
        ];

    public static List<Category> GetCategoriesEmpty() => [];
}