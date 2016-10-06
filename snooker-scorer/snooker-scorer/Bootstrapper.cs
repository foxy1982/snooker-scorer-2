using Akka.Actor;
using Akka.Configuration;
using snooker_scorer.Actors;

namespace snooker_scorer
{
    public static class Bootstrapper
    {
        public static void InitializeActorSystem()
        {
            ActorSystemRefs.ActorSystem = ActorSystem.Create("snooker");
            ActorSystemRefs.Actors.GameManager = ActorSystemRefs.ActorSystem.ActorOf<GameManagerActor>("game-manager");
        }
    }
}