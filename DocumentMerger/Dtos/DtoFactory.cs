public interface IDtoGeneric { }
public interface IDtoCreator
{
    IDtoGeneric CreateDto(IDtoGeneric dtoData);
}

public class DtoCreator : IDtoCreator
{
    private readonly IDtoGeneric _dtoData;
    public DtoCreator(IDtoGeneric dtoData)
    {
        _dtoData = dtoData;
    }
    public IDtoGeneric CreateDto(IDtoGeneric dtoData)
    {
        return dtoData switch
        {
            UserDto => new UserDtoCreator().CreateDto(dtoData),
            AddressDto => new AddressDtoCreator().CreateDto(dtoData),
            _ => throw new NotSupportedException("DTO type not supported"),
        };
    }
}

