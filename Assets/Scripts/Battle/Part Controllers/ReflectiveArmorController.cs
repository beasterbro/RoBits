using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReflectiveArmorController : ArmorController
{

    public float maxEnergy = 30f;
    private float currentEnergy;

    public float rechargeTime = 3f;
    private bool canReflect = true;

    public override void Setup()
    {
        currentEnergy = maxEnergy;
        canReflect = true;
    }

    public override void CollideWith(Bullet bullet)
    {
        if (canReflect) bullet.Rebound();
        base.CollideWith(bullet);
    }

    public override void TakeDamage(float amount, Bullet source)
    {
        currentEnergy -= amount;

        if (currentEnergy <= 0 && canReflect)
        {
            Dim();
            canReflect = false;
            StartCoroutine(Recharge());
        }
        else if (!canReflect)
        {
            base.TakeDamage(amount, source);
        }
    }

    public IEnumerator Recharge()
    {
        yield return new WaitForSeconds(rechargeTime);
        if (!bot.isDead)
        {
            currentEnergy = maxEnergy;
            canReflect = true;

            Undim();
        }
    }

    public bool CanReflect()
    {
        return canReflect;
    }

}