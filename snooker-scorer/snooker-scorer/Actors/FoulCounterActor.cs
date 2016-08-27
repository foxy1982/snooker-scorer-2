using Akka.Actor;
using System;
using System.Threading.Tasks;

namespace snooker_scorer.Actors
{
    public partial class FoulCounterActor : ReceiveActor
    {
        private int _foulCount;
        private int _foulValueCount;

        public FoulCounterActor()
        {
            Receive<FoulCountRequest>(msg => HandleFoulCountRequest(msg));
            Receive<Foul>(msg => HandleFoul(msg));
        }

        private void HandleFoul(Foul msg)
        {
            _foulCount++;
            _foulValueCount += msg.Value;
        }

        private void HandleFoulCountRequest(FoulCountRequest msg)
        {
            Sender.Tell(new FoulCountResponse(_foulCount, _foulValueCount));
        }
    }
}