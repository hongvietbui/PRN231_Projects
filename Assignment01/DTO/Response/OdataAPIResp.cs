using System.Text.Json.Serialization;

namespace Assignment01.DTO;

public class OdataAPIResp<T>
{   
    [JsonPropertyName("@odata.context")]
    public string Context { get; set; }
    public T Value { get; set; }
}