using System;
using Akka.Actor;
using system_console.Messages;

namespace system_console.Actors
{
    public class TypeBEventHandlerActor : UntypedActor
    {
        private readonly IActorRef _batchIndexer;

        public TypeBEventHandlerActor(IActorRef batchIndexer)
        {
            _batchIndexer = batchIndexer;
        }

        protected override void OnReceive(object message)
        {
            if (message is SqsMessage m)
            {
                Console.WriteLine($"{Self.Path}: handling type b logic, with its payload body: {m.SqsBody}, then mapping it into ec-doc schema");
                _batchIndexer.Tell(new EsEventRecordMessage()
                    {Doc = $"mapped {m.SqsBody}", ReceiptHandle = m.ReceiptHandle});
            }
        }
        
        public static Props Props(IActorRef batchIndexer)
        {
            return Akka.Actor.Props.Create(() => new TypeBEventHandlerActor(batchIndexer));
        }
    }
}