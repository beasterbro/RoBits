using System;
using UnityEngine;

public class GunController : ActorPartController
{
    public Rigidbody2D bullet;
    private Transform weaponLocation;
    private Transform fireTransform;
    private float launchForce = 5f;

    private void Start()
    {
//        if (info != null & info.GetAttributes().ContainsKey("launchForce"))
//        {
//            launchForce = (float) info.GetAttributes()["launchForce"];
//        }
    }

    public void Setup(Transform weaponLocation)
    {
        transform.SetPositionAndRotation(weaponLocation.position, weaponLocation.rotation);
        GameObject fireObject = new GameObject();
        fireObject.transform.parent = gameObject.transform;
        fireObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        fireObject.transform.localPosition = new Vector3(0.5f, 0f, 0f);
        fireObject.transform.localRotation = Quaternion.identity;
        
        fireTransform = fireObject.transform;
    }

    public override bool IsActor()
    {
        return true;
    }

    public override void Act()
    {
        Rigidbody2D bulletInstance =
            Instantiate(bullet, fireTransform.position, fireTransform.rotation);

        float vx = (float) Math.Cos(bulletInstance.rotation * Math.PI / 180) * launchForce;
        float vy = (float) Math.Sin(bulletInstance.rotation * Math.PI / 180) * launchForce;

        bulletInstance.velocity = new Vector2(vx, vy);
        Destroy(bulletInstance.gameObject, 2f);
    }
    
}