using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Interface Objects/Drag and Drop (Singleton)")]
public class DragAndDropController : MonoBehaviour
{
    private static DragAndDropController _dndCtrl;
    public static DragAndDropController Instance() { return _dndCtrl; }
    public static bool IsPresent() { return _dndCtrl != null; }

    [Tooltip("Disables the ability to drag items. Stops dragging, grabbing, and dropping. Useful for pausing.")]
    [SerializeField] private bool isEnabled = true;
    [Range(0f, -10f)]
    [Tooltip("The z value of the item as it is being dragged.")]
    [SerializeField] private float grabZ = -0.1f;

    [Tooltip("The object currently being hovered over (that isn't held).")]
    [SerializeField] private InterfaceObject potential;
    [Tooltip("The object currently being dragged.")]
    [SerializeField] private Block held;
    [Tooltip("The z value of the item before it was grabbed and will be returned to on drop.")]
    [SerializeField] private float originalZ;
    [Tooltip("The (x, y) offset from when the object was grabbed.")]
    [SerializeField] private Vector3 originalOffset;
    [Tooltip("The original position of the held object in case the dropped position is invalid.")]
    [SerializeField] private Vector3 originalPosition;
    [Tooltip("The original container of the held object in case the dropped position is invalid.")]
    [SerializeField] private InterfaceObject originalContainer;

    void Awake()
    {
        if (DragAndDropController.IsPresent())
        {
            throw new NotSupportedException("Can not support multiple drag and drop controllers in one scene.");
        }
        else
        {
            DragAndDropController._dndCtrl = this;
        }
    }

    public void HoverOn(InterfaceObject obj)
    {
        potential = obj;
    }

    public void HoverOff(InterfaceObject obj)
    {
        if (potential == obj)
        {
            potential = null;
        }
    }

    private void Update()
    {
        if (IsHolding() && isEnabled)
        {
            held.transform.position = OffsetMousePosition();

            if (Input.GetMouseButtonUp(0))
            {
                // Notify potential of dropped item
                if (potential != null)
                {
                    potential.OnDrop();
                }
                else
                {
                    // No potential means it was dropped directly on the canvas
                    Drop();
                }
            }
        }
    }

    public Vector3 MousePosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = grabZ;
        return mousePos;
    }

    public Vector3 OffsetMousePosition()
    {
        return MousePosition() + originalOffset;
    }

    public bool IsHolding()
    {
        return held != null;
    }

    public Block GetHeld()
    {
        if (!IsHolding())
        {
            throw new NotSupportedException("Nothing is currently held.");
        }
        return held;
    }

    public void Grab(InterfaceObject obj)
    {
        if (obj is Block)
        {
            Grab((Block)obj, obj.GetContainer());
        }
    }

    public void Grab(Block block, InterfaceObject container)
    {
        if (isEnabled)
        {
            if (IsHolding())
            {
                throw new NotSupportedException("Can not grab multiple items at once.");
            }
            held = block;
            held.GetComponent<Collider2D>().enabled = false;

            Vector3 mousePos = MousePosition();
            originalOffset = block.transform.position - mousePos;
            originalOffset.z = 0;
            originalZ = block.transform.position.z;
            originalPosition = block.transform.position;
            originalContainer = container;
        }
    }

    public Block ResetDrop()
    {
        if (originalContainer == null)
        {
            return DropAt(originalPosition);
        }
        else
        {
            originalContainer.OnDrop();
            return null;
        }
    }

    public Block Drop()
    {
        Vector3 currentPosition = OffsetMousePosition();
        currentPosition.z = originalZ;

        Block result = DropAt(currentPosition);

        return result;
    }

    private Block DropAt(Vector3 position)
    {
        if (isEnabled && IsHolding())
        {
            // Reset object's original values (that are needed)
            Block result = GetHeld();
            result.transform.position = position;
            result.GetComponent<Collider2D>().enabled = true;

            // Get rid of current reference
            held = null;

            return result;
        }
        return null;
    }
}
