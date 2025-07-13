using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.UseCases.AuthUseCases;
using BackendSport.Application.Interfaces.AuthInterfaces;

namespace BackendSport.API.Controllers.v1.Users;

/// <summary>
/// Controller for user management operations
/// </summary>
[ApiController]
[Route("api/v1/users")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly CreateUserUseCase _createUserUseCase;

    public UsersController(CreateUserUseCase createUserUseCase)
    {
        _createUserUseCase = createUserUseCase;
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="createUserDto">User data</param>
    /// <returns>Created user</returns>
    /// <response code="201">User created successfully</response>
    /// <response code="400">User with this email already exists</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserResponseDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            var user = await _createUserUseCase.ExecuteAsync(createUserDto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="userRepository">User repository</param>
    /// <returns>User details</returns>
    /// <response code="200">User found</response>
    /// <response code="404">User not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponseDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetUserById(
        string id, 
        [FromServices] IUserRepository userRepository)
    {
        try
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var userResponse = new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                DocumentTypeId = user.DocumentTypeId,
                DocumentNumber = user.DocumentNumber,
                CountryId = user.CountryId,
                DepartmentId = user.DepartmentId,
                MunicipalityId = user.MunicipalityId,
                CityId = user.CityId,
                LocalityId = user.LocalityId,
                CreatedAt = user.CreatedAt
            };

            return Ok(userResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get all users (admin only)
    /// </summary>
    /// <returns>List of users</returns>
    /// <response code="200">Users retrieved successfully</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<UserResponseDto>), 200)]
    public IActionResult GetAllUsers()
    {
        try
        {
            // Note: This endpoint should be implemented when GetAllAsync is available in IUserRepository
            return Ok(new List<UserResponseDto>());
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
