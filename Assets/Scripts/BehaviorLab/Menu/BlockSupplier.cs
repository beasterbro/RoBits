using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSupplier : BlockTerminator
{

    [SerializeField] private string type;

    protected override void OnGrab()
    {
        if (DragAndDropController.IsAvailable())
        {
            var block = Block.FromType(type);
            block.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation);
            DragAndDropController.Instance().Grab(block);
            BehaviorLabController.GetShared().AddBlock(block);
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
