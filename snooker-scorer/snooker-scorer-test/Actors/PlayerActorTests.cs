using Akka.Actor;
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
        [TearDown]
        public void TearDown()
        {
            Shutdown();
        }

        [Test]
        public void ShouldRespondWithNameAndNoScoreWhenCreated()
        {
            var name = "Lex";
            var player = ActorOfAsTestActorRef<PlayerActor>(PlayerActor.Props(name, 1));
            var status = player.Ask(new PlayerActor.StatusRequest()).Result as PlayerActor.Status;
            status.Name.Should().Be(name);
            status.Id.Should().NotBe(Guid.Empty);
            status.Score.Should().Be(0);
            status.Fouls.Count.Should().Be(0);
            status.Fouls.Value.Should().Be(0);
        }

        [Test]
        public void ShouldAddFoulsOnWhenFoulCommitted()
        {
            var name = "Lex";
            var player = ActorOfAsTestActorRef<PlayerActor>(PlayerActor.Props(name, 1));
            player.Tell(new PlayerActor.FoulCommittedCommand(5));
            var status = player.Ask(new PlayerActor.StatusRequest()).Result as PlayerActor.Status;
            status.Name.Should().Be(name);
            status.Id.Should().NotBe(Guid.Empty);
            status.Score.Should().Be(0);
            status.Fouls.Count.Should().Be(1);
            status.Fouls.Value.Should().Be(5);
        }

        [Test]
        public void ShouldNotAddFoulsOnWhenFoulPointsAwarded()
        {
            var name = "Lex";
            var player = ActorOfAsTestActorRef<PlayerActor>(PlayerActor.Props(name, 1));
            player.Tell(new PlayerActor.AwardFoulPointsCommand(5));
            var status = player.Ask(new PlayerActor.StatusRequest()).Result as PlayerActor.Status;
            status.Name.Should().Be(name);
            status.Id.Should().NotBe(Guid.Empty);
            status.Score.Should().Be(5);
            status.Fouls.Count.Should().Be(0);
            status.Fouls.Value.Should().Be(0);
        }

        [Test]
        public void TwoPlayersShouldNotHaveSameId()
        {
            var name1 = "Lex";
            var name2 = "Lex";
            var player1 = ActorOfAsTestActorRef<PlayerActor>(PlayerActor.Props(name1, 1));
            var player2 = ActorOfAsTestActorRef<PlayerActor>(PlayerActor.Props(name2, 2));
            var status1 = player1.Ask(new PlayerActor.StatusRequest()).Result as PlayerActor.Status;
            var status2 = player2.Ask(new PlayerActor.StatusRequest()).Result as PlayerActor.Status;
            status1.Id.Should().NotBe(status2.Id);
        }
    }
}