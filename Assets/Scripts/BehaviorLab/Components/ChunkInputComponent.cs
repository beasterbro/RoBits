using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkInputComponent : BlockComponent
{
    [SerializeField] private List<Block> blocks;
    [SerializeField] private GameObject indicator;

    public bool MatchesOutputType(Block block)
    {
        // TODO: OR Function Block?
        return ReturnType.EMPTY == block.OutputType();
    }

    // Returns true if index is within bounds [0, length). Otherwise, returns false.
    public bool WithinBounds(int index)
    {
        return WithinBounds(index, false);
    }

    // Returns true if index is within bounds [0, length). When isInclusive is specified as true,
    // also returns true if index is length (a.k.a. within range [0, length]). Otherwise, returns false.
    public bool WithinBounds(int index, bool isInclusive)
    {
        return 0 <= index && (index < blocks.Count || (isInclusive && index == blocks.Count));
    }

    public Block At(int index)
    {
        return blocks[index];
    }

    public void Add(Block block)
    {
        Insert(blocks.Count, block);
    }

    public void Insert(int index, Block block)
    {
        if (WithinBounds(index, true) && MatchesOutputType(block))
        {
            blocks.Insert(index, block);
            Entering(block);
        }
        else
        {
            Debug.Log("Did not add block: " + index);
        }
    }

    private void Entering(Block block)
    {
        block.SetContainer(this);
        block.transform.parent = this.transform;
        // TODO: probably some other stuff to setup the block
    }

    public Block Remove(Block block)
    {
        return RemoveAt(blocks.IndexOf(block));
    }

    public Block RemoveAt(int index)
    {
        if (WithinBounds(index))
        {
            Block block = At(index);
            blocks.RemoveAt(index);
            Exiting(block);
            return block;
        }
        return null;
    }

    private void Exiting(Block block)
    {
        block.SetContainer(null);
        block.transform.parent = null;
        // TODO: probably some other stuff to de-setup the block
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
    
    // Override meant to remove grabbed item from list
    protected override void OnGrab()
    {
        if (blocks.Count > 0)
        {
            int index = FindHoverIndex(DragAndDropController.Instance().MousePosition());
            DragAndDropController.Instance().Grab(RemoveAt(index), this);
            // TODO: update position of all elements after index
        }
    }
    
    // Override meant to place item into index corresponding to it's position (upon dropping)
    public override void OnDrop()
    {
        if (DragAndDropController.Instance().IsHolding())
        {
            if (MatchesOutputType(DragAndDropController.Instance().GetHeld()))
            {
                Block block = DragAndDropController.Instance().Drop();
                int index = FindHoverIndex(block.transform.position);
                Insert(index + 1, block);
                // TODO: update position of all elements after index (including newly added element)
            }
            else
            {
                DragAndDropController.Instance().ResetDrop();
            }
        }
    }

    // "Overloads" mouse down event for Blocks when they are inside the chunk
    private void OnMouseDown()
    {
        Debug.Log("Click " + this);
        if (DragAndDropController.IsPresent() && !DragAndDropController.Instance().IsHolding())
        {
            this.OnGrab();
        }
    }

    // Display indicator of where a dragged item would go if it can
    protected override void OnMouseOver()
    {
        base.OnMouseOver();
        if (DragAndDropController.Instance().IsHolding())
        {
            indicator.SetActive(true);
            indicator.transform.position =
                FindBlockPosition(DragAndDropController.Instance().OffsetMousePosition());
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
