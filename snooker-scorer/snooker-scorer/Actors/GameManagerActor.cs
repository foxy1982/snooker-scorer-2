using Akka.Actor;

namespace snooker_scorer.Actors
{
    public partial class GameManagerActor : ReceiveActor
    {
        private int _numberOfGames;

        public GameManagerActor()
        {
            Receive<CreateGameRequest>(msg =>
            {
                _numberOfGames++;
                var game = Context.ActorOf<GameActor>();
                Sender.Tell(new CreateGameResponse(game));
            });
            Receive<GameCountRequest>(msg =>
            {
                Sender.Tell(new GameCountResponse(_numberOfGames));
            });
        }

        public class CreateGameRequest
        {
            public CreateGameRequest()
            {
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

    public partial class GameManagerActor
    {
        public class GameCountRequest
        {
        }

        public class GameCountResponse
        {
            public readonly int NumberOfGames;

            public GameCountResponse(int numberOfGames)
            {
                NumberOfGames = numberOfGames;
            }
        }
    }
}