namespace snooker_scorer
{
    using Actors;
    using Akka.Actor;

    public static class Bootstrapper
    {
        public static void InitializeActorSystem()
        {
            ActorSystemRefs.ActorSystem = ActorSystem.Create("snooker");
            ActorSystemRefs.Actors.GameManager = ActorSystemRefs.ActorSystem.ActorOf<GameManagerActor>("game-manager");
        }
    }
}