using Akka.Actor;
using Akka.TestKit.NUnit3;
using NUnit.Framework;
using snooker_scorer.Messages;
using snooker_scorer.Actors;
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
        [Test]
        public void BreakCounterActor_should_save_first_shot()
        {
            var breakCounter = Sys.ActorOf(Props.Create(() => new BreakCounterActor()));
            breakCounter.Tell(new ScoringShot(5));
            Assert.True(ExpectMsg<BreakCounterActor.CurrentBreak>().Value == 5);
        }
    }
}
