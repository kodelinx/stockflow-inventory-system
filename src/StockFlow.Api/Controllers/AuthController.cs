using Microsoft.AspNetCore.Mvc;
using StockFlow.Api.Models.Auth;
using StockFlow.Api.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using StockFlow.Api.Security;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthenticationService
        _authenticationService;

    public AuthController(
        AuthenticationService authenticationService
    )
    {
        _authenticationService =
            authenticationService;
    }

    [HttpPost("login")]
    public IActionResult Login(
        LoginRequest request
    )
    {
        if (
            string.IsNullOrWhiteSpace(
                request.Username
            ) ||
            string.IsNullOrWhiteSpace(
                request.Password
            )
        )
        {
            return BadRequest(
                "Username and password are required."
            );
        }

        // Attempts to authenticate the user and generate a JWT.
        LoginResponse? response =
            _authenticationService.Login(
                request.Username,
                request.Password
            );

        if (response == null)
        {
            return Unauthorized(
                "Invalid username or password."
            );
        }

        // Returns the authenticated identity and JWT.
        return Ok(response);
    }

    // Requires a valid JWT but does not require a specific role.
    // Used to confirm the identity and claims contained in the token.
    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        // Reads the user ID claim created when the JWT was generated.
        string? userId =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

        // Reads the username from the authenticated identity.
        string? username =
            User.FindFirstValue(
                ClaimTypes.Name
            );

        // Reads the user's role claim.
        string? role =
            User.FindFirstValue(
                ClaimTypes.Role
            );

        // Reads the custom fullName claim added to the JWT.
        string? fullName =
            User.FindFirstValue(
                "fullName"
            );

        var response = new
        {
            UserId = userId,
            Username = username,
            FullName = fullName,
            Role = role
        };

        return Ok(response);
    }

    // Requires authentication and the Admin role.
    // This verifies that StockFlow's AdminOnly policy works
    // before the policy is applied to business endpoints in M55.
    [Authorize(
        Policy = AuthorizationPolicies.AdminOnly
    )]
    [HttpGet("admin-check")]
    public IActionResult AdminCheck()
    {
        return Ok(
            new
            {
                Message =
                    "Admin authorization successful."
            }
        );
    }

    // Allows either Staff or Admin users.
    // Demonstrates a policy that accepts more than one role.
    [Authorize(
        Policy = AuthorizationPolicies.StaffOrAdmin
    )]
    [HttpGet("staff-check")]
    public IActionResult StaffCheck()
    {
        return Ok(
            new
            {
                Message =
                    "Staff or Admin authorization successful."
            }
        );
    }
}