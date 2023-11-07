using Dashboard.Api.Validations.User;
using Dashboard.Core.Models;
using Dashboard.Dtos.Payload.Auth;
using Dashboard.Dtos.Payload.User;
using Dashboard.Services.Container.TokenContainer;
using Dashboard.Services.Container.UserContainer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly UserManager<UserModel> _userManager;
    private readonly TokenService _tokenService;
    private readonly IUserService _userService;

    public UsersController(
        ILogger<UsersController> logger,
        IUserService userService,
        UserManager<UserModel> userManager,
        TokenService tokenService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _userService = userService ?? throw new ArgumentException(nameof(userService));
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetAllUsers")]
    public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUsers()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            var users = await _userService.ReadAllUsers();

            _logger.LogInformation("executing GetAllUsers()");

            return Ok(users);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetUserById/{id}")]
    public async Task<ActionResult<UserModel>> GetUserById(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Invalid user id.");
            }

            var user = await _userService.ReadUserById(id);

            if (user == null)
            {
                return NotFound("User does not exist.");
            }

            if (!user.IsActive)
            {
                return NotFound("User is inactive.");
            }

            return Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateUserDetails")]
    public async Task<ActionResult> UpdateUserDetails([FromForm] UserUpdateDetailsRequestDto request)
    {
        try
        {
            var userUpdateDetailsDTOValidation = new UserUpdateValidator();
            var validationResult = userUpdateDetailsDTOValidation.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.First());
            }

            var user = await _userService.ReadUserById(request.Id);

            if (!user.IsActive)
            {
                return NotFound("User is inactive.");
            }

            if (user == null)
            {
                return NotFound("User does not exist.");
            }

            var updatedUser = _userService.UpdateUser(user, request);

            return Ok(updatedUser);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register([FromForm] UserRegistrationRequestDto request)
    {
        try
        {
            var userRegistrationDTOValidation = new UserRegistrationValidator();
            var validationResult = userRegistrationDTOValidation.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.First());
            }

            var response = await _userService.RegisterUser(request);

            if (response.Succeeded)
            {
                return CreatedAtAction(nameof(Register), request);
            }

            response
            .Errors
            .Select(x => new
            {
                Code = x.Code,
                Description = x.Description
            })
            .ToList()
            .ForEach(x => ModelState.AddModelError(x.Code, x.Description));

            return BadRequest(ModelState);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<AuthResponseDto>> Authenticate([FromForm] AuthRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userManager = await _userManager.FindByEmailAsync(request.Email);

            if (userManager.Equals(null))
            {
                return BadRequest("Bad Credentials");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(userManager, request.Password);

            if (!isPasswordValid)
            {
                return BadRequest("Bad Credentials");
            }

            var userAndAccessToken = _tokenService.CreateToken(request);
            var userToken = userAndAccessToken?.Select(x => x.Key).First();

            if (string.IsNullOrEmpty(userToken))
            {
                return Unauthorized();
            }

            var user = userAndAccessToken?.Select(x => x.Value).First();

            return Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteUser/{id}")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Invalid user id.");
            }

            var user = await _userService.ReadUserById(id);

            if (!user.IsActive)
            {
                return NotFound("User is inactive.");
            }

            if (user == null)
            {
                return NotFound("User does not exist.");
            }

            await _userService.DeleteUser(user);

            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
