using Akka.Actor;
using Akka.Event;

namespace snooker_scorer.Actors
{
    public partial class GameManagerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();

        public GameManagerActor()
        {
            _log.Debug("ctor");
            Receive<CreateGameRequest>(msg =>
            {
                var game = Context.ActorOf(GameActor.Props(msg.Player1, msg.Player2));
                var statusResponse = game.Ask(new GameActor.StatusRequest()).Result as GameActor.StatusResponse;
                Sender.Tell(new CreateGameResponse(statusResponse.Id));
            });
        }
    }
}