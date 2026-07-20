using FluentAssertions;
using KinematicsSimulator.Domain.Entities;
using KinematicsSimulator.Domain.Entities.ValueObjects;

namespace Kinematics.Domain.UnitTests.EntitiesTests;

public class UserTests
{
    [Fact]
    public void User_Create_GoodValues_ShouldReturnUser()
    {
        var emailResult = Email.Create("test@email.com");
        var result = User.Create("ValidUser", emailResult.Value, "passwordHash");

        result.IsSuccess.Should().BeTrue();
        result.Value.UserName.Should().Be("ValidUser");
    }

    [Fact]
    public void User_Create_EmptyUserName_ShouldReturnErrorEmptyUserName()
    {
        var emailResult = Email.Create("test@email.com");
        var result = User.Create(" ", emailResult.Value, "passwordHash");

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "User.EmptyUserName");
    }

    [Fact]
    public void User_Create_InvalidUserNameLength_ShouldReturnErrorInvalidUserNameLength()
    {
        var emailResult = Email.Create("test@email.com");
        var result = User.Create("ab", emailResult.Value, "passwordHash");

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "User.InvalidUserNameLength");
    }

    [Fact]
    public void User_Create_UserNameTooLong_ShouldReturnErrorInvalidUserNameLength()
    {
        var emailResult = Email.Create("test@email.com");
        string longUserName = new string('a', 51);
        var result = User.Create(longUserName, emailResult.Value, "passwordHash");

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "User.InvalidUserNameLength");
    }

    [Fact]
    public void User_Create_EmptyPasswordHash_ShouldReturnErrorEmptyPasswordHash()
    {
        var emailResult = Email.Create("test@email.com");
        var result = User.Create("ValidUser", emailResult.Value, " ");

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "User.EmptyPasswordHash");
    }
}
