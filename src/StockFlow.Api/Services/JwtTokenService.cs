using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StockFlow.Models;

namespace StockFlow.Api.Services;

public class JwtTokenService
{
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
        string? jwtKey = _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException(
                "JWT signing key is not configured."
            );
        }

        string issuer = _configuration["Jwt:Issuer"] ?? "StockFlow.Api";

        string audience = _configuration["Jwt:Audience"] ?? "StockFlow.Client";

        int expirationMinutes =
            int.TryParse(
                _configuration[ "Jwt:ExpirationMinutes" ],
                out int configuredMinutes
            ) ? configuredMinutes : 60;

        DateTime expiresAt =
            DateTime.UtcNow.AddMinutes(
                expirationMinutes
            );

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

                new Claim(
                    ClaimTypes.Role,
                    user.Role
                ),

                new Claim(
                    "fullName",
                    user.FullName
                ),

                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()
                )
            };

        SymmetricSecurityKey securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            );

        SigningCredentials credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

        JwtSecurityToken token =
            new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

        string tokenValue =
            new JwtSecurityTokenHandler()
                .WriteToken(token);

        return (
            tokenValue,
            expiresAt
        );
    }
}