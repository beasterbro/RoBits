using Extensions;
using UnityEngine;

public class ArmorController : PartController
{

    public float protection;
    public float durability;

    protected float currentHealth;

    public override void Setup()
    {
        base.Setup();
        protection = info.Attributes.GetOrDefault("protection", 1f);
        durability = info.Attributes.GetOrDefault("durability", 0f);
        currentHealth = durability;
    }

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
        currentHealth -= amount;

        if (currentHealth < 0f)
        {
            DamageBot(amount / 2);
        }
        else
        {
            DamageBot(amount);
        }
    }

    protected void DamageBot(float amount)
    {
        bot.TakeDamage(amount * (1f - protection));
    }

}