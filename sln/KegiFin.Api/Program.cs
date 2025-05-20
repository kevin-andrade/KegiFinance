using KegiFin.Api.Common.Api;
using KegiFin.Api.Endpoints;
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
    builder.LoadConfiguration();
    builder.LoadSecurity();
    builder.LoadDataContexts();
    builder.LoadDocumentation();
    builder.LoadServices();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
        app.ConfigureDevEnvironment();
    
    app.UseSecurity();
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