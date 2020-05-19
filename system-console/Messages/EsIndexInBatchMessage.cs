using System;

namespace system_console.Messages
{
    public sealed class EsIndexInBatchMessage
    {
        public EsIndexInBatchMessage(DateTime timestamp)
        {
            Timestamp = timestamp;
        }

        public DateTime Timestamp;
    }
}