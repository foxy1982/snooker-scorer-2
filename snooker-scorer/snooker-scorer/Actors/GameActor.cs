using Akka.Actor;
using System.Threading.Tasks;

namespace snooker_scorer.Actors
{
    public partial class GameActor : ReceiveActor
    {
        private readonly IActorRef player1;
        private readonly IActorRef player2;

        public GameActor(string player1Name, string player2Name)
        {
            player1 = Context.ActorOf(PlayerActor.Props(player1Name));
            player2 = Context.ActorOf(PlayerActor.Props(player2Name));

            Receive<StatusRequest>(msg => HandleStatusRequest(msg));
        }

        private void HandleStatusRequest(StatusRequest msg)
        {
            var sender = Sender;

            var task = Task.Run(async () =>
            {
                var player1Task = player1.Ask(new PlayerActor.StatusRequest());
                var player2Task = player2.Ask(new PlayerActor.StatusRequest());

                await Task.WhenAll(player1Task, player2Task);

                var player1Info = player1Task.Result as PlayerActor.Status;
                var player2Info = player2Task.Result as PlayerActor.Status;

                return new StatusResponse(
                    new Player(player1Info.Name),
                    new Player(player2Info.Name));
            });

            task.PipeTo(sender, Self);
        }

        public static Props Props(string player1, string player2)
        {
            return Akka.Actor.Props.Create(() => new GameActor(player1, player2));
        }
    }
}