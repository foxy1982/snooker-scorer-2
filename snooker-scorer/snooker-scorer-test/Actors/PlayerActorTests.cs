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
            player.Tell(new PlayerActor.StatusRequest());
            var response = ExpectMsg<PlayerActor.Status>();
            response.Name.Should().Be(name);
        }
    }
}