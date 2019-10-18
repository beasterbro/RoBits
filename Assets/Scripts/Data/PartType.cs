using Newtonsoft.Json;

[JsonConverter(typeof(JsonData.PartTypeConverter))]
public enum PartType
{

    BodyType = 0,
    Armor,
    Sensor,
    Weapon,
    Transport,
    Cpu

}