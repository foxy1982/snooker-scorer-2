namespace snooker_scorer.Actors
{
    public partial class PlayerActor
    {
        public class Status
        {
            public readonly string Name;

            public Status(string name)
            {
                Name = name;
            }
        }

        public class StatusRequest
        {
            public StatusRequest()
            {
            }
        }
    }
}