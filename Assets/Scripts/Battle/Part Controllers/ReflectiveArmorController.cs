using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReflectiveArmorController : ArmorController
{
    
    public float rechargeTime = 3f;
    private bool canReflect = true;

    public override void Setup()
    {
        base.Setup();
        canReflect = true;
    }

    public override void CollideWith(Bullet bullet)
    {
        if (canReflect) bullet.Rebound();
        base.CollideWith(bullet);
    }

    public override void TakeDamage(float amount, Bullet source)
    {
        currentHealth -= amount;

        if (currentHealth <= 0 && canReflect)
        {
            Dim();
            canReflect = false;
            StartCoroutine(Recharge());
        }
        else if (!canReflect)
        {
            DamageBot(amount);
        }
    }

    public IEnumerator Recharge()
    {
        yield return new WaitForSeconds(rechargeTime);
        if (!bot.isDead)
        {
            currentHealth = protection;
            canReflect = true;

            Undim();
        }
    }

    public bool CanReflect()
    {
        return canReflect;
    }

}