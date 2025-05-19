using KegiFin.Api.Data;
using KegiFin.Api.Endpoints;
using KegiFin.Api.Handlers;
using KegiFin.Api.Models;
using KegiFin.Core.Handlers;
using Microsoft.AspNetCore.Identity;
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
    builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
    
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
        options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    })
        .AddCookie(IdentityConstants.ApplicationScheme);
    builder.Services.AddAuthorization();
    
    builder.Services
        .AddIdentityCore<User>()
        .AddRoles<IdentityRole<long>>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddApiEndpoints();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapGet("/", () => new {message = "Ok"});
    app.MapEndpoints();
    
    app.MapGroup("v1/identity")
        .WithTags("Identity")
        .MapIdentityApi<User>();

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