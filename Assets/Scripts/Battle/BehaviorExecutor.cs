using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BehaviorExecutor
{

    public readonly BotController bot;
    public readonly BehaviorInfo behavior;

    public BehaviorExecutor(BotController bot, BehaviorInfo behavior)
    {
        this.bot = bot;
        this.behavior = behavior;
    }

    public bool IsApplicable()
    {
        if (behavior.Trigger.Sensor != null)
        {
            var sensor = bot.sensors.FirstOrDefault(botSensor => botSensor.info.ResourceName == behavior.Trigger.Sensor.ResourceName);
            if (sensor != null) return TriggerInfo.triggers[behavior.TriggerId].Item2(bot, sensor);
        }
        else
        {
            return TriggerInfo.triggers[behavior.TriggerId].Item2(bot, null);
        }

        return false;
    }

    // Execute a block in the behavior by ID, if it exists
    private object ExecuteBlock(int id)
    {
        var block = behavior.Blocks.FirstOrDefault(bi => bi.ID == id);

        if (block != null && executionFunctions.ContainsKey(block.Type.ToLower()))
            return executionFunctions[block.Type.ToLower()].Invoke(this, block);

        return null;
    }

    public void Execute() => ExecuteBlock(behavior.EntryBlockId);

    // Static reference for all execution functions
    private static Dictionary<string, Func<BehaviorExecutor, BlockInfo, object>> executionFunctions = new Dictionary<string, Func<BehaviorExecutor, BlockInfo, object>>
    {
        ["trigger"] = (executor, info) =>
        {
            foreach (var blockId in info.InputIDs)
            {
                executor.ExecuteBlock(blockId);
            }

            return null;
        },
        ["if"] = (executor, info) =>
        {
            if (info.InputIDs.Length > 0)
            {
                // If the condition is true...
                if (executor.ExecuteBlock(info.InputIDs[0]) is bool cond && cond)
                {
                    // And if there are blocks to execute...
                    if (info.InputIDs.Length > 1)
                    {
                        // Execute them
                        for (var i = 1; i < info.InputIDs.Length; i++)
                            executor.ExecuteBlock(info.InputIDs[i]);
                    }
                }
            }

            return null;
        },
        ["not"] = (executor, info) =>
        {
            if (info.InputIDs.Length > 0 && executor.ExecuteBlock(info.InputIDs[0]) is bool logicalInput)
            {
                return !logicalInput;
            }

            return null;
        },
        ["move"] = (executor, info) =>
        {
            var movementDirection = info.TypeAttrs["direction"];
            var motionController = executor.bot.parts.FirstOrDefault(part => part is MovementController);

            if (motionController != null && motionController is WheelsController wheels)
            {
                switch (movementDirection)
                {
                    case "stop":
                        wheels.SetForward(0f);
                        wheels.SetTurning(0f);
                        break;
                    case "forward":
                        wheels.SetForward(1f);
                        break;
                    case "backward":
                        wheels.SetForward(-1f);
                        break;
                    case "left":
                        wheels.SetTurning(-20f);
                        break;
                    case "right":
                        wheels.SetTurning(20f);
                        break;
                }
            }

            return null;
        },
        ["shootat"] = (executor, info) =>
        {
            if (info.InputIDs.Length > 0 && executor.ExecuteBlock(info.InputIDs[0]) is BotController enemy && enemy != null)
            {
                executor.bot.guns.ForEach(gun =>
                {
                    if (info.TypeAttrs["weapon"] == null || info.TypeAttrs["weapon"] == gun.info.ResourceName)
                    {
                        gun.FocusOn(enemy.gameObject.transform);
                        gun.Fire();
                    }
                });
            }

            return null;
        },
        ["targetprox"] = (executor, info) =>
        {
            var proxSensor = (ProximitySensor) executor.bot.parts.FirstOrDefault(part => part is ProximitySensor);
            if (proxSensor != null)
            {
                var n = int.Parse(info.TypeAttrs["n"] ?? "1");
                BotController target = TargetingManager.NthTarget(n, info.TypeAttrs["priority"], executor.bot);
                return target;
            }
            return null;
        },
        ["targetvision"] = (executor, info) =>
        {
            var proxSensor = (VisionSensor) executor.bot.parts.FirstOrDefault(part => part is VisionSensor);
            if (proxSensor != null)
            {
                var n = int.Parse(info.TypeAttrs["n"] ?? "1");
                return TargetingManager.NthTarget(n, info.TypeAttrs["priority"], executor.bot);
            }

            return null;
        },
        ["target"] = (executor, info) => {
            var n = int.Parse(info.TypeAttrs["n"] ?? "1");
            return TargetingManager.NthTarget(n, info.TypeAttrs["priority"], executor.bot);
        },
        ["condition"] = (executor, info) =>
        {
            if (executor.ExecuteBlock(info.InputIDs[0]) is BotController target && target != null)
            {
                return TargetingManager.TargetSatisfiesCondition(info.TypeAttrs["check"], executor.bot, target);
            }

            return false;
        }
    };

}