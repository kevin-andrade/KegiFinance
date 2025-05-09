using KegiFin.Api.Data;
using KegiFin.Api.Handlers;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models;
using KegiFin.Core.Requests.Categories;
using KegiFin.Core.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    // .WriteTo.Seq("http://localhost:5341") // Ative se for usar o Seq
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    Log.Information("Starting...");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    var cnnString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    builder.Services.AddDbContext<AppDbContext>(x => { x.UseSqlServer(cnnString); });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(x => { x.CustomSchemaIds(y => y.FullName); });
    builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapPost(
            "/v1/categories",
            async (CreateCategoryRequest request, ICategoryHandler handler)
                => await handler.CreateCategoryAsync(request))
        .WithName("Categories : Create")
        .WithSummary("Create a category")
        .Produces<Response<Category>>();
    
    app.MapPut(
            "/v1/categories/{id}",
            async (long id, UpdateCategoryRequest request, ICategoryHandler handler)
                =>
            {
                request.Id = id;
                return await handler.UpdateCategoryAsync(request);
            })
        .WithName("Categories : Update")
        .WithSummary("Update a category")
        .Produces<Response<Category>>();
    
    app.MapDelete(
            "/v1/categories/{id}",
            async (long id, ICategoryHandler handler)
                =>
            {
                var request = new DeleteCategoryRequest{Id = id, UserId = "test1"};
                return await handler.DeleteCategoryAsync(request);
            })
        .WithName("Categories : Delete")
        .WithSummary("Delete category")
        .Produces<Response<Category>>();
    
    app.MapGet(
            "/v1/categories/{id}",
            async (long id, ICategoryHandler handler)
                =>
            {
                var request = new GetCategoryByIdRequest{Id = id, UserId = "test1"};
                return await handler.GetCategoryByIdAsync(request);
            })
        .WithName("Categories : Get by Id")
        .WithSummary("Return a category")
        .Produces<Response<Category>>();
    
    app.MapGet(
            "/v1/categories",
            async (ICategoryHandler handler)
                =>
            {
                var request = new GetAllCategoriesRequest{UserId = "test1"};
                return await handler.GetAllCategoriesAsync(request);
            })
        .WithName("Categories : Get all categories")
        .WithSummary("Return all categories for a user")
        .Produces<PagedResponse<List<Category>?>>();

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Critical error starting the system");
}
finally
{
    Log.CloseAndFlush();
}