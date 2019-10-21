﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour, Target
{
    private float maxHealth = 100.0f;
    private float minHealth = 0.0f;

    public Rigidbody2D bullet;
    public Transform bulletTransform;
    public float launchForce;

    public GunLocation[] weaponLocations;

    public float movementSpeed;
    public float turningSpeed;

    [HideInInspector] public BotInfo info;

    [HideInInspector] public float currentHealth;

    /*[HideInInspector]*/
    public PartController[] parts;

    [HideInInspector] public Target target;
    // actions
    // triggers

    private Rigidbody2D rigidbody;
    private float movementValue = 0f;
    private float turningValue = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        int numWeapons = 0;
        foreach (PartController controller in parts)
        {
            if (numWeapons == weaponLocations.Length) break;
            if (controller is WeaponController)
            {
                (controller as WeaponController).transform.SetPositionAndRotation(
                    weaponLocations[numWeapons].weaponLocation.position,
                    weaponLocations[numWeapons].weaponLocation.rotation);
                numWeapons++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }

        movementValue = Input.GetAxis("Vertical");
        turningValue = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        float turn = turningValue * turningSpeed * Time.deltaTime;
        rigidbody.MoveRotation(rigidbody.rotation - turn);

        float movementDistance = movementValue * movementSpeed * Time.deltaTime;
        float movementX = (float) Math.Cos(rigidbody.rotation * Math.PI / 180) * movementDistance;
        float movementY = (float) Math.Sin(rigidbody.rotation * Math.PI / 180) * movementDistance;

        Vector2 movement = new Vector2(movementX, movementY);
        rigidbody.MovePosition(rigidbody.position + movement);
    }

    void Fire()
    {
        Rigidbody2D bulletInstance =
            Instantiate(bullet, bulletTransform.position, bulletTransform.rotation) as Rigidbody2D;

        float vx = (float) Math.Cos(bulletInstance.rotation * Math.PI / 180) * launchForce;
        float vy = (float) Math.Sin(bulletInstance.rotation * Math.PI / 180) * launchForce;

        bulletInstance.velocity = new Vector2(vx, vy);
        Destroy(bulletInstance.gameObject, 2f);
    }

    public void SetTarget(GameObject target)
    {
        // TODO: Implement
    }

    public void TakeDamage(float amount)
    {
        // TODO: Implement  
    }

    // PerformAction

    // OnTrigger
}