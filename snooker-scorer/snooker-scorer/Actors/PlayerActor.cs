namespace snooker_scorer.Actors
{
    using System;
    using Akka.Actor;
    using Akka.Event;

    public partial class PlayerActor : ReceiveActor
    {
        private readonly IActorRef _foulCounter;
        private readonly Guid _id;
        private readonly ILoggingAdapter _log = Context.GetLogger();

        private readonly string _name;
        private readonly int _playerNumber;
        private int _score;

        public PlayerActor(string name, int playerNumber)
        {
            _log.Debug("PlayerActor ctor");
            _id = Guid.NewGuid();
            _name = name;
            _playerNumber = playerNumber;

            _foulCounter = Context.ActorOf(FoulCounterActor.Props());

            Receive<StatusRequest>(msg => HandleStatusRequest());
            Receive<ShotTakenCommand>(msg => HandleShotTakenCommand(msg));
            Receive<AwardFoulPointsCommand>(msg => HandleFoulPointsAwardedCommand(msg));
            Receive<FoulCommittedCommand>(msg => HandleFoulCommittedCommand(msg));
        }

        private void HandleFoulCommittedCommand(FoulCommittedCommand msg)
        {
            _log.Debug("HandleFoulCommittedCommand");
            _foulCounter.Tell(new FoulCounterActor.Foul(msg.Value));
        }

        private void HandleFoulPointsAwardedCommand(AwardFoulPointsCommand msg)
        {
            _log.Debug("HandleFoulPointsAwardedCommand");
            _score += msg.Value;
        }

        private void HandleShotTakenCommand(ShotTakenCommand msg)
        {
            _log.Debug("HandleShotTakenCommand");
            _score += msg.Score;
        }

        private void HandleStatusRequest()
        {
            _log.Debug("HandleStatusRequest");

            var foulResponse = _foulCounter.Ask(new FoulCounterActor.FoulCountRequest()).Result as FoulCounterActor.FoulCountResponse;
            Sender.Tell(new Status(_id, _name, _playerNumber, _score, new Status.FoulCount(foulResponse.NumberOfFouls, foulResponse.TotalPoints)));
        }

        public static Props Props(string name, int playerNumber)
        {
            return Akka.Actor.Props.Create(() => new PlayerActor(name, playerNumber));
        }
    }
}