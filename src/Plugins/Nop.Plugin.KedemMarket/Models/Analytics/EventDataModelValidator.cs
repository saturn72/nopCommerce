namespace KedemMarket.Models.Analytics;
public class EventDataModelValidator : AbstractValidator<EventDataModel>
{
    public EventDataModelValidator()
    {
        RuleFor(x => x.EventName).Must(x => x.NotNullAndNotNotEmpty());
    }
}
