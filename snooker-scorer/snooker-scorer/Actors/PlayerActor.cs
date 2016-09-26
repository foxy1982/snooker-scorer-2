namespace snooker_scorer.Actors
{
    using Akka.Actor;

    public partial class PlayerActor : ReceiveActor
    {
        private readonly string _name;
        private int _score;

        public PlayerActor(string name)
        {
            _name = name;
            Receive<StatusRequest>(msg => { Sender.Tell(new Status(_name, _score)); });
            Receive<ShotTakenCommand>(msg => { _score += msg.Score; });
            Receive<AwardFoulPointsCommand>(msg => { _score += msg.Value; });
        }

        public static Props Props(string name)
        {
            return Akka.Actor.Props.Create(() => new PlayerActor(name));
        }
    }
}