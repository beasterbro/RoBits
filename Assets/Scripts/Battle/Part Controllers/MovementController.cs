using UnityEngine;

public class MovementController : PartController
{

    protected Rigidbody2D botBody;
    public float movementSpeed = 0.4f;

    public override void Position()
    {
        gameObject.transform.localPosition = new Vector3(0f, 0f, 2f);
    }

    public override void Setup()
    {
        botBody = bot.GetComponent<Rigidbody2D>();
    }

}