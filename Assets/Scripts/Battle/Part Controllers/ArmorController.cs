using UnityEngine;

public abstract class ArmorController : PartController
{

    public override void Position()
    {
        gameObject.transform.localPosition = new Vector3(0f, 0f, 1f);
    }

    public abstract void TakeDamage(float amount);

}