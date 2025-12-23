public class AddressDto : IDtoGeneric
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}

public class AddressDtoCreator : IDtoCreator
{
    public IDtoGeneric CreateDto(IDtoGeneric dtoData)
    {
        var data = dtoData as AddressDto;
        return data;
    }
}