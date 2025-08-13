using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexas.Inventory.Application.Auth.Interface;
using Nexas.Inventory.Application.Auth.ViewModel;
using Nexas.Inventory.Application.Base.Entity;
using Nexas.Inventory.Application.User.Interface;
using Nexas.Inventory.Application.User.ViewModel;
using System.Threading.Tasks;

namespace Nexas.Inventory.API.User.Controllers;
/// <summary>
/// API Controller responsible for managing user-related operations such as
/// retrieval, creation, modification, deletion, and authentication.
/// </summary>
/// <remarks>
/// All endpoints returning user data wrap the result inside a standardized 
/// <see cref="Result{T}"/> object to ensure consistent API responses.
/// </remarks>
[Route("api/[controller]")]
[ApiController]

/// <summary>
/// Initializes a new instance of the <see cref="UserController"/> class.
/// </summary>
/// <param name="userService">Service responsible for handling user business logic.</param>
/// <param name="tokenService">Service responsible for generating authentication tokens.</param>
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public UserController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Retrieves an empty list of users (primarily for testing and example purposes).
    /// </summary>
    /// <returns>An empty collection wrapped in a standardized <see cref="Result{T}"/> object.</returns>
    /// <response code="200">Request processed successfully.</response>
    [HttpGet]
    [ProducesResponseType(typeof(Result<List<UserViewModel>>), StatusCodes.Status200OK)]
    public IActionResult Index()
    {
        var model = new List<UserViewModel>();
        return Ok(Result<List<UserViewModel>>.Ok(model,"Success Return"));
    }

    /// <summary>
    /// Retrieves all registered users in the system.
    /// </summary>
    /// <remarks>
    /// Requires authentication via Bearer token.
    /// </remarks>
    /// <returns>List of users wrapped in a standardized <see cref="Result{T}"/> object.</returns>
    /// <response code="200">List of users successfully retrieved.</response>
    /// <response code="401">Unauthorized - Invalid or missing token.</response>
    [Authorize]
    [HttpGet("IndexGetAll")]
    [ProducesResponseType(typeof(Result<IEnumerable<UserViewModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> IndexAll()
    {
        var model = await _userService.GetAllAsync();
        return Ok(Result<IEnumerable<UserViewModel>>.Ok(model, "Success Return"));
    }

    /// <summary>
    /// Retrieves a specific user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique integer identifier of the user.</param>
    /// <returns>User details wrapped in a standardized <see cref="Result{T}"/> object.</returns>
    /// <response code="200">User successfully retrieved.</response>
    /// <response code="400">Invalid user ID provided.</response>
    /// <response code="401">Unauthorized - Invalid or missing token.</response>
    /// <response code="404">User not found.</response>
    [Authorize]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Result<UserViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<AuthResponseViewModel>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<UserViewModel>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest(Result<AuthResponseViewModel>.Unauthorized("Invalid User ID"));

        var model = await _userService.GetByIdAsync(id);
        if (model == null)
            return NotFound(Result<UserViewModel>.NotFound("User not found"));

        return Ok(Result<UserViewModel>.Ok(model, "Success Return"));
    }

    /// <summary>
    /// Authenticates a user and generates a JWT token for future requests.
    /// </summary>
    /// <param name="request">The login credentials (email and password).</param>
    /// <returns>Authentication token and user details.</returns>
    /// <remarks>
    /// This endpoint verifies the provided email and password combination.
    /// If valid, a JWT token is generated, which must be used for all
    /// subsequent authorized requests.
    /// </remarks>
    /// <response code="200">User successfully authenticated.</response>
    /// <response code="401">Invalid email or password.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(Result<AuthResponseViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<AuthResponseViewModel>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestViewModel request)
    {
        var isValid = await _userService.ValidateUserPassword(request.Email, request.Password);
        if (!isValid)
            return StatusCode(401, Result<AuthResponseViewModel>.Unauthorized("Invalid email or password"));

        var user = await _userService.GetByEmailAsync(request.Email);
        var userForToken = new UserViewModel(user.Id, user.Name, user.Email, null);

        var token = await _tokenService.GenerateToken(userForToken);

        var authResponse = new AuthResponseViewModel
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(60),
            User = new AuthUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            }
        };

        return Ok(Result<AuthResponseViewModel>.Ok(authResponse, "Login Realized With Success."));
    }


    /// <summary>
    /// Creates a new user in the system.
    /// </summary>
    /// <param name="model">User details.</param>
    /// <returns>Details of the newly created user.</returns>
    /// <response code="200">User successfully created.</response>
    /// <response code="401">Unauthorized - Invalid or missing token.</response>
    /// <response code="422">Validation errors occurred.</response>
    [Authorize]
    [HttpPost("create")]
    [ProducesResponseType(typeof(Result<UserViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] UserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return StatusCode(422, Result<UserViewModel>.ValidationError(errors, "Something went wrong."));
        }
        await _userService.AddAsync(model);
        return Ok(Result<UserViewModel>.Ok(model, "User created With Success."));

    }


    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="model">Updated user details.</param>
    /// <returns>Updated user information.</returns>
    /// <response code="200">User successfully updated.</response>
    /// <response code="401">Unauthorized - Invalid or missing token.</response>
    /// <response code="422">Validation errors occurred.</response>
    [Authorize]
    [HttpPost("edit")]
    [ProducesResponseType(typeof(Result<UserViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Edit([FromBody] UserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return StatusCode(422, Result<UserViewModel>.ValidationError(errors, "Something went wrong."));
        }
        await _userService.UpdateAsync(model);
        return Ok(Result<UserViewModel>.Ok(model, "User edited With Success"));
    }

    /// <summary>
    /// Deletes an existing user by ID.
    /// </summary>
    /// <param name="id">Unique identifier of the user to delete.</param>
    /// <returns>Details of the deleted user.</returns>
    /// <response code="200">User successfully deleted.</response>
    /// <response code="400">Invalid user ID provided.</response>
    /// <response code="401">Unauthorized - Invalid or missing token.</response>
    /// <response code="404">User not found.</response>
    [Authorize]
    [HttpDelete("delete{id:int}")]
    [ProducesResponseType(typeof(Result<UserViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<AuthResponseViewModel>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<UserViewModel>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest(Result<AuthResponseViewModel>.Unauthorized("Invalid User ID"));

        var model = await _userService.GetByIdAsync(id);
        if (model == null)
            return NotFound(Result<UserViewModel>.NotFound("User not found"));

        await _userService.DeleteAsync(model);
        return Ok(Result<UserViewModel>.Ok(model, "Success Return"));
    }
}

