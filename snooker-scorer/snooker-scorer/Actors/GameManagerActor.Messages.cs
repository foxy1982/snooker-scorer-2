using Akka.Actor;

namespace snooker_scorer.Actors
{
    public partial class GameManagerActor
    {
        public class CreateGameRequest
        {
            public readonly string Player1;
            public readonly string Player2;

            public CreateGameRequest(string player1, string player2)
            {
                Player1 = player1;
                Player2 = player2;
            }
        }

        public class CreateGameResponse
        {
            public readonly IActorRef Game;

            public CreateGameResponse(IActorRef game)
            {
                Game = game;
            }
        }
    }
}