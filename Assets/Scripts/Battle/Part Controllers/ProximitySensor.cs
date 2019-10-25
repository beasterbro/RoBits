using System.Collections.Generic;
using UnityEngine;

public class ProximitySensor : SensorPartController
{
    public float maxRange = 6f;
    public float minRange = 4f;
    private BotController nearestOpponent;

    public override void Observe()
    {
        List<BotController> otherGuys = new List<BotController>(GameObject.FindObjectsOfType<BotController>());
        otherGuys.RemoveAll(other => other.isDead || !bot.OtherIsEnemey(other));

        BotController nearest = null;
        float minDist = maxRange;

        foreach (BotController opponent in otherGuys)
        {
            float dist = Vector3.Distance(bot.gameObject.transform.position,
                opponent.gameObject.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = opponent;
            }
        }

        nearestOpponent = nearest;
    }

    public BotController GetNearestOpponent()
    {
        return nearestOpponent;
    }

    public bool ShouldMoveTowardsNearestOpponent()
    {
        if (nearestOpponent == null) return false;

        float distance = Vector3.Distance(bot.gameObject.transform.position,
            nearestOpponent.gameObject.transform.position);

        return distance > minRange;
    }

    public bool OpponentIsInRange()
    {
        return nearestOpponent != null;
    }
}