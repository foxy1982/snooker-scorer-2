using Akka.Actor;
using Akka.TestKit.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using snooker_scorer.Actors;
using snooker_scorer.Messages;
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
        public void ShouldReturnZeroForCurrentBreakAtStart()
        {
            var breakCounter = ActorOfAsTestActorRef<BreakCounterActor>();
            breakCounter.Tell(new BreakCounterActor.CurrentBreakRequest());
            ExpectMsg<BreakCounterActor.CurrentBreakResponse>().ShouldBeEquivalentTo(new BreakCounterActor.CurrentBreakResponse(0));
        }

        [Test]
        public void ShouldReturnCurrentBreakAfterSingleShot()
        {
            var breakCounter = ActorOfAsTestActorRef<BreakCounterActor>();
            breakCounter.Tell(new ScoringShot(5));
            var currentBreak = breakCounter.Ask(new BreakCounterActor.CurrentBreakRequest()).Result;
            currentBreak.ShouldBeEquivalentTo(new BreakCounterActor.CurrentBreakResponse(5));
        }

        [Test]
        public void ShouldReturnCurrentBreakAfterTwoShots()
        {
            IgnoreMessages(x => true);
            var breakCounter = ActorOfAsTestActorRef<BreakCounterActor>();
            breakCounter.Tell(new ScoringShot(1));
            IgnoreNoMessages();
            breakCounter.Tell(new ScoringShot(5));
            var currentBreak = breakCounter.Ask(new BreakCounterActor.CurrentBreakRequest()).Result;
            currentBreak.ShouldBeEquivalentTo(new BreakCounterActor.CurrentBreakResponse(6));
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
            ExpectMsg<BreakCounterActor.CurrentBreakResponse>().ShouldBeEquivalentTo(new BreakCounterActor.CurrentBreakResponse(0));
        }
    }
}