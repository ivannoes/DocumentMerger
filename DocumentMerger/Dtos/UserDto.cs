public class UserDto : DtoGeneric
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public class UserDtoCreator : IDtoCreator
{
    public DtoGeneric CreateDto(DtoGeneric dtoData)
    {
        var data = dtoData as UserDto;
        return data;
    }
}