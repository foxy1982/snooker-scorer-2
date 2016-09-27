﻿using Akka.Actor;
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

        public GameActor(Guid id, string player1Name, string player2Name)
        {
            _log.Debug("GameActor ctor");
            _id = id;
            CreatePlayer(player1Name, 1);
            CreatePlayer(player2Name, 2);

            Receive<StatusRequest>(msg => HandleStatusRequest());
            Receive<ShotTakenCommand>(msg => HandleShotTakenCommand(msg));
            Receive<FoulCommittedCommand>(msg => HandleFoulCommittedCommand(msg));
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
                    playerInfos.Select(x => new Player(x.Id, x.Name, x.Score)));
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
            GetOtherPlayer(msg.Id).Tell(new PlayerActor.AwardFoulPointsCommand(msg.Value));
        }

        public static Props Props(Guid id, string player1, string player2)
        {
            return Akka.Actor.Props.Create(() => new GameActor(id, player1, player2));
        }
    }
}