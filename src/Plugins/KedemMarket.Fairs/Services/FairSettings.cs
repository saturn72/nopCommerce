using Nop.Core.Configuration;

namespace KedemMarket.Fairs.Services;
public class FairSettings : ISettings
{
    public string PageSizeOptions { get; set; } = "10, 25, 50, 100";
    public int DefaultPageSize { get; set; } = 25;
    public double DefaultMinimumFairLengthInHours { get; set; } = 1;
}