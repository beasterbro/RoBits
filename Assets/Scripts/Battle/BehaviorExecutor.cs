using System;
using System.Collections.Generic;
using System.Linq;

public class BehaviorExecutor
{

    public readonly BotController bot;
    public readonly BehaviorInfo behavior;

    public BehaviorExecutor(BotController bot, BehaviorInfo behavior)
    {
        this.bot = bot;
        this.behavior = behavior;
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
                switch (movementDirection) {
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
            throw new NotImplementedException();
            return null;
        },
        ["target"] = (executor, info) =>
        {
            throw new NotImplementedException();
            return null;
        }
    };

}