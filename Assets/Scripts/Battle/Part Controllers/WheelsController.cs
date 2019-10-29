using UnityEngine;
using Extensions;

public class WheelsController : MovementController
{

    public float turningSpeed = 50f;

    private float turningValue;
    private float forwardValue;

    public void SetForward(float forward)
    {
        forwardValue = forward;
    }

    public void SetTurning(float turning)
    {
        turningValue = turning;
    }

    private void FixedUpdate()
    {
        if (!bot.IsEnabled()) return;

        botBody.MoveRotation(botBody.rotation - (turningValue * turningSpeed * Time.deltaTime));

        var dm = forwardValue * movementSpeed * Time.deltaTime;
        var angle = botBody.rotation;
        botBody.MovePosition(botBody.position + VectorHelper.MakeVector(dm, angle));
    }

}