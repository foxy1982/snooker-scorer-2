using Akka.Actor;
using Akka.TestKit.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using snooker_scorer.Actors;
using System;
using System.Linq;

namespace snooker_scorer_test.Actors
{
    [TestFixture]
    public class GameActorTests : TestKit
    {
        private Guid _id;
        private string _player1Name;
        private Guid _player1Id;
        private string _player2Name;
        private Guid _player2Id;
        private IActorRef _target;

        [SetUp]
        public void SetUp()
        {
            _id = Guid.Empty;
            _player1Name = "Alan";
            _player2Name = "John";
            _target = ActorOfAsTestActorRef(() => new GameActor(_id, _player1Name, _player2Name));

            var status = _target.Ask(new GameActor.StatusRequest()).Result as GameActor.StatusResponse;
            _player1Id = status.Players.First().Id;
            _player2Id = status.Players.Last().Id;
        }

        [TearDown]
        public void TearDown()
        {
            Shutdown();
        }

        [Test]
        public void ShouldReturnNoScoreAfterCreatingGame()
        {
            var response = _target.Ask(new GameActor.StatusRequest()).Result as GameActor.StatusResponse;

            response.Players.First().Name.Should().Be(_player1Name);
            response.Players.First().Score.Should().Be(0);
            response.Players.Last().Name.Should().Be(_player2Name);
            response.Players.Last().Score.Should().Be(0);
        }

        [Test]
        public void ShouldAddScoreOntoPlayer1()
        {
            _target.Tell(new GameActor.ShotTakenCommand(_player1Id, 5));
            var response = _target.Ask(new GameActor.StatusRequest()).Result as GameActor.StatusResponse;

            response.Players.First().Score.Should().Be(5);
            response.Players.Last().Score.Should().Be(0);
        }

        [Test]
        public void ShouldAddScoreOntoPlayer2()
        {
            _target.Tell(new GameActor.ShotTakenCommand(_player2Id, 5));
            var response = _target.Ask(new GameActor.StatusRequest()).Result as GameActor.StatusResponse;

            response.Players.First().Score.Should().Be(0);
            response.Players.Last().Score.Should().Be(5);
        }

        [Test]
        public void ShouldAddFoulScoreOntoPlayer1WhenPlayer2Fouls()
        {
            _target.Tell(new GameActor.FoulCommittedCommand(_player2Id, 5));
            var response = _target.Ask(new GameActor.StatusRequest()).Result as GameActor.StatusResponse;

            response.Players.First().Score.Should().Be(5);
            response.Players.Last().Score.Should().Be(0);
        }

        [Test]
        public void ShouldAddFoulScoreOntoPlaye2WhenPlayer1Fouls()
        {
            _target.Tell(new GameActor.FoulCommittedCommand(_player1Id, 5));
            var response = _target.Ask(new GameActor.StatusRequest()).Result as GameActor.StatusResponse;

            response.Players.First().Score.Should().Be(0);
            response.Players.Last().Score.Should().Be(5);
        }
    }
}