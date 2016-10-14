namespace snooker_scorer
{
    using Akka.Actor;

    public static class ActorSystemRefs
    {
        public static ActorSystem ActorSystem;

        public static class Actors
        {
            public static IActorRef GameManager;
        }
    }
}