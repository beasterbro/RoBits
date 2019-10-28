using System;
using System.Collections;
using UnityEngine;

public class GunController : PartController
{

    public Transform fireTransform;

    public Rigidbody2D bullet;
    private Transform weaponLocation;
    public float launchSpeed = 5f;
    public float downtime = 0.5f;

    private bool canFire = true;

    public override void Position()
    {
        base.Position();
        Vector3 position = transform.position;
        position.z = -1f;
        transform.SetPositionAndRotation(position, transform.rotation);
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

    public void Fire()
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
        Vector2 diff = target.position - transform.position;
        float angle = 180f * Mathf.Atan(Mathf.Abs(diff.y / diff.x)) / Mathf.PI;

        if (diff.x == 0) angle = 90f * Mathf.Sign(diff.y);
        else if (diff.y == 0 && diff.x < 0) angle = 180;
        else
        {
            if (diff.x < 0) angle = 180 - angle;
            if (diff.y < 0) angle = -angle;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private IEnumerator DelayNextShot()
    {
        canFire = false;
        yield return new WaitForSeconds(downtime);
        canFire = true;
    }

}