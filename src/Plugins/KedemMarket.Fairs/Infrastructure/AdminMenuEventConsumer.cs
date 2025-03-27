using KedemMarket.Fairs.Admin.Controllers;

namespace KedemMarket.Fairs.Infrastructure;
public class AdminMenuEventConsumer : IConsumer<AdminMenuCreatedEvent>
{
    public Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
    {
        eventMessage.RootMenuItem.InsertBefore("Promotions",
            new AdminMenuItem
            {
                SystemName = "Fairs",
                Title = "Fairs",
                Url = eventMessage.GetMenuItemUrl("Fair", nameof(FairController.Index)),
                IconClass = "fa-solid fa-guitar",
                Visible = true,
            });
        return Task.CompletedTask;
    }
}
