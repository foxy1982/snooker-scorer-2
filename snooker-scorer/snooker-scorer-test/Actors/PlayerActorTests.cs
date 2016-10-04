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
            var id = Guid.NewGuid();
            var name = "Lex";
            var player = ActorOfAsTestActorRef<PlayerActor>(PlayerActor.Props(id, name, 1));
            var status = player.Ask(new PlayerActor.StatusRequest()).Result as PlayerActor.Status;
            status.Name.Should().Be(name);
            status.Id.Should().Be(id);
            status.Score.Should().Be(0);
            status.Fouls.Count.Should().Be(0);
            status.Fouls.Value.Should().Be(0);
        }

        [Test]
        public void ShouldAddFoulsOnWhenFoulCommitted()
        {
            var id = Guid.NewGuid();
            var name = "Lex";
            var player = ActorOfAsTestActorRef<PlayerActor>(PlayerActor.Props(id, name, 1));
            player.Tell(new PlayerActor.FoulCommittedCommand(5));
            var status = player.Ask(new PlayerActor.StatusRequest()).Result as PlayerActor.Status;
            status.Name.Should().Be(name);
            status.Id.Should().Be(id);
            status.Score.Should().Be(0);
            status.Fouls.Count.Should().Be(1);
            status.Fouls.Value.Should().Be(5);
        }

        [Test]
        public void ShouldNotAddFoulsOnWhenFoulPointsAwarded()
        {
            var id = Guid.NewGuid();
            var name = "Lex";
            var player = ActorOfAsTestActorRef<PlayerActor>(PlayerActor.Props(id, name, 1));
            player.Tell(new PlayerActor.AwardFoulPointsCommand(5));
            var status = player.Ask(new PlayerActor.StatusRequest()).Result as PlayerActor.Status;
            status.Name.Should().Be(name);
            status.Id.Should().Be(id);
            status.Score.Should().Be(5);
            status.Fouls.Count.Should().Be(0);
            status.Fouls.Value.Should().Be(0);
        }
    }
}