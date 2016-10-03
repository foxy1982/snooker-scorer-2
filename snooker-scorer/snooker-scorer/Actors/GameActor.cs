using Akka.Actor;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace snooker_scorer.Actors
{
    public partial class GameActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();

        private readonly Guid _id;

        private readonly IDictionary<Guid, IActorRef> _players = new Dictionary<Guid, IActorRef>();

        public GameActor(Guid id, Props player1Props, Props player2Props)
        {
            _log.Debug("GameActor ctor");
            _id = id;
            CreatePlayer(player1Props);
            CreatePlayer(player2Props);

            Receive<StatusRequest>(msg => HandleStatusRequest());
            Receive<ShotTakenCommand>(msg => HandleShotTakenCommand(msg));
            Receive<FoulCommittedCommand>(msg => HandleFoulCommittedCommand(msg));
        }

        private void CreatePlayer(Props playerProps)
        {
            _log.Debug("CreatePlayer");
            var player = Context.ActorOf(playerProps);
            var playerStatus = player.Ask(new PlayerActor.StatusRequest()).Result as PlayerActor.Status;
            _players.Add(playerStatus.Id, player);
        }

        private void CreatePlayer(string name, int playerNumber)
        {
            _log.Debug("CreatePlayer");
            var player = Context.ActorOf(PlayerActor.Props(name, playerNumber));
            var playerStatus = player.Ask(new PlayerActor.StatusRequest()).Result as PlayerActor.Status;
            _players.Add(playerStatus.Id, player);
        }

        private IActorRef GetOtherPlayer(Guid id)
        {
            return _players.First(x => x.Key != id).Value;
        }

        private void HandleStatusRequest()
        {
            _log.Debug("HandleStatusRequest");
            var sender = Sender;

            var task = Task.Run(async () =>
            {
                var tasks = _players.Select(x => x.Value.Ask(new PlayerActor.StatusRequest())).ToList();

                await Task.WhenAll(tasks);

                var playerInfos = tasks.Select(x => x.Result as PlayerActor.Status).OrderBy(x => x.PlayerNumber);

                return new StatusResponse(
                    _id,
                    playerInfos.Select(x =>
                        new StatusResponse.Player(x.Id, x.Name, x.Score,
                            new StatusResponse.Player.FoulCount(x.Fouls.Count, x.Fouls.Value))));
            });

            task.PipeTo(sender, Self);
        }

        private void HandleShotTakenCommand(ShotTakenCommand msg)
        {
            _log.Debug("HandleShotTakenCommand");
            _players[msg.PlayerId].Tell(new PlayerActor.ShotTakenCommand(msg.Score));
        }

        private void HandleFoulCommittedCommand(FoulCommittedCommand msg)
        {
            _log.Debug("HandleFoulCommittedCommand");
            _players[msg.Id].Tell(new PlayerActor.FoulCommittedCommand(msg.Value));
            GetOtherPlayer(msg.Id).Tell(new PlayerActor.AwardFoulPointsCommand(msg.Value));
        }

        public static Props Props(Guid id, Props player1Props, Props player2Props)
        {
            return Akka.Actor.Props.Create(() => new GameActor(id, player1Props, player2Props));
        }
    }
}