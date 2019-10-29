using UnityEngine;

public class ArmorController : PartController
{

    public float protection = 1f;

    public override void Position()
    {
        gameObject.transform.localPosition = new Vector3(0f, 0f, 1f);
    }

    public virtual void CollideWith(Bullet bullet)
    {
        TakeDamage(bullet.damage, bullet);
    }

    public virtual void TakeDamage(float amount, Bullet source)
    {
        bot.TakeDamage(amount * (1f - protection));
    }

}