using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Block : InterfaceObject
{
    [SerializeField] private List<BlockLine> lines = new List<BlockLine>();
    [SerializeField] private bool isMovable = false;
    [SerializeField] private bool isDeletable = false;
    [SerializeField] private ReturnType outputType = ReturnType.EMPTY;
    //[SerializeField] private Color textColor; // Handled by component?
    [SerializeField] private Block prev; // Block that precedes, if any
    [SerializeField] private Block next; // Block that follows, if any
    [SerializeField] private Block containing; // Block to return an output to

    public ReturnType GetOutputType()
    {
        return this.outputType;
    }

    // Executes the code the block represents, either returning a value or performing
    // some action and calling evaluate on the block's successor
    public BehaviorData Evaluate()
    {
        BehaviorData result = InnerEvaluate();
        if (result.GetType() != outputType)
        {
            throw new SystemException("Unexpected return type.");
        }
        next.Evaluate();
        return result;
    }

    // Evaluation specific to this block.
    protected abstract BehaviorData InnerEvaluate();

    // Checks the code-validity of this block's structure.
    public abstract bool IsValid();

    private void OnMouseDown()
    {
        Debug.Log("Click " + this);
        if (DragAndDropController.IsPresent())
        {
            AttemptGrab();
        }
    }

    private void AttemptGrab()
    {
        if (isMovable && !DragAndDropController.Instance().IsHolding())
        {
            this.OnGrab();
        }
    }

    // Move the block to a specific point in the view
    public void Move(Vector3 pos)
    {
        this.transform.position = pos;
    }

    // Position any blocks connected to this block appropriately
    public void PositionConnections()
    {
        // TODO: this sounds super complicated
    }

    // Connects the block to a new predecessor in the behavior
    public void FormUpperConnection(Block predecessor)
    {
        this.prev = predecessor;
    }

    // Connects the block to a new successor in the hierarchy
    public void FormLowerConnection(Block successor)
    {
        this.next = successor;
        //this.PositionConnections(); // ??
    }

    // Disconnects the block from is predecessor and returns the predecessor
    public Block DisconnectUpperConnection()
    {
        Block result = this.prev;
        this.prev = null;
        return result;
    }

    // Disconnects the block from is successor and returns the successor
    public Block DisconnectLowerConnection()
    {
        Block result = this.next;
        this.next = null;
        return result;
    }

    // Adds a new BlockLine to the block's structure, and sets itself as the line's container
    public void AddLine(BlockLine line)
    {
        lines.Add(line);
        // TODO: Not sure if the container part makes sense
    }

    // Removes a line from the block's structure, and removes itself as the line's container
    public void RemoveLine(BlockLine line)
    {
        lines.Remove(line);
        // TODO: Not sure if the container part makes sense
    }

    // Returns a duplicate of the current block, with all of the same internal properties but no connections
    public Block MakeDuplicate()
    {
        // TODO: this should likely be handled via a controller and prefabs
        return this;
    }
}
