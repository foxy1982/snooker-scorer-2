namespace snooker_scorer.Actors
{
    using Akka.Actor;
    using Akka.Event;

    public partial class FoulCounterActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();

        private int _foulCount;
        private int _foulValueCount;

        public FoulCounterActor()
        {
            _log.Debug("ctor");
            Receive<FoulCountRequest>(msg => HandleFoulCountRequest());
            Receive<Foul>(msg => HandleFoul(msg));
        }

        private void HandleFoul(Foul msg)
        {
            _log.Debug("HandleFoul");
            _foulCount++;
            _foulValueCount += msg.Value;
        }

        private void HandleFoulCountRequest()
        {
            _log.Debug("HandleFoulCountRequest");
            Sender.Tell(new FoulCountResponse(_foulCount, _foulValueCount));
        }

        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new FoulCounterActor());
        }
    }
}