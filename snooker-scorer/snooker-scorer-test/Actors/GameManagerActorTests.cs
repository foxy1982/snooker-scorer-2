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
            var game = (gameResponse as GameManagerActor.CreateGameResponse).Game;

            game.Should().NotBeNull();
            game.Tell(new GameActor.StatusRequest());

            var status = ExpectMsg<GameActor.Status>();
            status.Player1.Name.Should().Be(player1);
            status.Player1.Score.Should().Be(0);
            status.Player2.Name.Should().Be(player2);
            status.Player2.Score.Should().Be(0);
        }
    }
}