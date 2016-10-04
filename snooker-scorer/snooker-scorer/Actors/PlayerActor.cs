namespace snooker_scorer.Actors
{
    using Akka.Actor;
    using Akka.Event;
    using System;

    public partial class PlayerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();
        private readonly IActorRef _foulCounter;

        private readonly Guid _id;
        private readonly string _name;
        private readonly int _playerNumber;
        private int _score;

        public PlayerActor(Guid id, string name, int playerNumber, Props foulCounterProps)
        {
            _log.Debug("PlayerActor ctor");
            _id = id;
            _name = name;
            _playerNumber = playerNumber;

            _foulCounter = Context.ActorOf(foulCounterProps, $"FoulCounter:{Guid.NewGuid()}");

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

            var senderClosure = Sender;

            _foulCounter.Ask<FoulCounterActor.FoulCountResponse>(new FoulCounterActor.FoulCountRequest())
                .ContinueWith(tr =>
                {
                    return new Status(_id,
                        _name,
                        _playerNumber,
                        _score,
                        new Status.FoulCount(tr.Result.NumberOfFouls,
                            tr.Result.TotalPoints));
                }).PipeTo(senderClosure);
        }

        public static Props Props(Guid id, string name, int playerNumber)
        {
            return Akka.Actor.Props.Create(() => new PlayerActor(id, name, playerNumber, FoulCounterActor.Props()));
        }
    }
}