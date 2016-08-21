using Akka.Actor;
using snooker_scorer.Messages;

namespace snooker_scorer.Actors
{
    public class BreakCounterActor : ReceiveActor
    {
        public class CurrentBreak
        {
            public readonly int Value;

            public CurrentBreak(int value)
            {
                Value = value;
            }
        }

        public BreakCounterActor()
        {
            Receive<ScoringShot>(msg => HandleScoringShot(msg));
        }

        private void HandleScoringShot(ScoringShot msg)
        {
            Sender.Tell(new CurrentBreak(msg.Value));
        }
    }
}