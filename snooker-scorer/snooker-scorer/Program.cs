namespace snooker_scorer
{
    using System;
    using Mono.Unix;
    using Mono.Unix.Native;
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

                if (IsRunningOnMono())
                {
                    var terminationSignals = GetUnixTerminationSignals();
                    UnixSignal.WaitAny(terminationSignals);
                }
                else
                {
                    Console.ReadLine();
                }

                host.Stop();
            }
        }

        private static UnixSignal[] GetUnixTerminationSignals()
        {
            return new[]
            {
                new UnixSignal(Signum.SIGINT),
                new UnixSignal(Signum.SIGTERM),
                new UnixSignal(Signum.SIGQUIT),
                new UnixSignal(Signum.SIGHUP)
            };
        }

        private static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}