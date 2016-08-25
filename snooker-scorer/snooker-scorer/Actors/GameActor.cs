using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snooker_scorer.Actors
{
    public class GameActor : ReceiveActor
    {
        private readonly IActorRef player1;
        private readonly IActorRef player2;

        public GameActor()
        {
            player1 = Context.ActorOf<PlayerActor>();
            player2 = Context.ActorOf<PlayerActor>();
        }
    }
}