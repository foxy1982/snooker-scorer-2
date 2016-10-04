using Akka.Actor;
using Akka.Event;
using System;
using System.Collections.Generic;

namespace snooker_scorer.Actors
{
    public partial class GameManagerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();

        private readonly Dictionary<Guid, IActorRef> _games;

        public GameManagerActor()
        {
            _log.Debug("ctor");

            _games = new Dictionary<Guid, IActorRef>();

            Receive<CreateGameRequest>(msg =>
            {
                var player1Id = Guid.NewGuid();
                var player2Id = Guid.NewGuid();
                var id = Guid.NewGuid();
                var game = Context.ActorOf(
                        GameActor.Props(id,
                            player1Id,
                            PlayerActor.Props(player1Id, msg.Player1, 1),
                            player2Id,
                            PlayerActor.Props(player2Id, msg.Player2, 2)),
                        $"Game:{id}");
                _games.Add(id, game);
                Sender.Tell(new CreateGameResponse(id));
            });

            Receive<GetGameRequest>(msg =>
            {
                if (!_games.ContainsKey(msg.Id))
                {
                    Sender.Tell(new GetGameResponse(null));
                    return;
                }

                Sender.Tell(new GetGameResponse(_games[msg.Id]));
            });

            Receive<EndGameCommand>(msg =>
            {
                _games[msg.Id].GracefulStop(TimeSpan.FromSeconds(5));
                _games[msg.Id] = null;
            });
        }
    }
}