using System;
using System.Collections;
using UnityEngine;

public class ReflectiveArmorController : ActorPartController
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

    public override void Position()
    {
        gameObject.transform.localPosition = Vector3.forward;
    }

    public override void Act() { }

    public void TakeDamange(float amount)
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