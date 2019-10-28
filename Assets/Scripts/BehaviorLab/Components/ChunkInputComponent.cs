using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Components/Chunk Component")]
public class ChunkInputComponent : BlockComponent
{
    [SerializeField] private ScalingList<Block> _blocks;
    [SerializeField] private List<Block> _elements;
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject elementContainer;

    private ScalingList<Block> blocks
    {
        get
        {
            if (_blocks == null)
            {
                _blocks = new ScalingList<Block>(this.transform);
            }
            return _blocks;
        }
    }

    protected override void Start()
    {
        base.Start();
        _elements = blocks.elements;
    }

    public override void Redraw()
    {
        blocks.UpdateScale();
        base.Redraw();
    }

    private void RedrawParent()
    {
        base.Redraw();
    }

    public void LinkScaleController(ScaleController scaleController)
    {
        blocks.LinkScaleController(scaleController);
    }

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
        block.transform.parent = this.elementContainer.transform;
        // TODO: probably some other stuff to setup the block
        RedrawParent();
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
        RedrawParent();
    }

    public BehaviorData Evaluate()
    {
        foreach (Block block in blocks)
        {
            block.Evaluate();
        }
        return BehaviorData.EMPTY;
    }

    public bool IsValid()
    {
        bool result = true;
        foreach (Block block in blocks)
        {
            result &= block.IsValid();
        }
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
        if (DragAndDropController.IsOccupied())
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
        if (DragAndDropController.IsAvailable())
        {
            this.OnGrab();
        }
    }

    // Display indicator of where a dragged item would go if it can
    protected override void OnOver()
    {
        if (DragAndDropController.IsOccupied())
        {
            indicator.SetActive(true);
            indicator.transform.position =
                FindBlockPosition(DragAndDropController.Instance().OffsetMousePosition());
        }
    }

    // Remove indicator of dragged item placement
    protected override void OnExit()
    {
        indicator.SetActive(false);
    }

    // The position of the last block above the given hover position
    private Vector3 FindBlockPosition(Vector3 hover)
    {
        int index = FindHoverIndex(hover);
        return index < 0 ? this.transform.position : At(index).Bounds().BottomLeft;
    }

    // The hover index is defined as the index of the last item whose y-position is at or above
    // the hover position.
    private int FindHoverIndex(Vector3 hover)
    {
        return blocks.FindLastIndex(block => hover.y <= block.Bounds().TopLeft.y);
    }
}
