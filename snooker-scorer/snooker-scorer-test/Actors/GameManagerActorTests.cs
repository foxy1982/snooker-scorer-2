using Akka.Actor;
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
        public void ShouldCreateNewGame()
        {
            var player1 = "Alan";
            var player2 = "John";
            var target = ActorOfAsTestActorRef<GameManagerActor>();
            var gameResponse = target.Ask(new GameManagerActor.CreateGameRequest(player1, player2)).Result;
            var id = (gameResponse as GameManagerActor.CreateGameResponse).Id;

            id.Should().NotBeEmpty();
        }
    }
}