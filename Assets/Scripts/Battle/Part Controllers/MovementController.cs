using Extensions;
using UnityEngine;

public class MovementController : PartController
{

    protected Rigidbody2D botBody;
    public float movementSpeed;
    public float handling;

    public override void Setup()
    {
        botBody = bot.GetComponent<Rigidbody2D>();
        movementSpeed = info.Attributes.GetOrDefault("speed", 0f);
        handling = info.Attributes.GetOrDefault("handling", 0f);
    }

    public override void Position()
    {
        gameObject.transform.localPosition = new Vector3(0f, 0f, 2f);
    }

}