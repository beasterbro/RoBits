public abstract class ActorPartController : PartController
{
    public override bool IsActor()
    {
        return true;
    }

    public abstract void Act();

}