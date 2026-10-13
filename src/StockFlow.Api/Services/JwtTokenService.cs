using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StockFlow.Models;

namespace StockFlow.Api.Services;

public class JwtTokenService
{
    // Provides access to appsettings, User Secrets, and other configuration sources.
    private readonly IConfiguration _configuration;

    public JwtTokenService(
        IConfiguration configuration
    )
    {
        _configuration = configuration;
    }

    public (
        string Token,
        DateTime ExpiresAt
    ) CreateToken(User user)
    {
        // Gets the secret signing key from configuration.
        string? jwtKey = _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException(
                "JWT signing key is not configured."
            );
        }

        string issuer = _configuration["Jwt:Issuer"] ?? "StockFlow.Api";

        // Identifies the intended client that will use the token.
        string audience = _configuration["Jwt:Audience"] ?? "StockFlow.Client";
        
        // Reads token lifetime from configuration or defaults to 60 minutes.
        int expirationMinutes =
            int.TryParse(
                _configuration[ "Jwt:ExpirationMinutes" ],
                out int configuredMinutes
            ) ? configuredMinutes : 60;

        DateTime expiresAt =
            DateTime.UtcNow.AddMinutes(
                expirationMinutes
            );

        // Claims store identity information inside the JWT.
        List<Claim> claims =
            new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()
                ),

                new Claim(
                    ClaimTypes.Name,
                    user.Username
                ),

                // Stores the user's role for future authorization checks.
                new Claim(
                    ClaimTypes.Role,
                    user.Role
                ),

                new Claim(
                    "fullName",
                    user.FullName
                ),

                // Gives each JWT a unique token ID.
                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()
                )
            };

        // Converts the text signing key into the key object used by JWT.
        SymmetricSecurityKey securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            );

        // Defines how the JWT will be digitally signed.
        SigningCredentials credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

        // Builds the JWT with issuer, audience, claims, expiration, and signature.
        JwtSecurityToken token =
            new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

        // Converts the JWT object into the token string returned to the client.
        string tokenValue =
            new JwtSecurityTokenHandler()
                .WriteToken(token);

        return (
            tokenValue,
            expiresAt
        );
    }
}