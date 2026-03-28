using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentMergerTests.Tests.Dto;

[TestClass]
public class UserDtoTests
{
    [TestMethod]
    public void UserDto_CanSetProperties()
    {
        var userDto = new UserDto
        {
            Id = 123,
            Name = "Jane Doe",
            Email = "jane.doe@example.com"
        };

        Assert.AreEqual(123, userDto.Id);
        Assert.AreEqual("Jane Doe", userDto.Name);
        Assert.AreEqual("jane.doe@example.com", userDto.Email);
    }

    [TestMethod]
    public void UserDto_PropertiesAreNullable()
    {
        var userDto = new UserDto
        {
            Id = 0,
            Name = null,
            Email = null
        };

        Assert.AreEqual(0, userDto.Id);
        Assert.IsNull(userDto.Name);
        Assert.IsNull(userDto.Email);
    }

    [TestMethod]
    public void UserDto_SerializesCorrectly()
    {
        var userDto = new UserDto
        {
            Id = 1,
            Name = "Test User",
            Email = "test@test.com"
        };

        var dict = userDto.ToDictionary();

        Assert.IsNotNull(dict);
        Assert.AreEqual(1, dict["Id"]);
        Assert.AreEqual("Test User", dict["Name"]);
        Assert.AreEqual("test@test.com", dict["Email"]);
    }

    [TestMethod]
    public void UserDto_InheritsFromDtoGeneric()
    {
        var userDto = new UserDto();

        Assert.IsInstanceOfType(userDto, typeof(DtoGeneric));
    }

    [TestMethod]
    public void UserDto_CanBeInstantiatedMultipleTimes()
    {
        var user1 = new UserDto { Id = 1, Name = "User1", Email = "user1@test.com" };
        var user2 = new UserDto { Id = 2, Name = "User2", Email = "user2@test.com" };

        Assert.AreNotSame(user1, user2);
        Assert.AreEqual(1, user1.Id);
        Assert.AreEqual(2, user2.Id);
    }
}
