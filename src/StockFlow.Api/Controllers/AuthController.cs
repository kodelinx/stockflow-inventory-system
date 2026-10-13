using Microsoft.AspNetCore.Mvc;
using StockFlow.Api.Models.Auth;
using StockFlow.Api.Services;

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
}