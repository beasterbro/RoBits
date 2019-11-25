using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTerminator : InterfaceObject
{
    public override void OnDrop()
    {
        if (DragAndDropController.IsOccupied())
        {
            var dropped = DragAndDropController.Instance().Drop();
            BehaviorLabController.GetShared().RemoveBlock(dropped);
            Destroy(dropped.gameObject);
        }
    }
}
