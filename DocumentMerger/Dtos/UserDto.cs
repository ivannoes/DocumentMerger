public class UserDto : IDtoGeneric
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public class UserDtoCreator : IDtoCreator
{
    public IDtoGeneric CreateDto(IDtoGeneric dtoData)
    {
        var data = dtoData as UserDto;
        return data;
    }
}