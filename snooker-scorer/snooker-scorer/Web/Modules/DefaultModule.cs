namespace snooker_scorer.Web.Modules
{
    using System;
    using Actors;
    using Akka.Actor;
    using Nancy;
    using Nancy.ModelBinding;

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
            Post["/game/{gameId}/player"] = CreatePlayer(gameId);

            Post["/game/{gameId}/pot"]=HandlePot();
            Post["/game/{gameId}/miss"]=HandleMiss();
            Post["/game/{gameId}/foul"]=HandleFoul();
            */

            Post["/game"] = _ =>
            {
                var request = this.Bind<PostGameRequest>();

                var response =
                    ActorSystemRefs.Actors.GameManager.Ask(new GameManagerActor.CreateGameRequest(request.Player1, request.Player2)).Result as GameManagerActor.CreateGameResponse;

                return Negotiate.WithModel(new {id = response.Id}).WithStatusCode(HttpStatusCode.Created);
            };

            Get["/game/{id:guid}/status"] = _ =>
            {
                var request = this.Bind<GetGameStatusRequest>();

                var gameRequestResponse = ActorSystemRefs.Actors.GameManager.Ask(new GameManagerActor.GetGameRequest(request.Id)).Result as GameManagerActor.GetGameResponse;

                var gameActor = gameRequestResponse.GameActor;

                var response = gameActor.Ask(new GameActor.StatusRequest()).Result as GameActor.StatusResponse;

                return Negotiate.WithModel(new
                {
                    id = response.Id,
                    player1 = new
                    {
                        name = response.Player1.Name,
                        score = response.Player1.Score
                    },
                    player2 = new
                    {
                        name = response.Player2.Name,
                        score = response.Player2.Score
                    }
                });
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