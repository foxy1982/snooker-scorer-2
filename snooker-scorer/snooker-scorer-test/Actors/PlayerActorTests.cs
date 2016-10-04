using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using snooker_scorer.Actors;
using System;

namespace snooker_scorer_test.Actors
{
    [TestFixture]
    public class PlayerActorTests : TestKit
    {
        private Guid _playerId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        private string _playerName = "Player 2";
        private int _playerNumber = 2;

        private TestProbe _foulCounter;

        private IActorRef _target;

        [SetUp]
        public void SetUp()
        {
            _foulCounter = CreateTestProbe("foulCounter");
            var foulCounterProps = Props.Create(() => new ForwarderActor(_foulCounter.Ref));

            _target = Sys.ActorOf(Props.Create(() => new PlayerActor(
                _playerId,
                _playerName,
                _playerNumber,
                foulCounterProps)));
        }

        [TearDown]
        public void TearDown()
        {
            Shutdown();
        }

        [Test]
        public void ShouldRequestStatusFromFoulCounter()
        {
            _target.Tell(new PlayerActor.StatusRequest());

            _foulCounter.ExpectMsg<FoulCounterActor.FoulCountRequest>();
            _foulCounter.Reply(new FoulCounterActor.FoulCountResponse(5, 6));

            var response = ExpectMsg<PlayerActor.Status>();

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Id, Is.EqualTo(_playerId));
            Assert.That(response.Name, Is.EqualTo(_playerName));
            Assert.That(response.Score, Is.EqualTo(0));
            Assert.That(response.Fouls.Count, Is.EqualTo(5));
            Assert.That(response.Fouls.Value, Is.EqualTo(6));
        }

        [Test]
        public void ShouldAddScoreOn()
        {
            _target.Tell(new PlayerActor.ShotTakenCommand(4));

            _target.Tell(new PlayerActor.StatusRequest());

            _foulCounter.ExpectMsg<FoulCounterActor.FoulCountRequest>();
            _foulCounter.Reply(new FoulCounterActor.FoulCountResponse(5, 6));

            var response = ExpectMsg<PlayerActor.Status>();

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Score, Is.EqualTo(4));
        }

        [Test]
        public void ShouldAddOnAwardedFoulPoints()
        {
            _target.Tell(new PlayerActor.AwardFoulPointsCommand(7));

            _target.Tell(new PlayerActor.StatusRequest());

            _foulCounter.ExpectMsg<FoulCounterActor.FoulCountRequest>();
            _foulCounter.Reply(new FoulCounterActor.FoulCountResponse(5, 6));

            var response = ExpectMsg<PlayerActor.Status>();

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Score, Is.EqualTo(7));
        }

        [Test]
        public void ShouldTellFoulCounterAboutFoulsCommitted()
        {
            _target.Tell(new PlayerActor.FoulCommittedCommand(7));

            var message = _foulCounter.ExpectMsg<FoulCounterActor.Foul>();

            Assert.That(message, Is.Not.Null);
            Assert.That(message.Value, Is.EqualTo(7));
        }
    }
}