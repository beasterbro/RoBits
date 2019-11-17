using System.Linq;
using Extensions;
using UnityEngine;

public class BodyTypeController : PartController
{

    public virtual void PositionWeapons()
    {
        var guns = bot.parts.Where(part => part is GunController).Cast<GunController>().ToList();

        if (guns.Count > 1)
        {
            const float distance = 0.42f;
            var rotationInterval = 360f / guns.Count;
            var startingRotation = rotationInterval / 2f;

            for (var i = 0; i < guns.Count; i++)
            {
                var rotation = startingRotation - (i * rotationInterval);
                var empty = new GameObject();
                empty.transform.parent = bot.gameObject.transform;
                empty.transform.localPosition = VectorHelper.MakeVector(distance, rotation, -1f);
                empty.transform.localRotation = Quaternion.Euler(0f, 0f, rotation);

                guns[i].Position(empty.transform);
                Destroy(empty);
            }
        }
    }

}