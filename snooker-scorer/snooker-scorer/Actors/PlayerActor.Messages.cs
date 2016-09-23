namespace snooker_scorer.Actors
{
    public partial class PlayerActor
    {
        public class Status
        {
            public readonly string Name;
            public readonly int Score;

            public Status(string name, int score)
            {
                Name = name;
                Score = score;
            }
        }

        public class StatusRequest
        {
        }

        public class ShotTakenCommand
        {
            public readonly int Score;

            public ShotTakenCommand(int score)
            {
                Score = score;
            }
        }

        public class AwardFoulPointsCommand
        {
            public readonly int Value;

            public AwardFoulPointsCommand(int value)
            {
                Value = value;
            }
        }
    }
}