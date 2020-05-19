using System;
using System.Linq;
using Akka.Actor;
using Akka.Util.Internal;
using system_console.Messages;

namespace system_console.Actors
{
    public class SqsDequeueActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is DequeueMessage list)
            {
                Console.WriteLine(value: $"{Self.Path}: dequeue messages with handler: {list.MessageHandlers.Join(",").ToString()}");
            }
        }
        
        
    }
}