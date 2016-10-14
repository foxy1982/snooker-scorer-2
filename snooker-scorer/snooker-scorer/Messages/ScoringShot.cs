namespace snooker_scorer.Messages
{
    public class ScoringShot
    {
        public ScoringShot(int value)
        {
            Value = value;
        }

        public int Value { get; private set; }
    }
}