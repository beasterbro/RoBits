using UnityEngine;

public abstract class PartController : MonoBehaviour
{

    [HideInInspector] public PartInfo info;

    public abstract bool IsActor();

}
