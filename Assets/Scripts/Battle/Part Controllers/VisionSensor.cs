using System.Collections.Generic;
using System.Linq;

public class VisionSensor : SensorPartController
{

    public override void MakeObservations() { }

    public bool BotHasPartOfType(BotController otherBot, string partName)
    {
        return otherBot.parts.Exists(part => part.info.Name == partName);
    }

    public List<T> PartsOnBotOfType<T>(BotController otherBot, string partName) where T : PartController
    {
        return otherBot.parts.FindAll(part => part.info.Name == partName).Cast<T>().ToList();
    }

}