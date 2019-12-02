using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class DropdownComponent : BlockComponent
{
    public delegate List<string> Supplier();

    private Supplier supplier;
    [SerializeField] private Dropdown dropdown;

    protected override void Start()
    {
        dropdown = dropdown ?? GetComponent<Dropdown>();
        base.Start();
    }

    public void SetSupplier(Supplier supplier)
    {
        this.supplier = supplier;
        UpdateOptions();
    }

    private void UpdateOptions()
    {
        List<string> supplied = supplier != null ? supplier() : new List<string>();
        List<Dropdown.OptionData> optionData = new List<Dropdown.OptionData>(supplied.Count);

        dropdown.ClearOptions();
        supplied.ForEach(option => optionData.Add(new Dropdown.OptionData(option)));
        dropdown.AddOptions(optionData);
    }

    public string Current {
        get => dropdown.options.Count > 0 ? dropdown.options[dropdown.value].text : "";
        set => dropdown.value = dropdown.options.FindIndex(opt => opt.text.Equals(value));
    }
}
