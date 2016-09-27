using System;

namespace snooker_scorer.Web.Modules
{
    public partial class DefaultModule
    {
        public class PostGameRequest
        {
            public string Player1 { get; set; }
            public string Player2 { get; set; }
        }

        public class PostShotTakenRequest
        {
            public Guid GameId { get; set; }

            public Guid PlayerId { get; set; }

            public int Value { get; set; }
        }

        public class PostFoulCommittedRequest
        {
            public Guid GameId { get; set; }

            public Guid PlayerId { get; set; }

            public int Value { get; set; }
        }

        public class DeleteGameRequest
        {
            public Guid Id { get; set; }
        }

        public class GetGameStatusRequest
        {
            public Guid Id { get; set; }
        }
    }
}