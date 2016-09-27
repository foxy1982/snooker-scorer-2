namespace snooker_scorer.Web.Modules
{
    using Actors;
    using Akka.Actor;
    using Nancy;
    using Nancy.ModelBinding;
    using System;
    using System.Linq;

    public class DefaultModule : NancyModule
    {
        public DefaultModule()
        {
            Get["/status"] = _ => Response.AsJson(new
            {
                status = "ok"
            });
            /*
            Post["/game/{gameId}/miss"]=HandleMiss();
            Post["/game/{gameId}/foul"]=HandleFoul();
            */

            Post["/game"] = _ =>
            {
                var request = this.Bind<PostGameRequest>();

                var response =
                    ActorSystemRefs.Actors.GameManager.Ask(new GameManagerActor.CreateGameRequest(request.Player1, request.Player2)).Result as GameManagerActor.CreateGameResponse;

                return Negotiate.WithModel(new { id = response.Id }).WithStatusCode(HttpStatusCode.Created);
            };

            Get["/game/{id:guid}"] = _ =>
            {
                var request = this.Bind<GetGameStatusRequest>();

                var gameRequestResponse = ActorSystemRefs.Actors.GameManager.Ask(new GameManagerActor.GetGameRequest(request.Id)).Result as GameManagerActor.GetGameResponse;

                var gameActor = gameRequestResponse.GameActor;

                var response = gameActor.Ask(new GameActor.StatusRequest()).Result as GameActor.StatusResponse;

                return Negotiate.WithModel(new
                {
                    id = response.Id,
                    players = response.Players.Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Score
                    })
                });
            };

            Post["/game/{id:guid}/shot"] = _ =>
            {
                var request = this.Bind<PostShotTakenRequest>();

                var gameRequestResponse = ActorSystemRefs.Actors.GameManager.Ask(new GameManagerActor.GetGameRequest(request.Id)).Result as GameManagerActor.GetGameResponse;

                var gameActor = gameRequestResponse.GameActor;

                //gameActor.Tell(new GameActor.ShotTakenCommand(request.Value));

                return Negotiate.WithStatusCode(HttpStatusCode.Accepted);
            };

            Delete["/game/{id:guid}"] = _ =>
            {
                var request = this.Bind<DeleteGameRequest>();

                ActorSystemRefs.Actors.GameManager.Tell(new GameManagerActor.EndGameCommand(request.Id));

                return Negotiate.WithStatusCode(HttpStatusCode.Accepted);
            };
        }

        public class PostGameRequest
        {
            public string Player1 { get; set; }
            public string Player2 { get; set; }
        }

        public class PostShotTakenRequest
        {
            public Guid Id { get; set; }

            public int Value { get; set; }
        }

        public class DeleteGameRequest
        {
            public Guid Id { get; set; }
        }

        public class GetGameStatusRequest
        {
            public Guid Id { get; set; }
        }
    }
}