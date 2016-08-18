using Nancy.Hosting.Self;
using System;

namespace snooker_scorer
{
    class Program
    {
        static void Main(string[] args)
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
