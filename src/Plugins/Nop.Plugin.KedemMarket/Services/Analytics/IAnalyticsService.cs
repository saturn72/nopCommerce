
namespace KedemMarket.Services.Analytics;
public interface IAnalyticsService
{
    Task ReportEventAsync(EventData data);
}
