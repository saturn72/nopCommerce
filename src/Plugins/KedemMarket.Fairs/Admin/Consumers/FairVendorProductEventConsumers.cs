using KM.Common.Services.Messaging;
using Nop.Core.Events;

namespace KedemMarket.Fairs.Admin.Consumers;
public class FairVendorProductEventConsumers :
    IConsumer<EntityInsertedEvent<FairVendorProductMap>>,
    IConsumer<EntityUpdatedEvent<FairVendorProductMap>>,
    IConsumer<EntityDeletedEvent<FairVendorProductMap>>
{
    private readonly IMessagingService _messagingService;

    public FairVendorProductEventConsumers(IMessagingService messagingService)
    {
        _messagingService = messagingService;
    }

    public async Task HandleEventAsync(EntityInsertedEvent<FairVendorProductMap> eventMessage)
    {
        await PublishMessageToAdmin(eventMessage.Entity, "New product added to vendor");
    }

    public async Task HandleEventAsync(EntityUpdatedEvent<FairVendorProductMap> eventMessage)
    {
        await PublishMessageToAdmin(eventMessage.Entity, "New product added to vendor");
    }

    public async Task HandleEventAsync(EntityDeletedEvent<FairVendorProductMap> eventMessage)
    {
        await PublishMessageToAdmin(eventMessage.Entity, "New product added to vendor");
    }

    private async Task PublishMessageToAdmin(FairVendorProductMap map, string template)
    {
        if (map.PendingApproval)
            await _messagingService.SendMessageAsync("fair-vendor-product", "New product added to vendor");
    }
}
