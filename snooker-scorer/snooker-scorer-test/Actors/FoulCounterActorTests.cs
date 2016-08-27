using Akka.Actor;
using Akka.TestKit.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using snooker_scorer.Actors;

namespace snooker_scorer_test.Actors
{
    [TestFixture]
    public class FoulCounterActorTests : TestKit
    {
        [TearDown]
        public void TearDown()
        {
            Shutdown();
        }

        [Test]
        public void ShouldReturnZeroForFoulCountAtStart()
        {
            var foulCounter = ActorOfAsTestActorRef<FoulCounterActor>();
            var foulCount = foulCounter.Ask(new FoulCounterActor.FoulCountRequest()).Result as FoulCounterActor.FoulCountResponse;
            foulCount.ShouldBeEquivalentTo(new FoulCounterActor.FoulCountResponse(0, 0));
        }

        [Test]
        public void ShouldReturnFoulScoreAfterOneFoul()
        {
            IgnoreMessages(x => true);
            var foulCounter = ActorOfAsTestActorRef<FoulCounterActor>();
            foulCounter.Tell(new FoulCounterActor.Foul(4));
            IgnoreNoMessages();
            var foulCount = foulCounter.Ask(new FoulCounterActor.FoulCountRequest()).Result;
            foulCount.ShouldBeEquivalentTo(new FoulCounterActor.FoulCountResponse(1, 4));
        }

        [Test]
        public void ShouldReturnFoulScoreAfterMultipleFouls()
        {
            IgnoreMessages(x => true);
            var foulCounter = ActorOfAsTestActorRef<FoulCounterActor>();
            foulCounter.Tell(new FoulCounterActor.Foul(4));
            foulCounter.Tell(new FoulCounterActor.Foul(5));
            IgnoreNoMessages();
            var foulCount = foulCounter.Ask(new FoulCounterActor.FoulCountRequest()).Result;
            foulCount.ShouldBeEquivalentTo(new FoulCounterActor.FoulCountResponse(2, 9));
        }
    }
}