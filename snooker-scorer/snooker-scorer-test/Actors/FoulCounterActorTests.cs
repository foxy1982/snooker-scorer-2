using Akka.Actor;
using Akka.TestKit.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using snooker_scorer.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            foulCounter.Tell(new FoulCounterActor.FoulCountRequest());
            ExpectMsg<FoulCounterActor.FoulCount>().ShouldBeEquivalentTo(new FoulCounterActor.FoulCount(0, 0));
        }

        [Test]
        public void ShouldReturnFoulScoreAfterOneFoul()
        {
            IgnoreMessages(x => true);
            var foulCounter = ActorOfAsTestActorRef<FoulCounterActor>();
            foulCounter.Tell(new FoulCounterActor.Foul(4));
            IgnoreNoMessages();
            foulCounter.Tell(new FoulCounterActor.FoulCountRequest());
            ExpectMsg<FoulCounterActor.FoulCount>().ShouldBeEquivalentTo(new FoulCounterActor.FoulCount(1, 4));
        }

        [Test]
        public void ShouldReturnFoulScoreAfterMultipleFouls()
        {
            IgnoreMessages(x => true);
            var foulCounter = ActorOfAsTestActorRef<FoulCounterActor>();
            foulCounter.Tell(new FoulCounterActor.Foul(4));
            foulCounter.Tell(new FoulCounterActor.Foul(5));
            IgnoreNoMessages();
            foulCounter.Tell(new FoulCounterActor.FoulCountRequest());
            ExpectMsg<FoulCounterActor.FoulCount>().ShouldBeEquivalentTo(new FoulCounterActor.FoulCount(2, 9));
        }
    }
}