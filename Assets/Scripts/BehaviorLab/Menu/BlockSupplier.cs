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
            Vector3 pos = this.transform.position;
            pos.z = toSupply.transform.localPosition.z;
            block.transform.position = pos;
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
