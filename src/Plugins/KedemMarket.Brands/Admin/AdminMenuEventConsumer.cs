using KedemMarket.Brands.Admin.Controllers;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace KedemMarket.Brands.Admin;
public class AdminMenuEventConsumer : IConsumer<AdminMenuCreatedEvent>
{
    public Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
    {
        var c = eventMessage.RootMenuItem.ChildNodes.FirstOrDefault(x => x.SystemName.Equals("Catalog", StringComparison.OrdinalIgnoreCase));

        c.InsertAfter("Manufacturers",
           new AdminMenuItem
           {
               SystemName = "Brand",
               Title = "Brands",
               Url = eventMessage.GetMenuItemUrl("Brand", nameof(BrandController.Index)),
               IconClass = "fa-regular fa-copyright",
               Visible = true,
           });

        return Task.CompletedTask;
    }
}
