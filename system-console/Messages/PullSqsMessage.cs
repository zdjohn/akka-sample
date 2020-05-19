using System;

namespace system_console.Messages
{
    public sealed class PullSqsMessage
    {
        public PullSqsMessage(DateTime timestamp)
        {
            Timestamp = timestamp;
        }

        public readonly DateTime Timestamp;
    }
}