using System;
using System.Collections.Generic;
using Akka.Actor;
using system_console.Messages;

namespace system_console.Actors
{
    public class SqsListenerActor : UntypedActor
    {
        private readonly IActorRef _batchIndexer;
        private readonly IActorRef _messageForwarder;

        public SqsListenerActor(IActorRef batchIndexer)
        {
            _batchIndexer = batchIndexer;
            _messageForwarder = Context.ActorOf(MessageForwarderActor.Props(_batchIndexer), "message-forwarder");
        }
        
        protected override void OnReceive(object message)
        {
            if (message is PullSqsMessage)
            {
                Console.WriteLine($"{Self.Path}: Pulling X messages from sqs");
                // hard coded demo messages
                var sqsList = new List<SqsMessage>()
                {
                    new SqsMessage() { ReceiptHandle = "Handle-a", SqsBody = "something for a-1", SqsType = "typeA"},
                    new SqsMessage() { ReceiptHandle = "Handle-b", SqsBody = "something for b-1", SqsType = "typeB"},
                    new SqsMessage() { ReceiptHandle = "Handle-c", SqsBody = "something for a-2", SqsType = "typeA"},
                    new SqsMessage() { ReceiptHandle = "Handle-d", SqsBody = "something for b-2", SqsType = "typeB"},
                    new SqsMessage() { ReceiptHandle = "Handle-s", SqsBody = "something for s", SqsType = "typeS3"},
                };
                _messageForwarder.Tell(new SortSqsBatchMessage(sqsList));
            }
        }
        
        public static Props Props(IActorRef batchIndexer)
        {
            return Akka.Actor.Props.Create(() => new SqsListenerActor(batchIndexer));
        }
    }
}