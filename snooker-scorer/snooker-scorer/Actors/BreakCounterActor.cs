using Akka.Actor;
using snooker_scorer.Messages;

namespace snooker_scorer.Actors
{
    public partial class BreakCounterActor : ReceiveActor
    {
        private int _score;

        public BreakCounterActor()
        {
            Receive<ScoringShot>(msg => HandleScoringShot(msg));
            Receive<CurrentBreakRequest>(msg => HandleCurrentBreakRequest(msg));
            Receive<EndOfBreak>(msg => HandleEndOfBreak(msg));
        }

        private void HandleEndOfBreak(EndOfBreak msg)
        {
            _score = 0;
        }

        private void HandleCurrentBreakRequest(CurrentBreakRequest msg)
        {
            Sender.Tell(new CurrentBreak(_score));
        }

        private void HandleScoringShot(ScoringShot msg)
        {
            _score += msg.Value;
            Sender.Tell(new CurrentBreak(_score));
        }
    }
}