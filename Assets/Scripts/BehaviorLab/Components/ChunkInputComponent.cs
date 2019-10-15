using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkInputComponent : BlockComponent
{
    [SerializeField] private List<Block> blocks;

    public void Add(Block block)
    {
        blocks.Add(block);
        // TODO: probably some other stuff to setup the block
    }

    public void Insert(int index, Block block)
    {
        blocks.Insert(index, block);
        // TODO: probably some other stuff to setup the block
    }

    public void Remove(Block block)
    {
        blocks.Remove(block);
    }

    public BehaviorData Evaluate()
    {
        blocks.ForEach(block => block.Evaluate());
        return BehaviorData.EMPTY;
    }

    public bool IsValid()
    {
        bool result = true;

        blocks.ForEach(block =>
        {
            result &= block.IsValid();
        });

        return result;
    }
}
