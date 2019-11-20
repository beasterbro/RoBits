using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProximitySensor : SensorPartController
{

    public float minRange;

    private readonly List<BotController> opponentsByDistance = new List<BotController>();
    private BotController nearestOpponent;

    public override void Setup()
    {
        base.Setup();
        minRange = range / 2;
    }

    public override void MakeObservations()
    {
        var opponents = OpponentsWithinRange();
        opponents.Sort((a, b) => a.dist.CompareTo(b.dist));
        opponentsByDistance.Clear();
        opponentsByDistance.AddRange(opponents.Select(opponent => opponent.bot));

        nearestOpponent = opponents.Count > 0 ? opponentsByDistance[0] : null;
    }

    public BotController GetNearestOpponent()
    {
        return nearestOpponent;
    }

    public BotController GetNthOpponent(int n)
    {
        return opponentsByDistance.Count > n ? opponentsByDistance[n] : null;
    }

    public bool ShouldMoveTowardsNearestOpponent()
    {
        if (nearestOpponent == null) return false;

        var distance = Vector3.Distance(bot.gameObject.transform.position,
            nearestOpponent.gameObject.transform.position);

        return distance > minRange;
    }

    public bool OpponentIsInRange()
    {
        return nearestOpponent != null;
    }

}