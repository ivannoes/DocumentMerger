using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
public abstract class DtoGeneric 
{
    public Dictionary<string, object>? ToDictionary()
    {
        var jObj = JObject.FromObject(this);

        return jObj.ToObject<Dictionary<string, object>>();
    }
}
public interface IDtoCreator
{
    DtoGeneric CreateDto(DtoGeneric dtoData);
}

public class DtoCreator : IDtoCreator
{
    private readonly DtoGeneric _dtoData;
    public DtoCreator(DtoGeneric dtoData)
    {
        _dtoData = dtoData;
    }
    public DtoGeneric CreateDto(DtoGeneric dtoData)
    {
        return dtoData switch
        {
            UserDto => new UserDtoCreator().CreateDto(dtoData),
            AddressDto => new AddressDtoCreator().CreateDto(dtoData),
            _ => throw new NotSupportedException("DTO type not supported"),
        };
    }
}

