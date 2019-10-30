using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Extensions;
using UnityEngine.UI;

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

    public void LoadInfo(BotInfo botInfo, TeamInfo teamInfo)
    {
        team = teamInfo;
        info = botInfo;
        LoadParts();

        var guns = new List<GunController>();
        parts.ForEach(part =>
        {
            if (part is GunController gun) guns.Add(gun);
        });

        var proxSensor = sensors.Find(part => part is ProximitySensor) as ProximitySensor;
        var visSensor = sensors.Find(part => part is VisionSensor) as VisionSensor;
        var wheels = parts.Find(part => part is WheelsController) as WheelsController;

        switch (teamInfo.GetUserID())
        {
            case "lmp122":

                switch (botInfo.GetID())
                {
                    case 0:
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

                        break;

                    case 1:
                        behaviors.Add(new Behavior(this, proxSensor.OpponentIsInRange, () =>
                        {
                            for (var i = 0; i < guns.Count; i++)
                            {
                                BotController opponent = proxSensor.GetNthOpponent(i);
                                if (opponent != null)
                                {
                                    bool shouldHoldFire = visSensor.BotHasPartOfType(opponent, "Reflective Armor") &&
                                                          visSensor.PartsOnBotOfType<ReflectiveArmorController>(
                                                              opponent,
                                                              "Reflective Armor")[0].CanReflect();

                                    guns[i].FocusOn(opponent.gameObject.transform);
                                    if (!shouldHoldFire) guns[i].Fire();
                                }
                            }

                            if (!proxSensor.ShouldMoveTowardsNearestOpponent())
                            {
                                wheels.SetForward(0f);
                            }
                        }));
                        break;
                }

                break;

            case "ax1477":

                switch (botInfo.GetID())
                {
                    case 0:
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
                        break;

                    case 1:
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
                        break;
                }

                break;
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

        PositionWeapons();
    }

    private void PositionWeapons()
    {
        var guns = parts.Where(part => part is GunController).Cast<GunController>().ToArray();

        if (guns.Length > 1)
        {
            const float distance = 0.42f;
            var rotationInterval = 360f / guns.Length;
            var startingRotation = rotationInterval / 2f;

            for (var i = 0; i < guns.Length; i++)
            {
                var rotation = startingRotation - (i * rotationInterval);
                var empty = new GameObject();
                empty.transform.parent = gameObject.transform;
                empty.transform.localPosition = VectorHelper.MakeVector(distance, rotation, -1f);
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

        gameObject.transform.SetZ(10f);

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
        var children = new List<GameObject>();

        var container = new GameObject();
        children.Add(CopySprite(gameObject, container.transform));

        foreach (var part in parts)
        {
            if (part.HasSprite())
            {
                var partObj = CopySprite(part.gameObject, container.transform);
                partObj.transform.CopyLocal(part.transform);
                children.Add(partObj);
            }
        }

        children.Sort((a, b) => a.transform.localPosition.z.CompareTo(b.transform.localPosition.z));
        children.ForEach(obj => obj.transform.SetAsFirstSibling());

        return container;
    }

    private GameObject CopySprite(GameObject obj, Transform parent)
    {
        var sprite = obj.GetComponent<SpriteRenderer>().sprite;

        var newObj = new GameObject();
        newObj.transform.parent = parent;
        var img = newObj.AddComponent<Image>();
        img.sprite = sprite;

        img.rectTransform.sizeDelta = sprite.rect.size / 100f;

        return newObj;
    }

}