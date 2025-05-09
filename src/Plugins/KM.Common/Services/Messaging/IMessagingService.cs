namespace KM.Common.Services.Messaging;
public interface IMessagingService
{
    public Task SendMessageAsync(string topic, string message);
}
