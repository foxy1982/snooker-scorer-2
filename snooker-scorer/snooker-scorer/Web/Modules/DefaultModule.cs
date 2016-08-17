using Akka.Actor;
using Akka.Configuration;
using Nancy;
using snooker_scorer.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snooker_scorer.Web.Modules
{
    public class DefaultModule : NancyModule
    {
        public DefaultModule()
        {
            var config = ConfigurationFactory.ParseString(@"akka.suppress-json-serializer-warning:true");
            var actorSystem = ActorSystem.Create("snooker", config);
            var logger = actorSystem.ActorOf<LoggingActor>("logger");

            Get["/status"] = _ =>
            {
                logger.Tell("/status");
                return Response.AsJson(new {
                    status = "ok"
                });
            };
        }
    }
}
