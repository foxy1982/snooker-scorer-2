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
        private IActorRef logger;

        public DefaultModule()
        {
            logger = ActorSystemRefs.Actors.Logger;

            Get["/status"] = _ =>
            {
                logger.Tell("/status");
                return Response.AsJson(new {
                    status = "ok"
                });
            };
            /*
            Get["/game/{gameId}"] = id => GetGame(id);
            Post["/game"] = CreateGame();
            Post["/game/{gameId}/player"] = CreatePlayer(gameId);

            Post["/game/{gameId}/pot"]=HandlePot();
            Post["/game/{gameId}/miss"]=HandleMiss();
            Post["/game/{gameId}/foul"]=HandleFoul();
            */
        }
    }
}
