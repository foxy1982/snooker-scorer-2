namespace snooker_scorer.Actors
{
    using Akka.Actor;
    using System;

    public partial class PlayerActor : ReceiveActor
    {
        private readonly string _name;
        private int _playerNumber;
        private int _score;
        private Guid _id;

        public PlayerActor(string name, int playerNumber)
        {
            _id = Guid.NewGuid();
            _name = name;
            _playerNumber = playerNumber;
            Receive<StatusRequest>(msg => { Sender.Tell(new Status(_id, _name, _playerNumber, _score)); });
            Receive<ShotTakenCommand>(msg => { _score += msg.Score; });
            Receive<AwardFoulPointsCommand>(msg => { _score += msg.Value; });
        }

        public static Props Props(string name, int playerNumber)
        {
            return Akka.Actor.Props.Create(() => new PlayerActor(name, playerNumber));
        }
    }
}