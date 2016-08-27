using Akka.Actor;

namespace snooker_scorer.Actors
{
    public partial class GameManagerActor : ReceiveActor
    {
        public GameManagerActor()
        {
            Receive<CreateGameRequest>(msg =>
            {
                var game = Context.ActorOf(GameActor.Props(msg.Player1, msg.Player2));
                Sender.Tell(new CreateGameResponse(game));
            });
        }
    }
}