using Akka.Actor;
using Akka.TestKit.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using snooker_scorer.Actors;
using snooker_scorer.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snooker_scorer_test.Actors
{
    [TestFixture]
    public class BreakCounterActorTests : TestKit
    {
        [TearDown]
        public void TearDown()
        {
            Shutdown();
        }

        [Test]
        public void ShouldReturnZeroForCurrentBreakAfterNoShot()
        {
            var breakCounter = Sys.ActorOf(Props.Create(() => new BreakCounterActor()));
            breakCounter.Tell(new BreakCounterActor.CurrentBreakRequest());
            ExpectMsg<BreakCounterActor.CurrentBreak>().ShouldBeEquivalentTo(new BreakCounterActor.CurrentBreak(0));
        }

        [Test]
        public void ShouldReturnCurrentBreakAfterSingleShot()
        {
            var breakCounter = Sys.ActorOf(Props.Create(() => new BreakCounterActor()));
            breakCounter.Tell(new ScoringShot(5));
            ExpectMsg<BreakCounterActor.CurrentBreak>().ShouldBeEquivalentTo(new BreakCounterActor.CurrentBreak(5));
            breakCounter.Tell(new BreakCounterActor.CurrentBreakRequest());
            ExpectMsg<BreakCounterActor.CurrentBreak>().ShouldBeEquivalentTo(new BreakCounterActor.CurrentBreak(5));
        }

        [Test]
        public void ShouldReturnCurrentBreakAfterTwoShots()
        {
            IgnoreMessages(x => true);
            var breakCounter = ActorOfAsTestActorRef<BreakCounterActor>();
            breakCounter.Tell(new ScoringShot(1));
            IgnoreNoMessages();
            breakCounter.Tell(new ScoringShot(5));
            ExpectMsg<BreakCounterActor.CurrentBreak>().ShouldBeEquivalentTo(new BreakCounterActor.CurrentBreak(6));
            breakCounter.Tell(new BreakCounterActor.CurrentBreakRequest());
            ExpectMsg<BreakCounterActor.CurrentBreak>().ShouldBeEquivalentTo(new BreakCounterActor.CurrentBreak(6));
        }

        [Test]
        public void ShouldResetCurrentBreakToZeroWhenRequested()
        {
            IgnoreMessages(x => true);
            var breakCounter = ActorOfAsTestActorRef<BreakCounterActor>();
            breakCounter.Tell(new ScoringShot(1));
            IgnoreNoMessages();
            breakCounter.Tell(new BreakCounterActor.EndOfBreak());
            ExpectNoMsg();
            breakCounter.Tell(new BreakCounterActor.CurrentBreakRequest());
            ExpectMsg<BreakCounterActor.CurrentBreak>().ShouldBeEquivalentTo(new BreakCounterActor.CurrentBreak(0));
        }
    }
}