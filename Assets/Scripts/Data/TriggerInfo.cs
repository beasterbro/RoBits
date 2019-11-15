using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sensors;

public class TriggerInfo
{
    private readonly int id;
    private readonly SensorType sensor;
    private readonly string name;

    public TriggerInfo(int id, string sensor, string name)
    {
        this.id = id;
        this.sensor = SensorTypeHelper.Parse(sensor);
        this.name = name;
    }

    public TriggerInfo(int id, SensorType sensor, string name)
    {
        this.id = id;
        this.sensor = sensor;
        this.name = name;
    }

    public int ID => id;
    public SensorType Sensor => sensor;
    public string Name => name;
}
