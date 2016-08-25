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
    public class GameManagerActorTests : TestKit
    {
        [TearDown]
        public void TearDown()
        {
            Shutdown();
        }

        [Test]
        public void ShouldStartWithNoGamesInSystem()
        {
            var target = ActorOfAsTestActorRef<GameManagerActor>();
            target.Tell(new GameManagerActor.GameCountRequest());
            ExpectMsg<GameManagerActor.GameCountResponse>().ShouldBeEquivalentTo(
                new GameManagerActor.GameCountResponse(0));
        }

        [Test]
        public void ShouldHaveOneGameInSystemAfterGameCreated()
        {
            IgnoreMessages(x => true);
            var target = ActorOfAsTestActorRef<GameManagerActor>();
            target.Tell(new GameManagerActor.CreateGameRequest());
            IgnoreNoMessages();
            target.Tell(new GameManagerActor.GameCountRequest());
            ExpectMsg<GameManagerActor.GameCountResponse>().ShouldBeEquivalentTo(
                new GameManagerActor.GameCountResponse(1));
        }

        [Test]
        public void ShouldCreateNewGame()
        {
            var target = ActorOfAsTestActorRef<GameManagerActor>();
            target.Tell(new GameManagerActor.CreateGameRequest());
            var response = ExpectMsg<GameManagerActor.CreateGameResponse>();
            response.Game.Should().NotBeNull();
        }
    }
}