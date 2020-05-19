namespace system_console.Messages
{
    //ref: https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/SQS/TMessage.html
    public class SqsMessage
    {
        public string ReceiptHandle;
        public string SqsBody;
        //MessageAttributes
        public string SqsType;
    }
}