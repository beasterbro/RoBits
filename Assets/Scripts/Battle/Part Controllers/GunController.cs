using System.Collections;
using UnityEngine;
using Extensions;

public class GunController : PartController
{

    public Transform fireTransform;

    public Rigidbody2D bullet;
    private Transform weaponLocation;
    public float launchSpeed;
    public float downtime;

    private bool canFire = true;

    public override void Setup()
    {
        base.Setup();
        launchSpeed = info.Attributes.GetOrDefault("launchSpeed", 5f);
        downtime = info.Attributes.GetOrDefault("downtime", 2f);
    }

    public override void Position()
    {
        base.Position();
        transform.SetZ(-1f);
    }

    public void Position(Transform location)
    {
        weaponLocation = location;
        transform.CopyPositionAndRotation(weaponLocation);
    }

    public void Reset()
    {
        if (weaponLocation != null)
        {
            transform.CopyPositionAndRotation(weaponLocation);
        }
    }

    public void Fire()
    {
        if (!canFire) return;

        StartCoroutine(DelayNextShot());

        var bulletInstance =
            Instantiate(bullet, fireTransform.position, fireTransform.rotation);

        Bullet bc = bulletInstance.GetComponent<Bullet>();
        if (bc != null) bc.firedBy = bot;

        bulletInstance.velocity = VectorHelper.MakeVector(launchSpeed, bulletInstance.rotation);
        Destroy(bulletInstance.gameObject, 2f);
    }

    public void FocusOn(Transform target)
    {
        Vector2 diff = target.position - transform.position;
        var angle = 180f * Mathf.Atan(Mathf.Abs(diff.y / diff.x)) / Mathf.PI;

        if (diff.x == 0f) angle = 90f * Mathf.Sign(diff.y);
        else if (diff.y == 0f && diff.x < 0f) angle = 180f;
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