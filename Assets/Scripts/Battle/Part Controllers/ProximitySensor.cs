using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProximitySensor : SensorPartController
{

    private struct Opponent
    {

        public BotController bot;
        public float dist;

        public Opponent(BotController self, BotController opponent)
        {
            bot = opponent;
            dist = Vector2.Distance(self.gameObject.transform.position, opponent.gameObject.transform.position);
        }

    }

    public float maxRange = 6f;
    public float minRange = 4f;

    private List<BotController> opponentsByDistance = new List<BotController>();

    private BotController nearestOpponent;

    public override void MakeObservations()
    {
        var otherGuys = new List<BotController>(BattleController.GetShared().GetAllBots());
        otherGuys.RemoveAll(other => other.isDead || !bot.OtherIsEnemey(other));

        var opponents =
            new List<Opponent>(otherGuys.Select(other => new Opponent(bot, other)));
        opponents.RemoveAll(other => other.dist > maxRange);
        opponents.Sort((a, b) => a.dist.CompareTo(b.dist));
        opponentsByDistance.Clear();
        opponentsByDistance.AddRange(opponents.Select(opponent => opponent.bot));

        if (opponents.Count > 0) nearestOpponent = opponentsByDistance[0];
        else nearestOpponent = null;
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

        float distance = Vector3.Distance(bot.gameObject.transform.position,
            nearestOpponent.gameObject.transform.position);

        return distance > minRange;
    }

    public bool OpponentIsInRange()
    {
        return nearestOpponent != null;
    }

}