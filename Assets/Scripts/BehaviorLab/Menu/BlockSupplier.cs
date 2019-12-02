using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSupplier : BlockTerminator
{
    [SerializeField] private string type;
    [SerializeField] private Image cover;
    [SerializeField] private bool canBeDeactivated;
    [SerializeField] private Block exampleBlock;

    private static List<BlockSupplier> subscribers = new List<BlockSupplier>();

    public static void UpdateActivity()
    {
        subscribers.ForEach(s => s.UpdateActive());
    }

    protected virtual void UpdateActive()
    {
        if (cover != null) cover.gameObject.SetActive(!IsActive());
    }

    protected override void Start()
    {
        base.Start();
        subscribers.Add(this);
        UpdateActive();
    }

    protected override void OnGrab()
    {
        if (IsActive() && DragAndDropController.IsAvailable())
        {
            var block = Block.FromType(type);
            block.info.ID = BehaviorLabController.GetShared().NextBlockID();
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

    private bool IsActive()
    {
        return canBeDeactivated ? exampleBlock.IsValid() : true;
    }
}