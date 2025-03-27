using FluentValidation;
using Nop.Web.Framework.Validators;

namespace KedemMarket.Fairs.Admin.Models;
public class CreateOrUpdateFairValidator : BaseNopValidator<FairAdminModel>
{
    public CreateOrUpdateFairValidator(
        ILocalizationService localizationService,
        IFairService fairService,
        FairSettings fairSettings,
        TimeProvider timeProvider,
        IWorkContext workContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Fair.Fields.Name.Required"));

        RuleFor(x => x.Name)
            .MustAwait(async (f, ct) =>
            {
                var currentCustomer = await workContext.GetCurrentCustomerAsync();
                var fairs = await fairService.GetFairsByNameAsync(f.Name, currentCustomer.Id);
                return fairs == null || fairs.Count() == 0;
            })
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Fair.Fields.Name.Unique"));

        RuleFor(x => x.EndsOnUtc)
            .Must((fi, endsOnUtc) =>
            {

                if (endsOnUtc.HasValue)
                {
                    var startDate = fi.StartsOnUtc ?? timeProvider.GetUtcNow().UtcDateTime;
                    return endsOnUtc.Value >= startDate.AddHours(fairSettings.DefaultFairLengthInHours);
                }
                return false;
            })
            .WithMessageAwait(async () =>
            {
                var format = await localizationService.GetResourceAsync("Admin.Fair.Fields.EndsOnUtc.MinimumLength");
                return string.Format(format, fairSettings.DefaultFairLengthInHours.ToString());
            });

        RuleFor(x => x.Published)
           .Must((fi, published) => !published || fi.EndsOnUtc.HasValue)
           .WithMessageAwait(localizationService.GetResourceAsync("Admin.Fair.Fields.Published.EndsOnUtcRequired"));
    }
}