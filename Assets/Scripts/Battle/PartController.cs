using UnityEngine;

public abstract class PartController : MonoBehaviour
{
    [HideInInspector] public PartInfo info;

    [HideInInspector] public BotController bot;
    public abstract bool IsActor();

    public static PartController ControllerForPart(PartInfo info)
    {
        if (info.GetName() == "Basic Gun")
        {
            GameObject obj = Instantiate(Resources.Load("Battle/BasicGun")) as GameObject;
            return obj.GetComponent<GunController>();
        }

        return null;
    }
}