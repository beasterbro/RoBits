using System.Collections.Generic;

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

    public static Dictionary<int, TriggerInfo> triggers = new Dictionary<int, TriggerInfo>();

    public static void LoadTriggers()
    {
        var proxSensor = DataManager.Instance.GetPart(400);
        var visionSensor = DataManager.Instance.GetPart(401);

        triggers[0] = new TriggerInfo(0, null, "Bot is idle");
        triggers[1] = new TriggerInfo(1, proxSensor, "Enemy within range");
        triggers[2] = new TriggerInfo(2, proxSensor, "Enemy nearby");
        triggers[3] = new TriggerInfo(3, visionSensor, "Enemy in sight");
    }
    
}