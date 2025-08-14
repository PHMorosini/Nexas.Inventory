using FluentAssertions;
using Nexas.Inventory.Domain.User.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexasUnityTests.User.Controller;
public class UserEntityTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateUserEntity()
    {
        // Arrange
        var id = 1;
        var name = "John Doe";
        var email = "john@example.com";
        var password = "password123";

        // Act
        var user = new UserEntity(id, name, email, password);

        // Assert
        user.Id.Should().Be(id);
        user.Name.Should().Be(name.ToUpper());
        user.Email.Should().Be(email);
        user.PasswordHash.Should().NotBeNullOrEmpty();
        BCrypt.Net.BCrypt.Verify(password, user.PasswordHash).Should().BeTrue();
    }

    [Fact]
    public void Constructor_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var id = 1;
        string name = null;
        var email = "john@example.com";
        var password = "password123";

        // Act & Assert
        Action act = () => new UserEntity(id, name, email, password);
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName(nameof(name));
    }

    [Fact]
    public void Constructor_WithEmptyPassword_ShouldNotSetPasswordHash()
    {
        // Arrange
        var id = 1;
        var name = "John Doe";
        var email = "john@example.com";
        var password = "";

        // Act
        var user = new UserEntity(id, name, email, password);

        // Assert
        user.PasswordHash.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithWhitespacePassword_ShouldNotSetPasswordHash()
    {
        // Arrange
        var id = 1;
        var name = "John Doe";
        var email = "john@example.com";
        var password = "   ";

        // Act
        var user = new UserEntity(id, name, email, password);

        // Assert
        user.PasswordHash.Should().BeNull();
    }

    [Fact]
    public void DefaultConstructor_ShouldCreateEmptyUserEntity()
    {
        // Act
        var user = new UserEntity();

        // Assert
        user.Id.Should().Be(0);
        user.Name.Should().BeNull();
        user.Email.Should().BeNull();
        user.PasswordHash.Should().BeNull();
    }

    #endregion

    #region SetEmail Tests

    [Fact]
    public void SetEmail_WithValidEmail_ShouldSetEmail()
    {
        // Arrange
        var user = new UserEntity();
        var email = "john@example.com";

        // Act
        user.SetEmail(email);

        // Assert
        user.Email.Should().Be(email);
    }

    [Fact]
    public void SetEmail_WithEmailWithSpaces_ShouldTrimEmail()
    {
        // Arrange
        var user = new UserEntity();
        var email = "  john@example.com  ";

        // Act
        user.SetEmail(email);

        // Assert
        user.Email.Should().Be("john@example.com");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SetEmail_WithInvalidEmail_ShouldThrowArgumentException(string invalidEmail)
    {
        // Arrange
        var user = new UserEntity();

        // Act & Assert
        Action act = () => user.SetEmail(invalidEmail);
        act.Should().Throw<ArgumentException>()
            .WithParameterName("email")
            .WithMessage("Email cannot be null or empty. (Parameter 'email')");
    }

    #endregion

    #region SetPassword Tests

    [Fact]
    public void SetPassword_WithValidPassword_ShouldHashAndSetPassword()
    {
        // Arrange
        var user = new UserEntity();
        var password = "password123";

        // Act
        user.SetPassword(password);

        // Assert
        user.PasswordHash.Should().NotBeNullOrEmpty();
        BCrypt.Net.BCrypt.Verify(password, user.PasswordHash).Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SetPassword_WithInvalidPassword_ShouldThrowArgumentException(string invalidPassword)
    {
        // Arrange
        var user = new UserEntity();

        // Act & Assert
        Action act = () => user.SetPassword(invalidPassword);
        act.Should().Throw<ArgumentException>()
            .WithParameterName("password")
            .WithMessage("Password cannot be null or empty. (Parameter 'password')");
    }

    #endregion

    #region VerifyPassword Tests

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "password123";
        var user = new UserEntity(1, "John", "john@example.com", password);

        // Act
        var result = user.VerifyPassword(password);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "password123";
        var wrongPassword = "wrongpassword";
        var user = new UserEntity(1, "John", "john@example.com", password);

        // Act
        var result = user.VerifyPassword(wrongPassword);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void VerifyPassword_WithInvalidPassword_ShouldReturnFalse(string invalidPassword)
    {
        // Arrange
        var user = new UserEntity(1, "John", "john@example.com", "password123");

        // Act
        var result = user.VerifyPassword(invalidPassword);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region ChangePassword Tests

    [Fact]
    public void ChangePassword_WithValidNewPassword_ShouldUpdatePassword()
    {
        // Arrange
        var originalPassword = "oldpassword";
        var newPassword = "newpassword123";
        var user = new UserEntity(1, "John", "john@example.com", originalPassword);
        var originalHash = user.PasswordHash;

        // Act
        user.ChangePassword(newPassword);

        // Assert
        user.PasswordHash.Should().NotBe(originalHash);
        user.VerifyPassword(newPassword).Should().BeTrue();
        user.VerifyPassword(originalPassword).Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ChangePassword_WithInvalidPassword_ShouldThrowArgumentException(string invalidPassword)
    {
        // Arrange
        var user = new UserEntity(1, "John", "john@example.com", "password123");

        // Act & Assert
        Action act = () => user.ChangePassword(invalidPassword);
        act.Should().Throw<ArgumentException>()
            .WithParameterName("password");
    }

    #endregion

    #region Edit Tests

    [Fact]
    public void Edit_WithValidUserEntity_ShouldUpdateProperties()
    {
        // Arrange
        var originalUser = new UserEntity(1, "John", "john@example.com", "password123");
        var updatedUser = new UserEntity(2, "Jane Doe", "jane@example.com","TESTE");

        // Act
        originalUser.Edit(updatedUser);

        // Assert
        originalUser.Id.Should().Be(2);
        originalUser.Name.Should().Be("JANE DOE");
        originalUser.Email.Should().Be("jane@example.com");
    }

    [Fact]
    public void Edit_WithUserEntityWithNullName_ShouldKeepOriginalName()
    {
        // Arrange
        var originalUser = new UserEntity(1, "John", "john@example.com", "password123");
        var updatedUser = new UserEntity { Id = 2};
        updatedUser.SetEmail("jane@example.com");

        // Act
        originalUser.Edit(updatedUser);

        // Assert
        originalUser.Id.Should().Be(2);
        originalUser.Name.Should().Be("JOHN"); // Should keep original name
        originalUser.Email.Should().Be("jane@example.com");
    }

    [Fact]
    public void Edit_WithNullUser_ShouldThrowArgumentNullException()
    {
        // Arrange
        var user = new UserEntity(1, "John", "john@example.com", "password123");

        // Act & Assert
        Action act = () => user.Edit(null);
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("user");
    }

    #endregion

    #region Name Capitalization Tests

    [Theory]
    [InlineData("john doe", "JOHN DOE")]
    [InlineData("JOHN DOE", "JOHN DOE")]
    [InlineData("John Doe", "JOHN DOE")]
    [InlineData("jOhN dOe", "JOHN DOE")]
    public void Constructor_ShouldCapitalizeName(string inputName, string expectedName)
    {
        // Act
        var user = new UserEntity(1, inputName, "john@example.com", "TESTE");

        // Assert
        user.Name.Should().Be(expectedName);
    }

    #endregion
}
