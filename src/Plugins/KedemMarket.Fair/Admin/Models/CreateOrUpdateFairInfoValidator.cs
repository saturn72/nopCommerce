using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace KedemMarket.Fair.Admin.Models;
public class CreateOrUpdateFairInfoValidator : BaseNopValidator<FairInfoAdminModel>
{
    public CreateOrUpdateFairInfoValidator(
        ILocalizationService localizationService,
        IFairService fairService,
        FairSettings fairSettings,
        TimeProvider timeProvider)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Fair.Fields.Name.Required"));

        RuleFor(x => x.Name)
            .MustAwait(async (f, ct) =>
            {
                var fairs = await fairService.GetFairInfosByNameAsync(f.Name);
                return fairs == null || fairs.Count() == 1;
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