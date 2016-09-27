using System;

namespace snooker_scorer.Actors
{
    public partial class PlayerActor
    {
        public class Status
        {
            public readonly Guid Id;
            public readonly string Name;
            public readonly int PlayerNumber;
            public readonly int Score;

            public Status(Guid id, string name, int playerNumber, int score)
            {
                Id = id;
                Name = name;
                PlayerNumber = playerNumber;
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