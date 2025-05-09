namespace KM.Common.Services.Messaging;

public class MessagingService : IMessagingService
{
    public Task SendMessageAsync(string topic, string message)
    {
        return Task.CompletedTask;
    }
}
