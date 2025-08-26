using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using SMS.WebApi.Middleware;

try
{
   
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build())
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();

    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

   
    builder.Host.UseSerilog();

    
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "SMS Web API",
            Description = "Demo API with global exception handling"
        });
    });

    var app = builder.Build();

    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "SMS Web API v1");
            c.RoutePrefix = string.Empty; 
        });
    }

    
    app.UseGlobalExceptionHandler();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    Console.WriteLine("Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
    Console.WriteLine("Application shutdown");
}
