namespace snooker_scorer_test.Actors
{
    using Akka.Actor;

    public class ForwarderActor : UntypedActor
    {
        private readonly IActorRef _target;

        public ForwarderActor(IActorRef target)
        {
            _target = target;
        }

        protected override void OnReceive(object msg)
        {
            _target.Forward(msg);
        }
    }
}