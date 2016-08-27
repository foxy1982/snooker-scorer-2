using Akka.Actor;
using Akka.Configuration;
using Nancy;
using Nancy.ModelBinding;
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
            Get["/status"] = _ =>
            {
                return Response.AsJson(new
                {
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

            Post["/game"] = _ =>
            {
                var request = this.Bind<PostGameRequest>();

                var response = ActorSystemRefs.Actors.GameManager.Ask(new GameManagerActor.CreateGameRequest(request.Player1, request.Player2)).Result as GameManagerActor.CreateGameResponse;

                return Response.AsJson(new
                {
                    game = response.Id
                });
            };
        }

        public class PostGameRequest
        {
            public string Player1 { get; set; }
            public string Player2 { get; set; }
        }
    }
}