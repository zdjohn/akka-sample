using System;
using Akka.Actor;
using system_console.Messages;

namespace system_console.Actors
{
    public class MessageForwarderActor : UntypedActor
    {
        private readonly IActorRef _batchIndexer;

        public MessageForwarderActor(IActorRef batchIndexer)
        {
            _batchIndexer = batchIndexer;
        }

        protected override void OnReceive(object message)
        {
            if (message is SortSqsBatchMessage messageList)
            {
                foreach (var m in messageList.Messages)
                {
                    // the switch can be replaced with borad cast router
                    // https://getakka.net/articles/actors/routers.html#broadcast
                    switch (m.SqsType)
                    {
                        case "typeA":
                            Context.ActorOf(TypeAEventHandlerActor.Props(_batchIndexer)).Tell(m);
                            break;
                        case "typeB":
                            Context.ActorOf(TypeBEventHandlerActor.Props(_batchIndexer)).Tell(m);
                            break;
                        case "typeS3":
                            Context.ActorOf(S3BackFillerHandlerActor.Props()).Tell(m);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
            }
        }
        
        public static Props Props(IActorRef batchIndexer)
        {
            return Akka.Actor.Props.Create(() => new MessageForwarderActor(batchIndexer));
        }
        
    }
}