using System.Collections.Generic;
using TriggerFunction = System.Func<BotController, SensorPartController, bool>;

public class TriggerInfo
{

    private readonly int id;
    private readonly PartInfo sensor;
    private readonly string name;

    public TriggerInfo(int id, PartInfo sensor, string name)
    {
        this.id = id;
        this.sensor = sensor;
        this.name = name;
    }

    public int ID => id;
    public PartInfo Sensor => sensor;
    public string Name => name;

    public static Dictionary<int, (TriggerInfo, TriggerFunction)> triggers = new Dictionary<int, (TriggerInfo, TriggerFunction)>();

    public static void LoadTriggers()
    {
        var proxSensor = DataManager.Instance.GetPart(400);
        var visionSensor = DataManager.Instance.GetPart(401);

        triggers[0] = (new TriggerInfo(0, null, "Bot is idle"), (bot, _) => !bot.isDead);
        
        triggers[1] = (new TriggerInfo(1, proxSensor, "Enemy within range"), (bot, sensor) =>
        {
            if (sensor is ProximitySensor proximitySensor) return proximitySensor.OpponentIsInRange();
            return false;
        });
        
        triggers[2] = (new TriggerInfo(2, proxSensor, "Enemy nearby"), (bot, sensor) =>
        {
            if (sensor is ProximitySensor proximitySensor) return proximitySensor.ShouldMoveTowardsNearestOpponent();
            return false;
        });
        
        // TODO: Implement
        triggers[3] = (new TriggerInfo(3, visionSensor, "Enemy in sight"), (bot, sensor) => false);
    }
    
}