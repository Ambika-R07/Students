using Serilog;
using SMS.WebAPI.Middleware;
using SMS.Services.Implementations;
using SMS.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ------------------------
// Serilog configuration
// ------------------------
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// ------------------------
// Add services to DI
// ------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register StudentService (In-Memory for now)
builder.Services.AddScoped<IStudentService, StudentService>();

var app = builder.Build();

// ------------------------
// Middleware pipeline
// ------------------------

// Global Exception Handling Middleware (Ticket 3)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger (API Docs)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
