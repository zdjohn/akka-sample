using System;
using System.Threading;
using Akka.Actor;
using Akka.Routing;
using system_console.Actors;
using system_console.Messages;

namespace system_console
{
    class Program
    {
        
        
        static void Main(string[] args)
        {
            Console.WriteLine("Service Starting");
            using (ActorSystem system = ActorSystem.Create("MySystem"))
            {
                var batchIndexer = system.ActorOf<EsBatchIndexerActor>("batch-indexer");

                var sqsListener = system.ActorOf(SqsListenerActor.Props(batchIndexer).WithRouter(new RoundRobinPool(5)), "sqs-listener");
                
                //we can use for loop or scheduler to trigger message pull
                // system
                //     .Scheduler
                //     .ScheduleTellRepeatedly(TimeSpan.FromSeconds(0),
                //         TimeSpan.FromSeconds(5),
                //         batchIndexer, new EsIndexInBatchMessage(DateTime.Now), ActorRefs.NoSender);
                
                sqsListener.Tell(new PullSqsMessage(DateTime.Now));
                Thread.Sleep(5000);
                batchIndexer.Tell(new EsIndexInBatchMessage(DateTime.Now));
            }
        }
    }
}