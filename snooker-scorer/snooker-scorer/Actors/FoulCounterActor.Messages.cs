namespace snooker_scorer.Actors
{
    public partial class FoulCounterActor
    {
        public class FoulCount
        {
            public readonly int NumberOfFouls;
            public readonly int TotalPoints;

            public FoulCount(int numberOfFouls, int totalPoints)
            {
                NumberOfFouls = numberOfFouls;
                TotalPoints = totalPoints;
            }
        }

        public class FoulCountRequest
        {
            public FoulCountRequest()
            {
            }
        }

        public class Foul
        {
            public readonly int Value;

            public Foul(int value)
            {
                Value = value;
            }
        }
    }
}