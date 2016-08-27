using Akka.Actor;
using Akka.TestKit.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using snooker_scorer.Actors;

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
            var player = ActorOfAsTestActorRef<PlayerActor>(PlayerActor.Props(name));
            var status = player.Ask(new PlayerActor.StatusRequest()).Result as PlayerActor.Status;
            status.Name.Should().Be(name);
        }
    }
}