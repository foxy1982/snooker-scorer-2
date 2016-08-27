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
            public readonly Player Player1;
            public readonly Player Player2;

            public StatusResponse(Guid id, Player player1, Player player2)
            {
                Id = id;
                Player1 = player1;
                Player2 = player2;
            }
        }

        public class Player
        {
            public readonly string Name;
            public readonly int Score;

            public Player(string name)
            {
                Name = name;
            }
        }

        public class StatusRequest
        {
        }
    }
}