using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snooker_scorer.Actors
{
    public class LoggingActor : ReceiveActor
    {
        public LoggingActor()
        {
            Receive<string>(message => Console.WriteLine(message));
        }
    }
}
