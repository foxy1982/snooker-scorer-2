using Akka.Actor;
using Akka.TestKit.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using snooker_scorer.Actors;

namespace snooker_scorer_test.Actors
{
    using System;

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

        [Test]
        public void ShouldReturnRequestedGame()
        {
            var player1 = "Alan";
            var player2 = "John";
            var target = ActorOfAsTestActorRef<GameManagerActor>();
            var gameResponse = target.Ask(new GameManagerActor.CreateGameRequest(player1, player2)).Result;
            var id = (gameResponse as GameManagerActor.CreateGameResponse).Id;

            var response = target.Ask(new GameManagerActor.GetGameRequest(id)).Result as GameManagerActor.GetGameResponse;

            response.Should().NotBeNull();
        }

        [Test]
        public void ShouldReturnNullForInvalidGame()
        {
            var target = ActorOfAsTestActorRef<GameManagerActor>();

            var response = target.Ask(new GameManagerActor.GetGameRequest(Guid.NewGuid())).Result as GameManagerActor.GetGameResponse;

            response.Should().NotBeNull();
            response.GameActor.Should().BeNull();
        }

        [Test]
        public void ShouldDeleteGameOnEndGameRequest()
        {
            var player1 = "Alan";
            var player2 = "John";
            var target = ActorOfAsTestActorRef<GameManagerActor>();
            var gameResponse = target.Ask(new GameManagerActor.CreateGameRequest(player1, player2)).Result;
            var id = (gameResponse as GameManagerActor.CreateGameResponse).Id;

            target.Tell(new GameManagerActor.EndGameCommand(id));

            var response = target.Ask(new GameManagerActor.GetGameRequest(id)).Result as GameManagerActor.GetGameResponse;

            response.Should().NotBeNull();
            response.GameActor.Should().BeNull();
        }
    }
}