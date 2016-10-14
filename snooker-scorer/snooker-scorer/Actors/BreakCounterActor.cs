namespace snooker_scorer.Actors
{
    using Akka.Actor;
    using Messages;

    public partial class BreakCounterActor : ReceiveActor
    {
        private int _score;

        public BreakCounterActor()
        {
            Receive<ScoringShot>(msg => HandleScoringShot(msg));
            Receive<EndOfBreak>(msg => HandleEndOfBreak(msg));
            Receive<CurrentBreakRequest>(msg => HandleCurrentBreakRequest(msg));
        }

        private void HandleEndOfBreak(EndOfBreak msg)
        {
            _score = 0;
        }

        private void HandleCurrentBreakRequest(CurrentBreakRequest msg)
        {
            Sender.Tell(new CurrentBreakResponse(_score));
        }

        private void HandleScoringShot(ScoringShot msg)
        {
            _score += msg.Value;
        }
    }
}