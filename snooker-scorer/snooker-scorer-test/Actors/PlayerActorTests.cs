using Akka.TestKit.NUnit3;
using FluentAssertions;
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
    public class PlayerActorTests : TestKit
    {
        [TearDown]
        public void TearDown()
        {
            Shutdown();
        }

        [Test]
        public void ShouldRespondWithName()
        {
            var name = "Lex";
            var player = ActorOfAsTestActorRef<PlayerActor>(PlayerActor.Props(name));
            player.Tell(new PlayerActor.StatusRequest());
            var response = ExpectMsg<PlayerActor.Status>();
            response.Name.Should().Be(name);
        }
    }
}