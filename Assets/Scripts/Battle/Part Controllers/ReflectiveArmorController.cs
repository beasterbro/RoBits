using System;
using System.Collections;
using UnityEngine;

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

    public override void TakeDamage(float amount)
    {
        currentEnergy -= amount;

        if (currentEnergy <= 0 && canReflect)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Color spriteColor = renderer.color;
            spriteColor.a = 0.2f;
            renderer.color = spriteColor;

            canReflect = false;
            StartCoroutine(Recharge());
        }
    }

    public IEnumerator Recharge()
    {
        yield return new WaitForSeconds(rechargeTime);
        if (!bot.isDead)
        {
            currentEnergy = maxEnergy;
            canReflect = true;

            // Dim sprite
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Color spriteColor = renderer.color;
            spriteColor.a = 1f;
            renderer.color = spriteColor;
        }
    }

    public bool CanReflect()
    {
        return canReflect;
    }

}