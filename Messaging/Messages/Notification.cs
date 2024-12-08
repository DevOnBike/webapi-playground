namespace Messaging.Messages
{
    [Serializable]
    public sealed class Notification
    {
        public string Sender { get; }
        
        public string Message { get; }
        
        public Notification(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }

        public override string ToString()
        {
            return $"sender: {Sender}, message: {Message}";
        }
    }
}
