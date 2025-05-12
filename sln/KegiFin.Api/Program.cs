using KegiFin.Api.Data;
using KegiFin.Api.Endpoints;
using KegiFin.Api.Handlers;
using KegiFin.Core.Handlers;
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

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapGet("/", () => new {message = "Ok"});
    app.MapEndpoints();

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