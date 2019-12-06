using System.Collections.Generic;
using UnityEngine;

public class ListController : MonoBehaviour
{

    public ListItem headerPrefab;
    public ListItem itemPrefab;
    private readonly List<ListItem> items = new List<ListItem>();
    private bool hasHeadings;
    private readonly List<object> listData = new List<object>();
    private readonly Dictionary<object, List<object>> dictData = new Dictionary<object, List<object>>();

    public void LoadData(IEnumerable<object> data)
    {
        hasHeadings = false;
        dictData.Clear();
        listData.Clear();
        listData.AddRange(data);
    }

    public void LoadData(Dictionary<object, IEnumerable<object>> data)
    {
        hasHeadings = true;
        listData.Clear();
        dictData.Clear();

        foreach (var key in data.Keys)
            dictData[key] = new List<object>(data[key]);
    }

    public void GenerateItems()
    {
        items.ForEach(item => Destroy(item.gameObject));
        items.Clear();

        if (hasHeadings)
        {
            foreach (var group in dictData)
            {
                AddNewItem(headerPrefab, group.Key);

                foreach (var el in group.Value)
                {
                    AddNewItem(itemPrefab, el);
                }
            }
        }
        else
        {
            foreach (var el in listData)
            {
                AddNewItem(itemPrefab, el);
            }
        }
    }

    private void AddNewItem(ListItem prefab, object element)
    {
        var item = Instantiate(prefab, gameObject.transform, false);
        item.LoadData(element);
        items.Add(item);
    }

    public List<ListItem> Items => items;

}