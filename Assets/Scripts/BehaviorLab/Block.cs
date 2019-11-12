using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[AddComponentMenu("Interface Objects/Block")]
public abstract class Block : InterfaceObject
{
    [SerializeField] private bool isMovable = false;
    [SerializeField] private bool isDeletable = false;
    [SerializeField] private Block prev; // Block that precedes, if any
    [SerializeField] private Block next; // Block that follows, if any
    [SerializeField] private Block containing; // Block to return an output to

    public abstract ReturnType OutputType();

    // Executes the code the block represents, either returning a value or performing
    // some action and calling evaluate on the block's successor
    public BehaviorData Evaluate()
    {
        BehaviorData result = InnerEvaluate();
        if (result.GetType() != OutputType())
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

    // Generates and lists out all BlockInfo states below this and including this
    public List<BlockInfo> States(int startingId)
    {
        List<BlockInfo> working_states = new List<BlockInfo>();
        foreach (Block child in Children())
        {
            working_states.AddRange(child.States(startingId + 1 + working_states.Count));
        }
        working_states.Add(State(startingId, working_states.Count));
        return working_states;
    }

    // Generates the BlockInfo state for this object given a unique id and the highest id of their children.
    // This assumes that all ids falling between id (non-inclusive) and maxChildId (inclusive) are children to this Block.
    public BlockInfo State(int id, int maxChildId)
    {
        return new BlockInfo(id, Type(), new Dictionary<string, string>(), IDs(id + 1, maxChildId), ChunkSizes());
    }

    private int[] IDs(int first, int last)
    {
        return Enumerable.Range(first, last - first + 1).ToArray();
    }

    protected abstract List<Block> Children();

    protected abstract string Type();

    protected abstract int[] ChunkSizes();

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
}
