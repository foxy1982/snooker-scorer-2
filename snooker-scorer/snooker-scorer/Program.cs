namespace snooker_scorer
{
    using System;
    using Nancy.Hosting.Self;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Bootstrapper.InitializeActorSystem();

            using (var host = new NancyHost(new Uri("http://localhost:1147")))
            {
                host.Start();
                Console.WriteLine("Started");
                Console.ReadLine();
            }
        }
    }
}