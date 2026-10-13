using StockFlow.Database;
using StockFlow.Repositories;
using StockFlow.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StockFlow.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<DatabaseConnectionService>();

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<OrderItemRepository>();
builder.Services.AddScoped<ReceiptRepository>();
builder.Services.AddScoped<StockMovementRepository>();
builder.Services.AddScoped<NotificationRepository>();
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<DevelopmentUserSeeder>();

builder.Services.AddScoped<ProductManager>();

string jwtKey =
    builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "Jwt:Key is not configured."
    );

string jwtIssuer =
    builder.Configuration["Jwt:Issuer"]
    ?? "StockFlow.Api";

string jwtAudience =
    builder.Configuration["Jwt:Audience"]
    ?? "StockFlow.Client";

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme
    )
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtKey
                        )
                    ),

                ClockSkew = TimeSpan.Zero
            };
    });

var app = builder.Build();

DatabaseConnectionService databaseConnectionService =
    app.Services.GetRequiredService<DatabaseConnectionService>();

databaseConnectionService.InitializeDatabase();

if (app.Environment.IsDevelopment())
{
    using IServiceScope scope =
        app.Services.CreateScope();

    DevelopmentUserSeeder seeder =
        scope.ServiceProvider
            .GetRequiredService<
                DevelopmentUserSeeder
            >();

    seeder.SeedAdmin();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();