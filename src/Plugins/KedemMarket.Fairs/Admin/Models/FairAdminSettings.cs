using Nop.Core.Configuration;

namespace KedemMarket.Fairs.Admin.Models;
public class FairAdminSettings : ISettings
{
    public string PageSizeOptions { get; set; } = "10, 25, 50, 100";
    public int DefaultPageSize { get; set; } = 25;
}