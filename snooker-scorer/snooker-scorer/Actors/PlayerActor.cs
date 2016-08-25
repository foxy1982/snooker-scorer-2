using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snooker_scorer.Actors
{
    public class PlayerActor : ReceiveActor
    {
        public class Status
        {
            public readonly string Name;

            public Status(string name)
            {
                Name = name;
            }
        }

        public class StatusRequest
        {
            public StatusRequest()
            {
            }
        }

        private string _name;

        public PlayerActor(string name)
        {
            _name = name;
            Receive<StatusRequest>(msg =>
            {
                Sender.Tell(new Status(_name));
            });
        }

        public static Props Props(string name)
        {
            return Akka.Actor.Props.Create(() => new PlayerActor(name));
        }
    }
}