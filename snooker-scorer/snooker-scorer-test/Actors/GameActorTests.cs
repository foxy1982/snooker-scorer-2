using Akka.Actor;
using Akka.TestKit.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using snooker_scorer.Actors;
using System;

namespace snooker_scorer_test.Actors
{
    [TestFixture]
    public class GameActorTests : TestKit
    {
        [TearDown]
        public void TearDown()
        {
            Shutdown();
        }

        private IActorRef BuildTarget()
        {
            var id = Guid.Empty;
            var player1 = "Alan";
            var player2 = "John";
            return ActorOfAsTestActorRef(() => new GameActor(id, player1, player2));
        }

        [Test]
        public void ShouldReturnNoScoreAfterCreatingGame()
        {
            var id = Guid.Empty;
            var player1 = "Alan";
            var player2 = "John";
            var target = ActorOfAsTestActorRef(() => new GameActor(id, player1, player2));

            var response = target.Ask(new GameActor.StatusRequest()).Result as GameActor.StatusResponse;

            response.Player1.Name.Should().Be(player1);
            response.Player1.Score.Should().Be(0);
            response.Player2.Name.Should().Be(player2);
            response.Player2.Score.Should().Be(0);
        }

        [Test]
        public void ShouldAddFirstScoreOntoPlayer1()
        {
            var target = BuildTarget();
            target.Tell(new GameActor.ShotTakenCommand(5));
            var response = target.Ask(new GameActor.StatusRequest()).Result as GameActor.StatusResponse;

            response.Player1.Score.Should().Be(5);
            response.Player2.Score.Should().Be(0);
        }

        [Test]
        public void ShouldAddSecondShotOntoPlayer1IfFirstShotScored()
        {
            var target = BuildTarget();
            target.Tell(new GameActor.ShotTakenCommand(5));
            target.Tell(new GameActor.ShotTakenCommand(5));
            var response = target.Ask(new GameActor.StatusRequest()).Result as GameActor.StatusResponse;

            response.Player1.Score.Should().Be(10);
            response.Player2.Score.Should().Be(0);
        }

        [Test]
        public void ShouldAddSecondShotOntoPlayer2IfFirstShotNotScored()
        {
            var target = BuildTarget();
            target.Tell(new GameActor.ShotTakenCommand(0));
            target.Tell(new GameActor.ShotTakenCommand(5));
            var response = target.Ask(new GameActor.StatusRequest()).Result as GameActor.StatusResponse;

            response.Player1.Score.Should().Be(0);
            response.Player2.Score.Should().Be(5);
        }
    }
}