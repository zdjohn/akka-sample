using System;
using Akka.Actor;
using system_console.Messages;

namespace system_console.Actors
{
    public class S3BackFillerHandlerActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is SqsMessage m)
            {
                Console.WriteLine($"{Self.Path}: {m.SqsBody} handle type s3 logic");
                Console.WriteLine($"{Self.Path}: unpack messages from s3, then send handle the message type from there");
            }
        }
        
        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new S3BackFillerHandlerActor());
        }
        
    }
}