using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentMergerTests.Tests.Dto;

[TestClass]
public class AddressDtoTests
{
    [TestMethod]
    public void AddressDto_CanSetProperties()
    {
        var addressDto = new AddressDto
        {
            Street = "789 Elm Street",
            City = "Portland",
            State = "OR",
            ZipCode = "97201"
        };

        Assert.AreEqual("789 Elm Street", addressDto.Street);
        Assert.AreEqual("Portland", addressDto.City);
        Assert.AreEqual("OR", addressDto.State);
        Assert.AreEqual("97201", addressDto.ZipCode);
    }

    [TestMethod]
    public void AddressDto_PropertiesAreNullable()
    {
        var addressDto = new AddressDto
        {
            Street = null,
            City = null,
            State = null,
            ZipCode = null
        };

        Assert.IsNull(addressDto.Street);
        Assert.IsNull(addressDto.City);
        Assert.IsNull(addressDto.State);
        Assert.IsNull(addressDto.ZipCode);
    }

    [TestMethod]
    public void AddressDto_SerializesCorrectly()
    {
        var addressDto = new AddressDto
        {
            Street = "100 Test Blvd",
            City = "Testville",
            State = "TS",
            ZipCode = "00000"
        };

        var dict = addressDto.ToDictionary();

        Assert.IsNotNull(dict);
        Assert.AreEqual("100 Test Blvd", dict["Street"]);
        Assert.AreEqual("Testville", dict["City"]);
        Assert.AreEqual("TS", dict["State"]);
        Assert.AreEqual("00000", dict["ZipCode"]);
    }

    [TestMethod]
    public void AddressDto_InheritsFromDtoGeneric()
    {
        var addressDto = new AddressDto();

        Assert.IsInstanceOfType(addressDto, typeof(DtoGeneric));
    }

    [TestMethod]
    public void AddressDto_PartialProperties()
    {
        var addressDto = new AddressDto
        {
            Street = "Partial Address",
            City = "Some City"
        };

        Assert.AreEqual("Partial Address", addressDto.Street);
        Assert.AreEqual("Some City", addressDto.City);
        Assert.IsNull(addressDto.State);
        Assert.IsNull(addressDto.ZipCode);
    }
}
