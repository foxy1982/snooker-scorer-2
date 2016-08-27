namespace snooker_scorer.Actors
{
    public partial class FoulCounterActor
    {
        public class FoulCountResponse
        {
            public readonly int NumberOfFouls;
            public readonly int TotalPoints;

            public FoulCountResponse(int numberOfFouls, int totalPoints)
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