using Akka.Actor;
using Akka.Event;
using System;
using System.Collections.Generic;

namespace snooker_scorer.Actors
{
    public partial class GameManagerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();

        private Dictionary<Guid, IActorRef> _games;

        public GameManagerActor()
        {
            _log.Debug("ctor");

            _games = new Dictionary<Guid, IActorRef>();

            Receive<CreateGameRequest>(msg =>
            {
                var id = Guid.NewGuid();
                var game = Context.ActorOf(GameActor.Props(id, msg.Player1, msg.Player2), "Game_" + id.ToString());
                _games.Add(id, game);
                Sender.Tell(new CreateGameResponse(id));
            });

            Receive<GetGameRequest>(msg =>
            {
                Sender.Tell(new GetGameResponse(_games[msg.Id]));
            });

            Receive<EndGameCommand>(msg =>
            {
                _games[msg.Id].GracefulStop(TimeSpan.FromSeconds(5));
            });
        }
    }
}