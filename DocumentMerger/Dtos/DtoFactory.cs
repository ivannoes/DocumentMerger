using Newtonsoft.Json.Linq;

public abstract class DtoGeneric 
{
    public Dictionary<string, object>? ToDictionary()
    {
        var jObj = JObject.FromObject(this);

        return jObj.ToObject<Dictionary<string, object>>();
    }
}

