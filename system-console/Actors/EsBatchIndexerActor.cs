using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using system_console.Messages;

namespace system_console.Actors
{
    public class EsBatchIndexerActor : UntypedActor
    {
        //to be set at config level
        private const int BatchSize = 100;

        private readonly ConcurrentQueue<EsEventRecordMessage> _recordsStore;
        private readonly IActorRef _dequeueHandler;

        public EsBatchIndexerActor()
        {
            _recordsStore = new ConcurrentQueue<EsEventRecordMessage>();
            _dequeueHandler = Context.ActorOf<SqsDequeueActor>();
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case EsEventRecordMessage msg:
                    _recordsStore.Enqueue(msg);
                    Console.WriteLine($"{Self.Path.Name}: event added to batch indexer queue");
                    Console.WriteLine($"{Self.Path.Name}: {_recordsStore.Count} records in total ready for indexing");
                    break;
                case EsIndexInBatchMessage batchCommand:
                    var batchList = new List<EsEventRecordMessage>();
                    var count = _recordsStore.Count > BatchSize ? BatchSize : _recordsStore.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if(_recordsStore.TryDequeue(out var rec))
                            batchList.Add(rec);
                    }
                    Console.WriteLine($"{Self.Path.Name}: making batch index request to ES, indexing {batchList.Count} from batch indexer queue");
                    var dequeueMessage =
                        new DequeueMessage(batchList.Select(x => x.ReceiptHandle));
                    _dequeueHandler.Tell(dequeueMessage);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
        
        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new EsBatchIndexerActor());
        }
    }
}