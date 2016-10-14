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
        }

        public class EndOfBreak
        {
        }
    }
}