using System;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class PartController : MonoBehaviour
{

    [HideInInspector] public BotController bot;
    [HideInInspector] public PartInfo info;

    public static PartController ControllerForPart(PartInfo info)
    {
        string stripped = Regex.Replace(info.GetName(), "[\\W]", "");

        try
        {
            GameObject obj = Instantiate(Resources.Load("Battle/" + stripped)) as GameObject;
            return obj.GetComponent<PartController>();
        }
        catch (ArgumentException exc)
        {
            Debug.LogWarning("Attempted to load missing asset: Battle/" + stripped);
        }

        return null;
    }

    public virtual void Setup() { }

    public virtual void Position()
    {
        gameObject.transform.localPosition = Vector3.zero;
    }

    public abstract bool IsActor();

}