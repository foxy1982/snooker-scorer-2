using Akka.Actor;
using System;

namespace snooker_scorer_test.Actors
{
    public class ForwarderActor : UntypedActor
    {
        private readonly IActorRef target;

        public ForwarderActor(IActorRef target)
        {
            this.target = target;
        }

        protected override void OnReceive(Object msg)
        {
            target.Forward(msg);
        }
    }
}