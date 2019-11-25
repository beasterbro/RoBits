using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{

    public object data;
    public Text title;

    public virtual void LoadData(object data)
    {
        this.data = data;
        if (data is string str) title.text = str;
    }

}