using KedemMarket.Fairs.Admin.Controllers;

namespace KedemMarket.Fairs.Infrastructure;
public class AdminMenuEventConsumer : IConsumer<AdminMenuCreatedEvent>
{
    public Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
    {
        var ami = new AdminMenuItem
        {
            SystemName = "Fairs",
            Title = "Fairs",
            //Url = eventMessage.GetMenuItemUrl("Fair", nameof(FairController.Index)),
            IconClass = "fas fa-guitar",
            ChildNodes = new List<AdminMenuItem>{
                new (){
                    SystemName = "Fairs",
                    Title = "Fair List",
                    Url = eventMessage.GetMenuItemUrl("Fair", nameof(FairController.Index)),
                    PermissionNames = new List<string> { FairPermissions.ADMIN_VIEW},
                    IconClass = "far fa-dot-circle",
                },
            }
        };

        eventMessage.RootMenuItem.InsertBefore("Promotions", ami);
        return Task.CompletedTask;
    }



}
