using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snooker_scorer.Actors
{
    public partial class GameActor
    {
        public class StatusResponse
        {
            public readonly Guid Id;
            public IEnumerable<Player> Players;

            public StatusResponse(Guid id, IEnumerable<Player> players)
            {
                Id = id;
                Players = players;
            }
        }

        public class Player
        {
            public readonly Guid Id;
            public readonly string Name;
            public readonly int Score;

            public Player(Guid id, string name, int score)
            {
                Id = id;
                Name = name;
                Score = score;
            }
        }

        public class StatusRequest
        {
        }

        public class ShotTakenCommand
        {
            public readonly Guid PlayerId;
            public readonly int Score;

            public ShotTakenCommand(Guid playerId, int score)
            {
                Score = score;
                PlayerId = playerId;
            }
        }

        public class FoulCommittedCommand
        {
            public readonly Guid Id;
            public readonly int Value;

            public FoulCommittedCommand(Guid id, int value)
            {
                Id = id;
                Value = value;
            }
        }
    }
}