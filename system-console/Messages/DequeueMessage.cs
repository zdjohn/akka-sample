using System.Collections.Generic;

namespace system_console.Messages
{
    public sealed class DequeueMessage
    {
        public DequeueMessage(IEnumerable<string> messageHandlers)
        {
            MessageHandlers = messageHandlers;
        }

        public readonly IEnumerable<string> MessageHandlers;
    }
}