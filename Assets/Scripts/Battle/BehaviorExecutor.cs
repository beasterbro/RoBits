using System;
using System.Collections.Generic;
using System.Linq;

public class BehaviorExecutor
{

    private readonly BehaviorInfo behavior;

    public BehaviorExecutor(BehaviorInfo behavior)
    {
        this.behavior = behavior;
    }

    // Execute a block in the behavior by ID, if it exists
    private object ExecuteBlock(int id)
    {
        var block = behavior.Blocks.FirstOrDefault(bi => bi.ID == id);

        if (block != null && executionFunctions.ContainsKey(block.Type))
            return executionFunctions[block.Type].Invoke(this, block);

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
        }
    };

}