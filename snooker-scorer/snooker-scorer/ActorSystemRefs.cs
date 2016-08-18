using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snooker_scorer
{
    public static class ActorSystemRefs
    {
        public static ActorSystem ActorSystem;

        public static class Actors
        {
            public static IActorRef Logger;
        }
    }
}
