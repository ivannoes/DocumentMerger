using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentMergerTests;

[TestClass]
public class DtoGenericTests
{
    [TestMethod]
    public void ToDictionary_UserDto_ReturnsCorrectDictionary()
    {
        var userDto = new UserDto
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com"
        };

        var result = userDto.ToDictionary();

        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual(1, result["Id"]);
        Assert.AreEqual("John Doe", result["Name"]);
        Assert.AreEqual("john@example.com", result["Email"]);
    }

    [TestMethod]
    public void ToDictionary_AddressDto_ReturnsCorrectDictionary()
    {
        var addressDto = new AddressDto
        {
            Street = "123 Main St",
            City = "Springfield",
            State = "IL",
            ZipCode = "62701"
        };

        var result = addressDto.ToDictionary();

        Assert.IsNotNull(result);
        Assert.AreEqual(4, result.Count);
        Assert.AreEqual("123 Main St", result["Street"]);
        Assert.AreEqual("Springfield", result["City"]);
        Assert.AreEqual("IL", result["State"]);
        Assert.AreEqual("62701", result["ZipCode"]);
    }

    [TestMethod]
    public void ToDictionary_WithNullValues_HandlesGracefully()
    {
        var addressDto = new AddressDto
        {
            Street = "456 Oak Ave",
            City = null,
            State = "NY",
            ZipCode = null
        };

        var result = addressDto.ToDictionary();

        Assert.IsNotNull(result);
        Assert.AreEqual(4, result.Count);
        Assert.AreEqual("456 Oak Ave", result["Street"]);
        Assert.IsNull(result["City"]);
        Assert.AreEqual("NY", result["State"]);
        Assert.IsNull(result["ZipCode"]);
    }

    [TestMethod]
    public void ToDictionary_EmptyDto_ReturnsEmptyDictionary()
    {
        var userDto = new UserDto
        {
            Id = 0,
            Name = null,
            Email = null
        };

        var result = userDto.ToDictionary();

        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);
    }

    [TestMethod]
    public void ToDictionary_UserDtoWithAllNullProperties_ReturnsDictionaryWithNullValues()
    {
        var userDto = new UserDto
        {
            Id = 42,
            Name = null,
            Email = null
        };

        var result = userDto.ToDictionary();

        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual(42, result["Id"]);
        Assert.IsNull(result["Name"]);
        Assert.IsNull(result["Email"]);
    }
}
