namespace snooker_scorer.Messages
{
    public class ScoringShot
    {
        public int Value { get; private set; }

        public ScoringShot(int value)
        {
            Value = value;
        }
    }
}
