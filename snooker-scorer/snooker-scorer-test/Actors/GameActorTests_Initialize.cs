using Akka.Actor;
using Akka.TestKit.NUnit3;
using NUnit.Framework;
using snooker_scorer.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snooker_scorer_test.Actors
{
    [TestFixture]
    public class GameActorTests_Initialize : TestKit
    {
        [Test]
        public void ShouldInitialize()
        {
            var player1Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var player2Id = Guid.Parse("00000000-0000-0000-0000-000000000002");
            var player1 = CreateTestProbe("player1");
            var player2 = CreateTestProbe("player2");

            var player1Props = Props.Create(() => new ForwarderActor(player1.Ref));
            var player2Props = Props.Create(() => new ForwarderActor(player2.Ref));

            Sys.ActorOf(Props.Create(() => new GameActor(Guid.NewGuid(),
                player1Id,
                player1Props,
                player2Id,
                player2Props)));
        }
    }
}