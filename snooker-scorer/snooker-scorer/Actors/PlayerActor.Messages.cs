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
            public readonly FoulCount Fouls;

            public Status(Guid id, string name, int playerNumber, int score, FoulCount fouls)
            {
                Id = id;
                Name = name;
                PlayerNumber = playerNumber;
                Score = score;
                Fouls = fouls;
            }

            public class FoulCount
            {
                public readonly int Count;
                public readonly int Value;
                
                public FoulCount(int count, int value)
                {
                    Count = count;
                    Value = value;
                }
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

        public class FoulCommittedCommand
        {
            public readonly int Value;

            public FoulCommittedCommand(int value)
            {
                Value = value;
            }
        }
    }
}