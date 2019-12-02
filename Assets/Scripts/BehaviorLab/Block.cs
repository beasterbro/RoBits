using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

[AddComponentMenu("Interface Objects/Block")]
public abstract class Block : InterfaceObject
{

    public BlockInfo info;
    [SerializeField] private bool isMovable = false;
    [SerializeField] private bool isDeletable = false;
    [SerializeField] private Block prev; // Block that precedes, if any
    [SerializeField] private Block next; // Block that follows, if any
    [SerializeField] private Block containing; // Block to return an output to

    public static Block FromType(string type)
    {
        try
        {
            var prefab = Instantiate(Resources.Load<GameObject>("Behavior Lab/" + type + "Block"));
            var block = prefab.GetComponent<Block>();
            block.GenerateInfo();
            return block;
        }
        catch (ArgumentException)
        {
            Debug.Log("Unable to load block of type " + type);
            return null;
        }
    }

    public static Block FromInfo(BlockInfo info)
    {
        var block = Block.FromType(info.Type);
        if (block != null) block.info = info;
        block.ApplyTypeAttributes();

        return block;
    }

    private void GenerateInfo() => info = MakeNewInfo();

    protected virtual BlockInfo MakeNewInfo()
    {
        return new BlockInfo(0, Type(), TypeAttributes(), InputIDs(), ChunkSizes());
    }

    protected virtual void ApplyTypeAttributes() { }

    public abstract ReturnType OutputType();

    // Executes the code the block represents, either returning a value or performing
    // some action and calling evaluate on the block's successor
    public BehaviorData Evaluate()
    {
        BehaviorData result = InnerEvaluate();
        if (result.GetReturnType() != OutputType())
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
    // If a state is null it is because there was no block in the defined space involved
    public List<BlockInfo> States()
    {
        List<BlockInfo> working_states = new List<BlockInfo>();
        foreach (Block child in Children())
        {
            if (child != null)
            {
                working_states.AddRange(child.States());
            }
            else
            {
                working_states.Add(null);
                Debug.Log("Found a null within " + Type() + " statement! - Block");
            }
        }

        working_states.Add(State());
        return working_states;
    }

    // Generates the BlockInfo state for this object given a unique id and the highest id of their children.
    // This assumes that all ids falling between id (non-inclusive) and maxChildId (inclusive) are children to this Block.
    public BlockInfo State()
    {
        return new BlockInfo(info.ID, Type(), TypeAttributes(), InputIDs(), ChunkSizes());
    }

    protected virtual List<Block> Children() => new List<Block>();
    protected virtual Dictionary<string, string> TypeAttributes() => new Dictionary<string, string>();
    protected abstract string Type();
    protected virtual int[] InputIDs() => new int[0];
    protected virtual int[] ChunkSizes() => new int[0];

    // Move the block to a specific point in the view
    public void Move(Vector3 pos)
    {
        this.transform.position = pos;
    }

    protected virtual void SetupScaleControllers()
    {
        if (scaleController == null) scaleController = GetComponent<ScaleController>();
    }

    // Position any blocks connected to this block appropriately
    public virtual void PositionConnections() { }

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