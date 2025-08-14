using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexas.Inventory.API.User.Controllers;
using Nexas.Inventory.Application.Auth.Interface;
using Nexas.Inventory.Application.Auth.ViewModel;
using Nexas.Inventory.Application.Base.Entity;
using Nexas.Inventory.Application.User.Interface;
using Nexas.Inventory.Application.User.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexasUnityTests.User.Controller;
public class UserControllerTests
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _userService = A.Fake<IUserService>();
        _tokenService = A.Fake<ITokenService>();
        _controller = new UserController(_userService, _tokenService);
    }

    #region Index Tests

    [Fact]
    public void Index_ShouldReturnEmptyListWithSuccessResult()
    {
        // Act
        var result = _controller.Index();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resultValue = okResult.Value.Should().BeAssignableTo<Result<List<UserViewModel>>>().Subject;
        resultValue.Data.Should().NotBeNull().And.BeEmpty();
        resultValue.Message.Should().Be("Success Return");
        resultValue.Success.Should().BeTrue();
    }

    #endregion

    #region IndexAll Tests

    [Fact]
    public async Task IndexAll_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<UserViewModel>
            {
                new UserViewModel(1, "John Doe", "john@example.com", null),
                new UserViewModel(2, "Jane Doe", "jane@example.com", null)
            };
        A.CallTo(() => _userService.GetAllAsync()).Returns(users);

        // Act
        var result = await _controller.IndexAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resultValue = okResult.Value.Should().BeAssignableTo<Result<IEnumerable<UserViewModel>>>().Subject;
        resultValue.Data.Should().HaveCount(2);
        resultValue.Message.Should().Be("Success Return");
        resultValue.Success.Should().BeTrue();

        A.CallTo(() => _userService.GetAllAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task IndexAll_WithEmptyList_ShouldReturnEmptyResult()
    {
        // Arrange
        var users = new List<UserViewModel>();
        A.CallTo(() => _userService.GetAllAsync()).Returns(users);

        // Act
        var result = await _controller.IndexAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var resultValue = okResult.Value.Should().BeAssignableTo<Result<IEnumerable<UserViewModel>>>().Subject;
        resultValue.Data.Should().BeEmpty();
    }

    #endregion

    #region GetById Tests

    [Fact]
    public async Task GetById_WithValidId_ShouldReturnUser()
    {
        // Arrange
        var userId = 1;
        var user = new UserViewModel(userId, "John Doe", "john@example.com", null);
        A.CallTo(() => _userService.GetByIdAsync(userId)).Returns(user);

        // Act
        var result = await _controller.GetById(userId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var resultValue = okResult.Value.Should().BeAssignableTo<Result<UserViewModel>>().Subject;
        resultValue.Data.Should().Be(user);
        resultValue.Message.Should().Be("Success Return");
        resultValue.Success.Should().BeTrue();

        A.CallTo(() => _userService.GetByIdAsync(userId)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task GetById_WithInvalidId_ShouldReturnBadRequest(int invalidId)
    {
        // Act
        var result = await _controller.GetById(invalidId);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        var resultValue = badRequestResult.Value.Should().BeAssignableTo<Result<AuthResponseViewModel>>().Subject;
        resultValue.Message.Should().Be("Invalid User ID");
        resultValue.Success.Should().BeFalse();

        A.CallTo(() => _userService.GetByIdAsync(A<int>._)).MustNotHaveHappened();
    }

    [Fact]
    public async Task GetById_WithNonExistentUser_ShouldReturnNotFound()
    {
        // Arrange
        var userId = 999;
        A.CallTo(() => _userService.GetByIdAsync(userId)).Returns((UserViewModel)null);

        // Act
        var result = await _controller.GetById(userId);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var resultValue = notFoundResult.Value.Should().BeAssignableTo<Result<UserViewModel>>().Subject;
        resultValue.Message.Should().Be("User not found");
        resultValue.Success.Should().BeFalse();
    }

    #endregion

    #region Login Tests

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnTokenAndUser()
    {
        // Arrange
        var loginRequest = new LoginRequestViewModel { Email = "john@example.com", Password = "password123" };
        var user = new UserViewModel(1, "John Doe", "john@example.com", null);
        var token = "jwt-token-here";

        A.CallTo(() => _userService.ValidateUserPassword(loginRequest.Email, loginRequest.Password)).Returns(true);
        A.CallTo(() => _userService.GetByEmailAsync(loginRequest.Email)).Returns(user);
        A.CallTo(() => _tokenService.GenerateToken(A<UserViewModel>._)).Returns(token);

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var resultValue = okResult.Value.Should().BeAssignableTo<Result<AuthResponseViewModel>>().Subject;

        resultValue.Success.Should().BeTrue();
        resultValue.Message.Should().Be("Login Realized With Success.");
        resultValue.Data.Token.Should().Be(token);
        resultValue.Data.User.Id.Should().Be(user.Id);
        resultValue.Data.User.Name.Should().Be(user.Name);
        resultValue.Data.User.Email.Should().Be(user.Email);

        A.CallTo(() => _userService.ValidateUserPassword(loginRequest.Email, loginRequest.Password)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _userService.GetByEmailAsync(loginRequest.Email)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _tokenService.GenerateToken(A<UserViewModel>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequestViewModel { Email = "john@example.com", Password = "wrongpassword" };
        A.CallTo(() => _userService.ValidateUserPassword(loginRequest.Email, loginRequest.Password)).Returns(false);

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        var unauthorizedResult = result.Should().BeOfType<ObjectResult>().Subject;
        unauthorizedResult.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

        var resultValue = unauthorizedResult.Value.Should().BeAssignableTo<Result<AuthResponseViewModel>>().Subject;
        resultValue.Success.Should().BeFalse();
        resultValue.Message.Should().Be("Invalid email or password");

        A.CallTo(() => _userService.GetByEmailAsync(A<string>._)).MustNotHaveHappened();
        A.CallTo(() => _tokenService.GenerateToken(A<UserViewModel>._)).MustNotHaveHappened();
    }

    #endregion

    #region Create Tests

    [Fact]
    public async Task Create_WithValidModel_ShouldCreateUser()
    {
        // Arrange
        var userModel = new UserViewModel(0, "John Doe", "john@example.com", "password123");
        _controller.ModelState.Clear(); // Ensure ModelState is valid

        // Act
        var result = await _controller.Create(userModel);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var resultValue = okResult.Value.Should().BeAssignableTo<Result<UserViewModel>>().Subject;

        resultValue.Success.Should().BeTrue();
        resultValue.Message.Should().Be("User created With Success.");
        resultValue.Data.Should().Be(userModel);

        A.CallTo(() => _userService.AddAsync(userModel)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Create_WithInvalidModelState_ShouldReturnValidationError()
    {
        // Arrange
        var userModel = new UserViewModel(0, "", "invalid-email", "password123");
        _controller.ModelState.AddModelError("Name", "Name is required");
        _controller.ModelState.AddModelError("Email", "Invalid email format");

        // Act
        var result = await _controller.Create(userModel);

        // Assert
        var unprocessableResult = result.Should().BeOfType<ObjectResult>().Subject;
        unprocessableResult.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);

        var resultValue = unprocessableResult.Value.Should().BeAssignableTo<Result<UserViewModel>>().Subject;
        resultValue.Success.Should().BeFalse();
        resultValue.Message.Should().Be("Something went wrong.");
        resultValue.ValidationErrors.Should().ContainKey("Name");
        resultValue.ValidationErrors.Should().ContainKey("Email");

        A.CallTo(() => _userService.AddAsync(A<UserViewModel>._)).MustNotHaveHappened();
    }

    #endregion

    #region Edit Tests

    [Fact]
    public async Task Edit_WithValidModel_ShouldUpdateUser()
    {
        // Arrange
        var userModel = new UserViewModel(1, "John Doe Updated", "john.updated@example.com", null);
        _controller.ModelState.Clear(); // Ensure ModelState is valid

        // Act
        var result = await _controller.Edit(userModel);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var resultValue = okResult.Value.Should().BeAssignableTo<Result<UserViewModel>>().Subject;

        resultValue.Success.Should().BeTrue();
        resultValue.Message.Should().Be("User edited With Success");
        resultValue.Data.Should().Be(userModel);

        A.CallTo(() => _userService.UpdateAsync(userModel)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Edit_WithInvalidModelState_ShouldReturnValidationError()
    {
        // Arrange
        var userModel = new UserViewModel(1, "", "invalid-email", null);
        _controller.ModelState.AddModelError("Name", "Name is required");
        _controller.ModelState.AddModelError("Email", "Invalid email format");

        // Act
        var result = await _controller.Edit(userModel);

        // Assert
        var unprocessableResult = result.Should().BeOfType<ObjectResult>().Subject;
        unprocessableResult.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);

        var resultValue = unprocessableResult.Value.Should().BeAssignableTo<Result<UserViewModel>>().Subject;
        resultValue.Success.Should().BeFalse();
        resultValue.Message.Should().Be("Something went wrong.");
        resultValue.ValidationErrors.Should().ContainKey("Name");
        resultValue.ValidationErrors.Should().ContainKey("Email");

        A.CallTo(() => _userService.UpdateAsync(A<UserViewModel>._)).MustNotHaveHappened();
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_WithValidId_ShouldDeleteUser()
    {
        // Arrange
        var userId = 1;
        var user = new UserViewModel(userId, "John Doe", "john@example.com", null);
        A.CallTo(() => _userService.GetByIdAsync(userId)).Returns(user);

        // Act
        var result = await _controller.Delete(userId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var resultValue = okResult.Value.Should().BeAssignableTo<Result<UserViewModel>>().Subject;

        resultValue.Success.Should().BeTrue();
        resultValue.Message.Should().Be("Success Return");
        resultValue.Data.Should().Be(user);

        A.CallTo(() => _userService.GetByIdAsync(userId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _userService.DeleteAsync(userId)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task Delete_WithInvalidId_ShouldReturnBadRequest(int invalidId)
    {
        // Act
        var result = await _controller.Delete(invalidId);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        var resultValue = badRequestResult.Value.Should().BeAssignableTo<Result<AuthResponseViewModel>>().Subject;
        resultValue.Message.Should().Be("Invalid User ID");
        resultValue.Success.Should().BeFalse();

        A.CallTo(() => _userService.GetByIdAsync(A<int>._)).MustNotHaveHappened();
        A.CallTo(() => _userService.DeleteAsync(A<int>._)).MustNotHaveHappened();
    }

    [Fact]
    public async Task Delete_WithNonExistentUser_ShouldReturnNotFound()
    {
        // Arrange
        var userId = 999;
        A.CallTo(() => _userService.GetByIdAsync(userId)).Returns((UserViewModel)null);

        // Act
        var result = await _controller.Delete(userId);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var resultValue = notFoundResult.Value.Should().BeAssignableTo<Result<UserViewModel>>().Subject;
        resultValue.Message.Should().Be("User not found");
        resultValue.Success.Should().BeFalse();

        A.CallTo(() => _userService.DeleteAsync(A<int>._)).MustNotHaveHappened();
    }

    #endregion
}
