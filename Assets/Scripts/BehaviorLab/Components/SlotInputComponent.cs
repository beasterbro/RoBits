using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotInputComponent : BlockComponent
{
    [SerializeField] private Block slot;

    public bool IsSlotAvailable()
    {
        return slot == null;
    }

    public void Push(Block block)
    {
        if (IsSlotAvailable())
        {
            slot = block;
            // TODO: probably some other stuff to setup the block
        }
    }

    public Block Pop()
    {
        Block result = slot;
        this.slot = null;
        return result;
    }
}
