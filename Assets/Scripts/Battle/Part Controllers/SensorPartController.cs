using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

public abstract class SensorPartController : PartController
{

    protected struct Opponent
    {

        public readonly BotController bot;
        public readonly float dist;

        public Opponent(BotController self, BotController opponent)
        {
            bot = opponent;
            dist = Vector2.Distance(self.gameObject.transform.position, opponent.gameObject.transform.position);
        }

    }

    protected float range;

    public override void Setup()
    {
        base.Setup();
        range = info.Attributes.GetOrDefault("range", 0f);
    }

    public abstract void MakeObservations();

    protected bool BotIsWithinRange(BotController bot)
    {
        return new Opponent(this.bot, bot).dist <= range;
    }

    protected List<Opponent> OpponentsWithinRange()
    {
        return OpponentsWithinRange(range);
    }

    protected List<Opponent> OpponentsWithinRange(float maxRange)
    {
        var otherGuys = new List<BotController>(BattleController.GetShared().GetAllBots());
        otherGuys.RemoveAll(other => other.isDead || !bot.OtherIsEnemey(other));

        var opponents = new List<Opponent>(otherGuys.Select(other => new Opponent(bot, other)));
        opponents.RemoveAll(other => other.dist > maxRange);
        return opponents;
    }

    public List<BotController> TargetableOpponents()
    {
        return new List<BotController>(OpponentsWithinRange().Select(opponent => opponent.bot));
    }

}