using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Extensions;
using UnityEngine.UI;

public class BotController : MonoBehaviour
{

    [HideInInspector] public BotInfo info;
    private TeamInfo team;

    [HideInInspector] public float maxHealth;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public bool isDead;
    private float weight = 100f;

    private bool actionsEnabled;

    [HideInInspector] public List<PartController> parts = new List<PartController>();
    public readonly List<SensorPartController> sensors = new List<SensorPartController>();
    public BodyTypeController body;

    public readonly List<GunController> guns = new List<GunController>();

    private readonly List<Behavior> behaviors = new List<Behavior>();
    private readonly List<BehaviorExecutor> behaviorExecutors = new List<BehaviorExecutor>();
    private BehaviorExecutor activeBehavior;
    private Behavior activeDemoBehavior;
    private bool usesDemoBehaviors = false;

    public void LoadInfoForPreview(BotInfo botInfo)
    {
        info = botInfo;
        LoadParts();
    }

    public void LoadInfo(BotInfo botInfo, TeamInfo teamInfo)
    {
        team = teamInfo;
        info = botInfo;

        maxHealth = info.Equipment.Sum(part => part.Attributes.GetOrDefault("health", 0f)) +
                    info.BodyType.Attributes.GetOrDefault("health", 0f);
        weight = info.Equipment.Sum(part => part.Attributes.GetOrDefault("weight", 0f)) +
                 info.BodyType.Attributes.GetOrDefault("weight", 0f);

        LoadParts();

        usesDemoBehaviors = info.Behaviors.Count == 0;

        if (usesDemoBehaviors) AddDemoBehaviors();
        else LoadBehaviors();

        var rigidbodyComponent = gameObject.GetComponent<Rigidbody2D>();
        if (rigidbodyComponent != null) rigidbodyComponent.mass = weight / 100f;
    }

    private void AddDemoBehaviors()
    {
        var proxSensor = sensors.Find(part => part is ProximitySensor) as ProximitySensor;
        var visSensor = sensors.Find(part => part is VisionSensor) as VisionSensor;
        var wheels = parts.Find(part => part is WheelsController) as WheelsController;

        switch (team.UserID)
        {
            case "lp0":

                switch (info.ID)
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

                switch (info.ID)
                {
                    case 0:
                        behaviors.Add(new Behavior(this, proxSensor.OpponentIsInRange, () =>
                        {
                            for (var i = 0; i < guns.Count; i++)
                            {
                                var opponent = proxSensor.GetNthOpponent(i);
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
                                var opponent = proxSensor.GetNthOpponent(i);
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
        body = PartController.ControllerForPart(info.BodyType) as BodyTypeController;
        body.bot = this;
        body.gameObject.transform.parent = transform;
        body.Setup();
        body.Position();

        foreach (var partInfo in info.Equipment)
        {
            var partController = PartController.ControllerForPart(partInfo);
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

        body?.PositionWeapons();
    }

    private void LoadBehaviors()
    {
        for (var i = info.Behaviors.Count - 1; i >= 0; i--)
        {
            var executor = new BehaviorExecutor(this, info.Behaviors[i]);
            behaviorExecutors.Add(executor);
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
        if (usesDemoBehaviors)
        {
            activeDemoBehavior = behaviors.FirstOrDefault(behavior => behavior.IsApplicable());
        }
        else
        {
            activeBehavior = behaviorExecutors.FirstOrDefault(behavior => behavior.IsApplicable());
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

        if (usesDemoBehaviors) activeDemoBehavior?.Execute();
        else activeBehavior?.Execute();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0 && !isDead) Die();
    }

    private void Die()
    {
        actionsEnabled = false;

        currentHealth = 0;
        isDead = true;

        gameObject.transform.SetZ(10f);

        var renderer = GetComponent<SpriteRenderer>();
        var spriteColor = renderer.color;
        spriteColor.a = 0.2f;
        renderer.color = spriteColor;

        body.Dim();
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
        var bodyObj = CopySprite(body.gameObject, container.transform);
        bodyObj.transform.CopyLocal(body.transform);
        children.Add(bodyObj);

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

    private static GameObject CopySprite(GameObject obj, Transform parent)
    {
        var sprite = obj.GetComponent<SpriteRenderer>().sprite;

        var newObj = new GameObject();
        newObj.transform.parent = parent;
        var img = newObj.AddComponent<Image>();
        img.sprite = sprite;

        img.rectTransform.sizeDelta = sprite.rect.size / 100f;

        return newObj;
    }

    public List<BotController> TargetableBots()
    {
        return new List<BotController>(); // TODO: Implement way of tracking which bots are "visible" to this bot's sensors
    }

}