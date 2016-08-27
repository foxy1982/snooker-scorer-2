using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snooker_scorer.Actors
{
    public partial class BreakCounterActor
    {
        public class CurrentBreakResponse
        {
            public readonly int Value;

            public CurrentBreakResponse(int value)
            {
                Value = value;
            }
        }

        public class CurrentBreakRequest
        {
            public CurrentBreakRequest()
            {
            }
        }

        public class EndOfBreak
        {
            public EndOfBreak()
            {
            }
        }
    }
}