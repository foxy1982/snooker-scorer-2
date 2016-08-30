using Akka.Actor;
using Akka.Event;
using System;
using System.Threading.Tasks;

namespace snooker_scorer.Actors
{
    public partial class GameActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Logging.GetLogger(Context);

        private readonly Guid _id;
        private readonly IActorRef _player1;
        private readonly IActorRef _player2;
        private IActorRef _nextToPlay;

        public GameActor(Guid id, string player1Name, string player2Name)
        {
            _log.Debug("GameActor ctor");
            _id = id;
            _player1 = Context.ActorOf(PlayerActor.Props(player1Name));
            _player2 = Context.ActorOf(PlayerActor.Props(player2Name));
            _nextToPlay = _player1;

            Receive<StatusRequest>(msg => HandleStatusRequest(msg));
            Receive<ShotTakenCommand>(msg => HandleShotTakenCommand(msg));
        }

        private void HandleStatusRequest(StatusRequest msg)
        {
            var sender = Sender;

            var task = Task.Run(async () =>
            {
                var player1Task = _player1.Ask(new PlayerActor.StatusRequest());
                var player2Task = _player2.Ask(new PlayerActor.StatusRequest());

                await Task.WhenAll(player1Task, player2Task);

                var player1Info = player1Task.Result as PlayerActor.Status;
                var player2Info = player2Task.Result as PlayerActor.Status;

                return new StatusResponse(
                    _id,
                    new Player(player1Info.Name, player1Info.Score),
                    new Player(player2Info.Name, player2Info.Score));
            });

            task.PipeTo(sender, Self);
        }

        private void HandleShotTakenCommand(ShotTakenCommand msg)
        {
            _nextToPlay.Tell(new PlayerActor.ShotTakenCommand(msg.Score));

            if (msg.Score == 0)
            {
                _nextToPlay = _nextToPlay == _player1 ? _player2 : _player1;
            }
        }

        public static Props Props(Guid id, string player1, string player2)
        {
            return Akka.Actor.Props.Create(() => new GameActor(id, player1, player2));
        }
    }
}