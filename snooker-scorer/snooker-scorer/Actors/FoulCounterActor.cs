namespace snooker_scorer.Actors
{
    using Akka.Actor;

    public partial class FoulCounterActor : ReceiveActor
    {
        private int _foulCount;
        private int _foulValueCount;

        public FoulCounterActor()
        {
            Receive<FoulCountRequest>(msg => HandleFoulCountRequest());
            Receive<Foul>(msg => HandleFoul(msg));
        }

        private void HandleFoul(Foul msg)
        {
            _foulCount++;
            _foulValueCount += msg.Value;
        }

        private void HandleFoulCountRequest()
        {
            Sender.Tell(new FoulCountResponse(_foulCount, _foulValueCount));
        }

        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new FoulCounterActor());
        }
    }
}