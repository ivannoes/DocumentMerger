public class AddressDto : DtoGeneric
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}

public class AddressDtoCreator : IDtoCreator
{
    public DtoGeneric CreateDto(DtoGeneric dtoData)
    {
        var data = dtoData as AddressDto;
        return data;
    }
}