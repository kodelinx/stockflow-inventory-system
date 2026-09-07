using StockFlow.Database;
using StockFlow.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<DatabaseConnectionService>();
builder.Services.AddScoped<ProductRepository>();

var app = builder.Build();

DatabaseConnectionService databaseConnectionService =
    app.Services.GetRequiredService<DatabaseConnectionService>();

databaseConnectionService.InitializeDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();