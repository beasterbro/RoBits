using System;

public class Behavior
{

    private BotController bot;
    private Func<bool> triggerFunction;
    private Action executionFunction;

    public Behavior(BotController bot, Func<bool> trigger, Action execute)
    {
        this.bot = bot;
        triggerFunction = trigger;
        executionFunction = execute;
    }

    public bool IsApplicable()
    {
        return triggerFunction.Invoke();
    }

    public void Execute()
    {
        executionFunction.Invoke();
    }

    public static Behavior BasicOffense(BotController bot)
    {
        return new Behavior(bot, () => true, () => { bot.SetMovementValue(0.3f); });
    }

}