using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTerminator : InterfaceObject
{
    public override void OnDrop()
    {
        if (DragAndDropController.IsOccupied())
        {
            Destroy(DragAndDropController.Instance().Drop().gameObject);
        }
    }
}
