using Dashboard.Core.Models;
using Dashboard.Services.Container.TokenContainer;
using Dashboard.Services.Container.UserContainer;
using Dashboard.TestHelpers;
using Dashboard.Api.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Dashboard.Api.UnitTests.Controllers.UserControllerTests;

public class UsersControllerGetById
{
    private readonly Mock<ILogger<UsersController>> _logger;
    private readonly Mock<IUserService> _userService;
    private readonly Mock<TokenService> _tokenService;
    private readonly Mock<UserManager<UserModel>> _userManager;
    private readonly UsersController _userController;

    public UsersControllerGetById()
    {
        _logger = new Mock<ILogger<UsersController>>();
        _tokenService = new Mock<TokenService>();
        _userManager = new Mock<UserManager<UserModel>>(Mock.Of<IUserStore<UserModel>>(), null, null, null, null, null, null, null, null);
        _userService = new Mock<IUserService>();

        _userController = new UsersController(
            _logger.Object,
            _userService.Object,
            _userManager.Object,
            _tokenService.Object
        );
    }

    #region Success
    [Fact]
    public async Task GetUserById_ShouldPass_WhenValidIdIsPassed()
    {
        // arrange
        var id = "9f44cb79-821d-4b42-8550-4ea01569d6b6";

        _userService.Setup(x => x.ReadUserById(id)).ReturnsAsync(UserHelper.GetUser());

        // act
        var result = await _userController.GetUserById(id);

        // assert
        Assert.IsType<OkObjectResult>(result.Result);
    }
    #endregion

    #region Fail
    [Fact]
    public async Task GetUserById_ShouldFail_WhenInvalidIdIsPassed()
    {
        // arrange
        var id = "";

        // act
        var result = await _userController.GetUserById(id);

        // assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUserById_ShouldFail_WhenUserIsInactive()
    {
        // arrange
        var id = "9f44cb79-821d-4b42-8550-4ea01569d6b6";
        var user = UserHelper.GetUser();
        user.IsActive = false;

        _userService.Setup(x => x.ReadUserById(id)).ReturnsAsync(user);

        // act
        var result = await _userController.GetUserById(id);

        // assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUserById_ShouldFail_WhenUserIsNull()
    {
        // arrange
        var id = "9f44cb79-821d-4b42-8550-4ea01569d6b6";

        _userService.Setup(x => x.ReadUserById(id)).ReturnsAsync((UserModel)null);

        // act
        var result = await _userController.GetUserById(id);

        // assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
    #endregion
}