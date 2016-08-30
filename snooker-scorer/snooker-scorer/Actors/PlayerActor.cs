using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snooker_scorer.Actors
{
    public partial class PlayerActor : ReceiveActor
    {
        private string _name;
        private int _score;

        public PlayerActor(string name)
        {
            _name = name;
            Receive<StatusRequest>(msg =>
            {
                Sender.Tell(new Status(_name, _score));
            });
            Receive<ShotTakenCommand>(msg =>
            {
                _score += msg.Score;
            });
        }

        public static Props Props(string name)
        {
            return Akka.Actor.Props.Create(() => new PlayerActor(name));
        }
    }
}