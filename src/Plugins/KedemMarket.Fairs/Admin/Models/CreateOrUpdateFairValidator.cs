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
                return fairs == null || fairs.Count() == 0 || (fairs.Count() == 1 && fairs.First().Id == f.Id);
                ;
            })
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Fair.Fields.Name.Unique"));

        var curUtc = timeProvider.GetUtcNow().UtcDateTime;
        RuleFor(x => x.StartsOnUtc)
            .Must((fi, startsOnUtc) =>
            {
                return startsOnUtc.HasValue && startsOnUtc >= curUtc;
            })
            .WithMessageAwait(async () => await localizationService.GetResourceAsync("Admin.Fair.Fields.StartsOnUtc.PastDateNotAllowed"));

        RuleFor(x => x.EndsOnUtc)
            .Must((fi, endsOnUtc) =>
            {
                return endsOnUtc.HasValue && endsOnUtc >= curUtc;
            })
            .WithMessageAwait(async () => await localizationService.GetResourceAsync("Admin.Fair.Fields.EndsOnUtc.PastDateNotAllowed"));

        RuleFor(x => x.EndsOnUtc)
            .Must((fi, endsOnUtc) =>
            {
                if (endsOnUtc.HasValue && fi.StartsOnUtc.HasValue)
                {
                    var fromUtc = fi.StartsOnUtc.Value.AddHours(fairSettings.DefaultMinimumFairLengthInHours);
                    return endsOnUtc.Value >= fromUtc;
                }
                return false;
            })
            .WithMessageAwait(async () =>
            {
                var format = await localizationService.GetResourceAsync("Admin.Fair.Fields.EndsOnUtc.MinimumLength");
                return string.Format(format, fairSettings.DefaultMinimumFairLengthInHours.ToString());
            });

        RuleFor(x => x.Published)
           .Must((fi, published) => !published || fi.EndsOnUtc.HasValue)
           .WithMessageAwait(localizationService.GetResourceAsync("Admin.Fair.Fields.Published.EndsOnUtcRequired"));
    }
}