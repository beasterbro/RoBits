using System;
using System.Collections.Generic;
using System.Linq;

public class BehaviorExecutor
{

    private static Dictionary<string, Func<BehaviorExecutor, BlockInfo, object>> executionFunctions;

    // TODO: Is this the best way to do this?
    public static void LoadExecutionFunctions()
    {
        executionFunctions = new Dictionary<string, Func<BehaviorExecutor, BlockInfo, object>>();

        executionFunctions["if"] = (executor, info) =>
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
        };

        executionFunctions["not"] = (executor, info) =>
        {
            if (info.InputIDs.Length > 0 && executor.ExecuteBlock(info.InputIDs[0]) is bool logicalInput)
            {
                return !logicalInput;
            }

            return null;
        };
    }

    private readonly BehaviorInfo behavior;

    public BehaviorExecutor(BehaviorInfo behavior)
    {
        this.behavior = behavior;
    }

    private object ExecuteBlock(int id)
    {
        var block = behavior.Blocks.FirstOrDefault(bi => bi.ID == id);
        if (block != null && executionFunctions.ContainsKey(block.Type))
            return executionFunctions[block.Type].Invoke(this, block);

        return null;
    }

    public void Execute() => ExecuteBlock(behavior.EntryBlockId);

}