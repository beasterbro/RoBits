using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class PickerInputComponent : BlockComponent
{
    [SerializeField] private Dropdown dropdown;

    private void Start()
    {
        dropdown = dropdown ?? GetComponent<Dropdown>();
    }
}
