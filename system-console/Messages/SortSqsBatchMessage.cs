using System.Collections.Generic;

namespace system_console.Messages
{
    public sealed class SortSqsBatchMessage
    {
        public SortSqsBatchMessage(List<SqsMessage> messages)
        {
            Messages = messages;
        }

        public List<SqsMessage> Messages;
    }
}