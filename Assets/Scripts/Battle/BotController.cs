using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotController : MonoBehaviour
{

    private TeamInfo team;

    public float maxHealth = 100.0f;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public bool isDead;

    private bool actionsEnabled;

    [HideInInspector] public BotInfo info;

    [HideInInspector] public List<PartController> parts = new List<PartController>();
    private readonly List<SensorPartController> sensors = new List<SensorPartController>();

    [HideInInspector] public Transform target;

    private readonly List<Behavior> behaviors = new List<Behavior>();

    private Behavior activeBehavior;

    public void LoadInfo(BotInfo botInfo)
    {
        info = botInfo;
        LoadParts();
    }

    public void LoadInfo(BotInfo botInfo, TeamInfo teamInfo)
    {
        team = teamInfo;
        LoadInfo(botInfo);

        List<GunController> guns = new List<GunController>();
        parts.ForEach(part =>
        {
            if (part is GunController gun) guns.Add(gun);
        });

        ProximitySensor proxSensor = sensors.Find(part => part is ProximitySensor) as ProximitySensor;
        WheelsController wheels = parts.Find(part => part is WheelsController) as WheelsController;

        if (teamInfo.GetUserID() == "lmp122")
        {
            if (botInfo.GetID() < 2)
            {
                behaviors.Add(new Behavior(this, proxSensor.OpponentIsInRange, () =>
                {
                    for (var i = 0; i < guns.Count; i++)    
                    {
                        BotController opponent = proxSensor.GetNthOpponent(i);
                        if (opponent != null)
                        {
                            guns[i].FocusOn(opponent.gameObject.transform);
                            guns[i].Fire();
                        }
                    }

                    if (!proxSensor.ShouldMoveTowardsNearestOpponent())
                    {
                        wheels.SetForward(0f);
                    }
                }));
            }
        }
        else if (teamInfo.GetUserID() == "axs1477")
        {
            if (botInfo.GetID() == 0)
            {
                proxSensor.maxRange = 10;

                behaviors.Add(new Behavior(this, proxSensor.OpponentIsInRange, () =>
                {
                    for (var i = 0; i < guns.Count; i++)
                    {
                        BotController opponent = proxSensor.GetNthOpponent(i);
                        if (opponent != null)
                        {
                            guns[i].FocusOn(opponent.gameObject.transform);
                            guns[i].Fire();
                        }
                    }
                }));
            }
            else if (botInfo.GetID() == 1)
            {
                behaviors.Add(new Behavior(this, proxSensor.OpponentIsInRange, () =>
                {
                    for (var i = 0; i < guns.Count; i++)
                    {
                        BotController opponent = proxSensor.GetNthOpponent(i);
                        if (opponent != null)
                        {
                            guns[i].FocusOn(opponent.gameObject.transform);
                            guns[i].Fire();
                        }
                    }

                    if (!proxSensor.ShouldMoveTowardsNearestOpponent())
                    {
                        wheels.SetForward(0f);
                    }
                }));
            }
        }

        if (wheels != null)
        {
            behaviors.Add(Behavior.BasicOffense(this, wheels));
        }
    }

    private void LoadParts()
    {
        List<GunController> guns = new List<GunController>();

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

                if (partController is SensorPartController sensor) sensors.Add(sensor);
                else if (partController is GunController gun) guns.Add(gun);
            }
        }

        if (guns.Count > 1)
        {
            float distance = 0.42f;
            float rotationInterval = 360f / guns.Count;
            float startingRotation = rotationInterval / 2f;

            for (var i = 0; i < guns.Count; i++)
            {
                float rotation = startingRotation + (i * rotationInterval);
                GameObject empty = new GameObject();
                empty.transform.parent = gameObject.transform;
                empty.transform.localPosition =
                    new Vector3(Mathf.Cos(rotation * Mathf.PI / 180f) * distance,
                        Mathf.Sin(rotation * Mathf.PI / 180f) * distance, -1f);
                empty.transform.localRotation = Quaternion.Euler(0f, 0f, rotation);

                guns[i].Position(empty.transform);
                Destroy(empty);
            }
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void MakeObservations()
    {
        foreach (var sensor in sensors)
            sensor.MakeObservations();
    }

    private void PickActiveBehavior()
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

    private void Update()
    {
        if (!actionsEnabled) return;

        activeBehavior = null;
        MakeObservations();
        PickActiveBehavior();
    }

    private void FixedUpdate()
    {
        if (!actionsEnabled) return;

        activeBehavior?.Execute();
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
        actionsEnabled = false;

        currentHealth = 0;
        isDead = true;

        Vector3 newPosition = gameObject.transform.position;
        newPosition.z = 10;
        gameObject.transform.position = newPosition;

        // Dim sprite
        var renderer = GetComponent<SpriteRenderer>();
        var spriteColor = renderer.color;
        spriteColor.a = 0.2f;
        renderer.color = spriteColor;

        parts.ForEach(part => part.Dim());
    }

    public bool OtherIsEnemey(BotController other)
    {
        return !other.team.Equals(team);
    }

    public void SetEnabled(bool enabled)
    {
        actionsEnabled = enabled;
    }

    public bool IsEnabled()
    {
        return actionsEnabled;
    }

    public GameObject BuildPreview()
    {
        GameObject bot = Instantiate(Resources.Load<GameObject>("Battle/Images/BasicBot"));

        foreach (PartController part in parts)
        {
            try
            {
                GameObject partObj =
                    Instantiate(Resources.Load<GameObject>("Battle/Images/" + part.info.GetResourceName()),
                        bot.transform);

                partObj.transform.localPosition = part.transform.localPosition;
                partObj.transform.localRotation = part.transform.localRotation;
                partObj.transform.localScale = part.transform.localScale;
            }
            catch (ArgumentException exc) { }
        }

        return bot;
    }

}