using System;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class PartController : MonoBehaviour
{

    [HideInInspector] public BotController bot;
    [HideInInspector] public PartInfo info;
    private SpriteRenderer spriteRenderer;

    public static PartController ControllerForPart(PartInfo info)
    {
        string stripped = Regex.Replace(info.GetName(), "[\\W]", "");

        try
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Battle/" + stripped));
            PartController controller = obj.GetComponent<PartController>();
            controller.info = info;
            return controller;
        }
        catch (ArgumentException exc)
        {
            Debug.LogWarning("Attempted to load missing asset: Battle/" + stripped);
        }

        return null;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Setup() { }

    public virtual void Position()
    {
        gameObject.transform.parent = bot.transform;
        gameObject.transform.localPosition = Vector3.zero;
    }

    public void Undim()
    {
        if (spriteRenderer == null) return;
        var spriteColor = spriteRenderer.color;
        spriteColor.a = 1f;
        spriteRenderer.color = spriteColor;
    }

    public void Dim()
    {
        if (spriteRenderer == null) return;
        var spriteColor = spriteRenderer.color;
        spriteColor.a = 0.2f;
        spriteRenderer.color = spriteColor;
    }

    public bool HasSprite()
    {
        return GetComponent<SpriteRenderer>() != null;
    }

}