using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkInputComponent : BlockComponent
{
    [SerializeField] private List<Block> blocks;
    [SerializeField] private GameObject indicator;

    public Block At(int index)
    {
        return blocks[index];
    }

    public void Add(Block block)
    {
        blocks.Add(block);
        Entering(block);
        // TODO: probably some other stuff to setup the block
    }

    public void Insert(int index, Block block)
    {
        blocks.Insert(index, block);
        Entering(block);
        // TODO: probably some other stuff to setup the block
    }

    private void Entering(Block block)
    {
        block.SetContainer(this);
        block.transform.parent = this.transform;
    }

    public Block Remove(Block block)
    {
        blocks.Remove(block);
        Exiting(block);
        return block;
    }

    public Block RemoveAt(int index)
    {
        Block block = At(index);
        blocks.RemoveAt(index);
        Exiting(block);
        return block;
    }

    private void Exiting(Block block)
    {
        block.SetContainer(null);
        block.transform.parent = null;
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
            // TODO: Check that block is allowed in a block-list (Nothing return type OR Function Block?)
            result &= block.IsValid();
        });

        return result;
    }
    /*// TODO: Not finished implementing
    // Override meant to remove grabbed item from list
    protected override void OnGrab()
    {
        if (blocks.Count > 0)
        {
            int index = FindHoverIndex(DragAndDropController.Instance().MousePosition());
            DragAndDropController.Instance().Grab(RemoveAt(index), this);
        }
    }
    *//* TODO: Not finished implementing
    // Override meant to place item into index corresponding to it's position (upon dropping)
    public override void OnDrop()
    {
        // TODO: Check that block is allowed in a block-list (Nothing return type OR Function Block?)
        if (false) // Block not allowed condition
        {
            DragAndDropController.Instance().ResetDrop();
        }
        else
        {
            Block block = DragAndDropController.Instance().Drop();
            int index = FindHoverIndex(block.transform.position);
            Insert(index, block);
        }
    }
    */
    // Display indicator of where a dragged item would go if it can
    protected override void OnMouseOver()
    {
        base.OnMouseOver();
        if (DragAndDropController.Instance().IsHolding())
        {
            indicator.SetActive(true);
            indicator.transform.position =
                FindBlockPosition(DragAndDropController.Instance().MousePosition());
        }
    }

    // Remove indicator of dragged item placement
    protected override void OnMouseExit()
    {
        base.OnMouseExit();
        indicator.SetActive(false);
    }

    // The position of the last block above the given hover position
    private Vector3 FindBlockPosition(Vector3 hover)
    {
        int index = FindHoverIndex(hover);
        return index < 0 ? this.transform.position : At(index).transform.position;
    }

    // The hover index is defined as the index of the last item whose y-position is at or above
    // the hover position.
    private int FindHoverIndex(Vector3 hover)
    {
        return blocks.FindLastIndex(block => hover.y <= block.transform.position.y);
    }
}
