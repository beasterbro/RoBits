using System;
using UnityEngine;

public abstract class PartController : MonoBehaviour
{

    [HideInInspector] public BotController bot;
    [HideInInspector] public PartInfo info;
    private SpriteRenderer spriteRenderer;

    public static PartController ControllerForPart(PartInfo info)
    {
        try
        {
            var obj = Instantiate(Resources.Load<GameObject>("Battle/" + info.ResourceName));
            var controller = obj.GetComponent<PartController>();
            controller.info = info;
            return controller;
        }
        catch (ArgumentException)
        {
            Debug.LogWarning("Attempted to load missing asset: Battle/" + info.ResourceName);
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