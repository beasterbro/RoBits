using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Components/Base Component")]
public class BlockComponent : InterfaceObject
{
    //[SerializeField] private ... alignment; // handle this by the text attribute

    // Most subclasses should implement UpdateStyle
    // Most subclasses should implement LayoutObject
}
