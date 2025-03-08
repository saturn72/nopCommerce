
namespace KedemMarket.Services.Analytics;

public class AnalyticsService : IAnalyticsService
{
    private readonly IRepository<EventData> _eventDataRepository;

    public AnalyticsService(
        IRepository<EventData> eventDataRepository
        )
    {
        _eventDataRepository = eventDataRepository;
    }

    public async Task ReportEventAsync(EventData data)
    {
        await _eventDataRepository.InsertAsync(data);
    }
}