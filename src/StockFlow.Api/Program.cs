using StockFlow.Database;
using StockFlow.Repositories;
using StockFlow.Services;
using System.Text;
// Provides JWT bearer authentication support for ASP.NET Core.
using Microsoft.AspNetCore.Authentication.JwtBearer;
// Provides token validation and signing-key classes.
using Microsoft.IdentityModel.Tokens;
using StockFlow.Api.Services;
using StockFlow.Api.Security;
using StockFlow.Security;

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

// Gets the private JWT signing key from configuration/User Secrets.
string jwtKey =
    builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "Jwt:Key is not configured."
    );

// Identifies StockFlow.Api as the system issuing tokens.
string jwtIssuer =
    builder.Configuration["Jwt:Issuer"]
    ?? "StockFlow.Api";

// Identifies which client the JWT is intended for.
string jwtAudience =
    builder.Configuration["Jwt:Audience"]
    ?? "StockFlow.Client";

// Registers JWT Bearer as the API authentication method.
builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme
    )
    .AddJwtBearer(options =>
    {
        // Defines the rules used when validating incoming JWT tokens.
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                // Confirms the token was issued by StockFlow.Api.
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                // Confirms the token is intended for the StockFlow client.
                ValidateAudience = true,
                ValidAudience = jwtAudience,

                // Rejects expired tokens.
                ValidateLifetime = true,

                // Confirms the token signature matches the private signing key.
                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtKey
                        )
                    ),

                // Removes the default extra expiration-time allowance.
                ClockSkew = TimeSpan.Zero
            };
    });

// Defines reusable role-based authorization rules.
builder.Services.AddAuthorization(options =>
{
    // Only users with the Admin role satisfy this policy.
    options.AddPolicy(
        AuthorizationPolicies.AdminOnly,
        policy =>
        {
            policy.RequireAuthenticatedUser();

            policy.RequireRole(
                UserRoles.Admin
            );
        }
    );

    // Both Staff and Admin users satisfy this policy.
    options.AddPolicy(
        AuthorizationPolicies.StaffOrAdmin,
        policy =>
        {
            policy.RequireAuthenticatedUser();

            policy.RequireRole(
                UserRoles.Admin,
                UserRoles.Staff
            );
        }
    );
});

var app = builder.Build();

DatabaseConnectionService databaseConnectionService =
    app.Services.GetRequiredService<DatabaseConnectionService>();

databaseConnectionService.InitializeDatabase();

// Seeds the initial development admin only while running in Development.
if (app.Environment.IsDevelopment())
{
    // Creates a temporary DI scope because UserRepository and related
    // authentication services are registered as scoped services.
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