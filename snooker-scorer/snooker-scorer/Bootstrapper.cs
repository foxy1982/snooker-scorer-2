using Akka.Actor;
using Akka.Configuration;
using snooker_scorer.Actors;

namespace snooker_scorer
{
    public static class Bootstrapper
    {
        public static void InitializeActorSystem()
        {
            var config = ConfigurationFactory.ParseString(
                @"akka {
                    loglevel = DEBUG
                    suppress-json-serializer-warning = true
                }");
            ActorSystemRefs.ActorSystem = ActorSystem.Create("snooker", config);
            ActorSystemRefs.Actors.GameManager = ActorSystemRefs.ActorSystem.ActorOf<GameManagerActor>("game-manager");
        }
    }
}