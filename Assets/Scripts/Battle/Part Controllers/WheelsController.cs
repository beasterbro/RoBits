using UnityEngine;

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
        botBody.MoveRotation(botBody.rotation - (turningValue * turningSpeed * Time.deltaTime));

        float dm = forwardValue * movementSpeed * Time.deltaTime;
        float angle = botBody.rotation * Mathf.PI / 180;
        botBody.MovePosition(botBody.position + new Vector2(Mathf.Cos(angle) * dm, Mathf.Sin(angle) * dm));
    }

}