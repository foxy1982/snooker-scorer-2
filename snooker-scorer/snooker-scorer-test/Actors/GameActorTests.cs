using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using NUnit.Framework;
using snooker_scorer.Actors;
using System;
using System.Linq;

namespace snooker_scorer_test.Actors
{
    [TestFixture]
    public class GameActorTests : TestKit
    {
        private Guid _id = Guid.NewGuid();
        private Guid _player1Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private Guid _player2Id = Guid.Parse("00000000-0000-0000-0000-000000000002");
        private string _player1Name = "Player 1";
        private string _player2Name = "Player 2";

        private TestProbe _player1;
        private TestProbe _player2;

        private IActorRef _target;

        [SetUp]
        public void SetUp()
        {
            _player1 = CreateTestProbe("player1");
            _player2 = CreateTestProbe("player2");

            var player1Props = Props.Create(() => new ForwarderActor(_player1.Ref));
            var player2Props = Props.Create(() => new ForwarderActor(_player2.Ref));

            _target = Sys.ActorOf(Props.Create(() => new GameActor(_id,
                _player1Id,
                player1Props,
                _player2Id,
                player2Props)));
        }

        [TearDown]
        public void TearDown()
        {
            Shutdown();
        }

        [Test]
        public void ShouldRequestStatusFromPlayers()
        {
            _target.Tell(new GameActor.StatusRequest());

            _player1.ExpectMsg<PlayerActor.StatusRequest>();
            _player2.ExpectMsg<PlayerActor.StatusRequest>();
            _player1.Reply(new PlayerActor.Status(_player1Id, _player1Name, 1, 0, new PlayerActor.Status.FoulCount(0, 0)));
            _player2.Reply(new PlayerActor.Status(_player2Id, _player2Name, 2, 0, new PlayerActor.Status.FoulCount(0, 0)));

            var response = ExpectMsg<GameActor.StatusResponse>();

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Id, Is.EqualTo(_id));
            Assert.That(response.Players.First().Id, Is.EqualTo(_player1Id));
            Assert.That(response.Players.First().Name, Is.EqualTo(_player1Name));
            Assert.That(response.Players.First().Score, Is.EqualTo(0));
            Assert.That(response.Players.Last().Id, Is.EqualTo(_player2Id));
            Assert.That(response.Players.Last().Name, Is.EqualTo(_player2Name));
            Assert.That(response.Players.Last().Score, Is.EqualTo(0));
        }

        [Test]
        public void ShouldGiveShotToPlayer()
        {
            var shotValue = 6;
            _target.Tell(new GameActor.ShotTakenCommand(_player2Id, shotValue));

            _player1.ExpectNoMsg();
            var message = _player2.ExpectMsg<PlayerActor.ShotTakenCommand>();
            Assert.That(message.Score, Is.EqualTo(shotValue));
        }

        [Test]
        public void ShouldMarkFoulAgainstPlayer()
        {
            var shotValue = 6;
            _target.Tell(new GameActor.FoulCommittedCommand(_player2Id, shotValue));

            var foulMessage = _player2.ExpectMsg<PlayerActor.FoulCommittedCommand>();
            Assert.That(foulMessage.Value, Is.EqualTo(shotValue));
        }

        [Test]
        public void ShouldAwardFoulPointsToOtherPlayer()
        {
            var shotValue = 6;
            _target.Tell(new GameActor.FoulCommittedCommand(_player2Id, shotValue));

            var awardMessage = _player1.ExpectMsg<PlayerActor.AwardFoulPointsCommand>();
            Assert.That(awardMessage.Value, Is.EqualTo(shotValue));
        }
    }
}