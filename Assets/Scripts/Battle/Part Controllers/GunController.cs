using System;
using System.Collections;
using UnityEngine;

public class GunController : ActorPartController
{
    public Transform fireTransform;

    public Rigidbody2D bullet;
    private Transform weaponLocation;
    public float launchSpeed = 5f;
    public float downtime = 0.5f;

    private bool canFire = true;

    public override bool IsActor()
    {
        return true;
    }

    public void Position(Transform location)
    {
        weaponLocation = location;
        transform.SetPositionAndRotation(location.position, location.rotation);
    }

    public void Reset()
    {
        if (weaponLocation != null)
        {
            transform.SetPositionAndRotation(weaponLocation.position, weaponLocation.rotation);
        }
    }

    public override void Act()
    {
        if (!canFire) return;

        StartCoroutine(DelayNextShot());

        Rigidbody2D bulletInstance =
            Instantiate(bullet, fireTransform.position, fireTransform.rotation);

        Bullet bc = bulletInstance.GetComponent<Bullet>();
        if (bc != null) bc.firedBy = bot;

        float vx = (float) Math.Cos(bulletInstance.rotation * Math.PI / 180) * launchSpeed;
        float vy = (float) Math.Sin(bulletInstance.rotation * Math.PI / 180) * launchSpeed;

        bulletInstance.velocity = new Vector2(vx, vy);
        Destroy(bulletInstance.gameObject, 2f);
    }

    public void FocusOn(Transform target)
    {
        transform.right = target.position - transform.position;
    }

    private IEnumerator DelayNextShot()
    {
        canFire = false;
        yield return new WaitForSeconds(downtime);
        canFire = true;
    }
}