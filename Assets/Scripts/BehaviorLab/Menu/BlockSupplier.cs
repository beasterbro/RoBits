using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSupplier : BlockTerminator
{
    [SerializeField] private Block toSupply;

    protected override void OnGrab()
    {
        if (DragAndDropController.IsAvailable())
        {
            Block block = Instantiate<Block>(toSupply);
            block.transform.position = this.transform.position;
            DragAndDropController.Instance().Grab(block);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Click " + this);
        if (DragAndDropController.IsAvailable())
        {
            this.OnGrab();
        }
    }
}
