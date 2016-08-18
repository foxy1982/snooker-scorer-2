using Akka.Actor;
using Akka.Configuration;
using snooker_scorer.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snooker_scorer
{
    public static class Bootstrapper
    {
        public static void InitializeActorSystem()
        {
            var config = ConfigurationFactory.ParseString(@"akka.suppress-json-serializer-warning:true");
            ActorSystemRefs.ActorSystem = ActorSystem.Create("snooker", config);
            ActorSystemRefs.Actors.Logger = ActorSystemRefs.ActorSystem.ActorOf<LoggingActor>("logger");
        }
    }
}
