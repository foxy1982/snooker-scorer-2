using Akka.Actor;
using Akka.TestKit.NUnit3;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snooker_scorer_test.Actors
{
    public class StatusCheck { }

    public class Parent : ReceiveActor
    {
        private readonly IList<IActorRef> _children;

        public Parent(Props child1Props, Props child2Props)
        {
            _children = new List<IActorRef> { Context.ActorOf(child1Props), Context.ActorOf(child2Props) };
            Receive<StatusCheck>(msg => HandleStatusCheck());
        }

        public Parent(IActorRef child1, IActorRef child2)
        {
            _children = new List<IActorRef> { child1, child2 };
            Receive<StatusCheck>(msg => HandleStatusCheck());
        }

        private void HandleStatusCheck()
        {
            var sender = Sender;

            var task = Task.Run(async () =>
            {
                var tasks = _children.Select(x => x.Ask(new StatusCheck())).ToList();

                await Task.WhenAll(tasks);

                var playerInfos = tasks.Select(x => x.Result as string);

                return string.Join(",", playerInfos);
            }).PipeTo(sender, Self);
        }
    }

    public class Child : ReceiveActor
    {
        public Child()
        {
            Receive<StatusCheck>(msg => Sender.Tell("ok"));
        }
    }

    [TestFixture]
    public class ScrapTests : TestKit
    {
        [Test]
        public void ShouldRequestStatusFromSubObjects()
        {
            var child1 = CreateTestProbe("child1");
            var child2 = CreateTestProbe("child2");

            var child1Props = Props.Create(() => new ForwarderActor(child1.Ref));
            var child2Props = Props.Create(() => new ForwarderActor(child2.Ref));

            var _target = Sys.ActorOf(Props.Create(() => new Parent(child1Props, child2Props)));

            _target.Tell(new StatusCheck());

            child1.ExpectMsg<StatusCheck>();
            child2.ExpectMsg<StatusCheck>();
            child1.Reply("testOK");
            child2.Reply("testOK2");

            var response = ExpectMsg<string>();
            Assert.That(response, Is.EqualTo("testOK,testOK2"));
        }
    }
}