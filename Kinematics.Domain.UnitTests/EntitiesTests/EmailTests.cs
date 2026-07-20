using FluentAssertions;
using KinematicsSimulator.Domain.Entities.ValueObjects;

namespace Kinematics.Domain.UnitTests.EntitiesTests;

public class EmailTests
{
    [Fact]
    public void Email_Create_GoodValues_ShouldReturnEmail()
    {
        const string email = "testEmail@gmail.com";

        var result = Email.Create(email);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("testemail@gmail.com");
    }

    [Fact]
    public void Email_Create_EmailEmptyWithWhiteSpace_ShouldReturnErrorEmailEmpty()
    {
        const string email = " ";

        var result = Email.Create(email);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "Email.Empty");
    }

    [Fact]
    public void Email_Create_EmailWith300Char_ShouldReturnErrorEmailMaxLeghtLimitExceeded()
    {
        const string email = "test-local-part-test-local-part-test-local-part-test-local-part-@dominio-di-test-dominio-di-test-dominio-di-test-dominio-di-test-dominio-di-test-dominio-di-test-dominio-di-test-dominio-di-test-dominio-di-test-dominio-di-test-dominio-di-test-dominio-di-test-dominio-di-test-dominio-di-test-1234567.com";

        var result = Email.Create(email);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "Email.MaxLeghtLimitExceeded");
    }

    [Fact]
    public void Email_Create_EmailWithSpecialChar_ShouldReturnErrorEmailInvalidEmailFormat()
    {
        const string email = "testEmailgmail.com";

        var result = Email.Create(email);

        result.IsFailure.Should().BeTrue();
        result.errorList.Should().ContainSingle(e => e.Name == "Email.InvalidEmailFormat");
    }
}
