using Akka.Actor;
using System;

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
