
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Necesario para generar documentación Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var assembly = typeof(Program).Assembly;

// Add FluentValidation and register validators from the current assembly
builder.Services.AddValidatorsFromAssembly(assembly);

// Add services to the container
builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
});

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();


// Configure logging to filter out less severe logs from specific sources
builder.Logging.AddFilter("Marten", LogLevel.Warning);
builder.Logging.AddFilter("Npgsql", LogLevel.Warning);


builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
app.MapCarter();

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
