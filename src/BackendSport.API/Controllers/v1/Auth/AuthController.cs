using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.UseCases.AuthUseCases;

namespace BackendSport.API.Controllers.v1.Auth;

/// <summary>
/// Controller for authentication operations
/// </summary>
[ApiController]
[Route("api/v1/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    /// <summary>
    /// Authenticate user and get access token
    /// </summary>
    /// <param name="loginDto">Login credentials</param>
    /// <param name="loginUserUseCase">Login use case</param>
    /// <returns>Authentication response with tokens</returns>
    /// <response code="200">Login successful</response>
    /// <response code="400">Invalid credentials</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto loginDto, 
        [FromServices] LoginUserUseCase loginUserUseCase)
    {
        try
        {
            var result = await loginUserUseCase.ExecuteAsync(loginDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    /// <param name="refreshDto">Refresh token request</param>
    /// <param name="refreshTokenUseCase">Refresh token use case</param>
    /// <returns>New access token</returns>
    /// <response code="200">Token refreshed successfully</response>
    /// <response code="400">Invalid refresh token</response>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(LoginResponseDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenRequestDto refreshDto,
        [FromServices] RefreshTokenUseCase refreshTokenUseCase)
    {
        try
        {
            if (refreshDto == null || string.IsNullOrEmpty(refreshDto.RefreshToken))
            {
                return BadRequest(new { message = "Refresh token is required" });
            }

            var result = await refreshTokenUseCase.ExecuteAsync(refreshDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Logout user and invalidate refresh token
    /// </summary>
    /// <param name="refreshDto">Refresh token to invalidate</param>
    /// <param name="logoutUserUseCase">Logout use case</param>
    /// <returns>Logout confirmation</returns>
    /// <response code="200">Logout successful</response>
    /// <response code="400">Invalid request</response>
    [HttpPost("logout")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Logout(
        [FromBody] RefreshTokenRequestDto refreshDto,
        [FromServices] LogoutUserUseCase logoutUserUseCase)
    {
        try
        {
            if (refreshDto == null || string.IsNullOrEmpty(refreshDto.RefreshToken))
            {
                return BadRequest(new { message = "Refresh token is required" });
            }

            await logoutUserUseCase.ExecuteAsync(refreshDto);
            return Ok(new { message = "Logout successful" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Logout user from all devices
    /// </summary>
    /// <param name="logoutAllUserDevicesUseCase">Logout all devices use case</param>
    /// <returns>Logout confirmation</returns>
    /// <response code="200">Logout from all devices successful</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost("logout-all")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> LogoutAll(
        [FromServices] LogoutAllUserDevicesUseCase logoutAllUserDevicesUseCase)
    {
        try
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest(new { message = "Access token is required in Authorization header" });
            }

            var accessToken = authHeader.Substring("Bearer ".Length).Trim();
            await logoutAllUserDevicesUseCase.ExecuteAsync(accessToken);
            return Ok(new { message = "Logout from all devices successful" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
