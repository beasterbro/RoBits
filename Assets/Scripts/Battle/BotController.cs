using System;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [HideInInspector] private TeamInfo team;

    // TODO: Reorganize all these nonsense fields & methods
    public float MAX_HEALTH = 100.0f;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public bool isDead = false;

    public bool controlsEnabled;

    public Transform[] weaponLocations;

    public float movementSpeed;
    public float turningSpeed;

    [HideInInspector] public BotInfo info;

    [HideInInspector] public List<PartController> parts = new List<PartController>();
    private List<SensorPartController> sensors = new List<SensorPartController>();
    private List<ActorPartController> actors = new List<ActorPartController>();

    [HideInInspector] public Transform target;

    private List<Behavior> behaviors = new List<Behavior>();
    // actions
    // triggers

    private Rigidbody2D rigidbody;
    private float movementValue = 0f;
    private float turningValue = 0f;

    private Behavior activeBehavior;

    public void LoadInfo(BotInfo botInfo, TeamInfo teamInfo)
    {
        team = teamInfo;
        info = botInfo;

        LoadParts();

        if (parts.Count > 1)
        {
            ProximitySensor proxSensor = sensors[0] as ProximitySensor;
            GunController gun = actors[0] as GunController;

            behaviors.Add(new Behavior(this, proxSensor.OpponentIsInRange, () =>
            {
                gun.FocusOn(proxSensor.GetNearestOpponent().transform);
                gun.Act();

                if (!proxSensor.ShouldMoveTowardsNearestOpponent())
                {
                    movementValue = 0f;
                }
            }));

            behaviors.Add(Behavior.BasicOffense(this));
        }
    }

    private void LoadParts()
    {
        foreach (PartInfo partInfo in info.GetEquippedParts())
        {
            PartController partController = PartController.ControllerForPart(partInfo);
            if (partController != null)
            {
                partController.bot = this;
                partController.gameObject.transform.parent = transform;
                partController.Setup();
                partController.Position();
                parts.Add(partController);
            }
        }

        foreach (PartController part in parts)
        {
            if (part is SensorPartController sensor) sensors.Add(sensor);
            else if (part is ActorPartController actor) actors.Add(actor);
        }

        int numWeapons = 0;
        foreach (PartController controller in parts)
        {
            if (numWeapons == weaponLocations.Length) break;
            if (controller is GunController gun)
            {
                gun.Position(weaponLocations[numWeapons++]);
            }
        }
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentHealth = MAX_HEALTH;
    }

    void MakeObservations()
    {
        foreach (SensorPartController sensor in sensors)
            sensor.Observe();
    }

    void PickActiveBehavior()
    {
        foreach (Behavior behavior in behaviors)
        {
            if (behavior.IsApplicable())
            {
                activeBehavior = behavior;
                break;
            }
        }
    }

    void Update()
    {
        activeBehavior = null;
        MakeObservations();
        PickActiveBehavior();

        if (!controlsEnabled) return;

        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    private void FixedUpdate()
    {
        activeBehavior?.Execute();

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
        foreach (ActorPartController actor in actors)
        {
            actor.Act();
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        currentHealth = 0;
        isDead = true;

        // Dim sprite
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color spriteColor = renderer.color;
        spriteColor.a = 0.2f;
        renderer.color = spriteColor;
    }

    public void SetMovementValue(float amount)
    {
        movementValue = amount;
    }

    public bool OtherIsEnemey(BotController other)
    {
        return !other.team.Equals(team);
    }

    // PerformAction

    // OnTrigger
}