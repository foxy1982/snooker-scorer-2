using Akka.Actor;
using System;

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
            public readonly Guid Id;

            public CreateGameResponse(Guid id)
            {
                Id = id;
            }
        }

        public class GetGameRequest
        {
            public readonly Guid Id;

            public GetGameRequest(Guid id)
            {
                Id = id;
            }
        }

        public class GetGameResponse
        {
            public readonly IActorRef GameActor;

            public GetGameResponse(IActorRef gameActor)
            {
                GameActor = gameActor;
            }
        }

        public class EndGameCommand
        {
            public readonly Guid Id;

            public EndGameCommand(Guid id)
            {
                Id = id;
            }
        }
    }
}