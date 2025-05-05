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
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Fairs.Fields.Name.Required"));

        RuleFor(x => x.Name)
            .MustAwait(async (f, ct) =>
            {
                var currentCustomer = await workContext.GetCurrentCustomerAsync();
                var fairs = await fairService.GetFairsByNameAsync(f.Name, currentCustomer.Id);
                return fairs == null || fairs.Count() == 0 || (fairs.Count() == 1 && fairs.First().Id == f.Id);
                ;
            })
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Fairs.Fields.Name.Unique"));

        var curUtc = timeProvider.GetUtcNow().UtcDateTime;
        RuleFor(x => x.StartsOnLocalDateTime)
            .Must((fi, sonld) =>
            {
                return sonld.HasValue && sonld >= curUtc;
            })
            .WithMessageAwait(async () => await localizationService.GetResourceAsync("Admin.Fairs.Fields.StartsOnUtc.PastDateNotAllowed"));

        RuleFor(x => x.EndsOnLocalDateTime)
            .Must((fi, eold) =>
            {
                return eold.HasValue && eold >= curUtc;
            })
            .WithMessageAwait(async () => await localizationService.GetResourceAsync("Admin.Fairs.Fields.EndsOnUtc.PastDateNotAllowed"));

        RuleFor(x => x.EndsOnLocalDateTime)
            .Must((fi, endsOnUtc) =>
            {
                if (endsOnUtc.HasValue && fi.StartsOnLocalDateTime.HasValue)
                {
                    var fromUtc = fi.StartsOnLocalDateTime.Value.AddHours(fairSettings.DefaultMinimumFairLengthInHours);
                    return endsOnUtc.Value >= fromUtc;
                }
                return false;
            })
            .WithMessageAwait(async () =>
            {
                var format = await localizationService.GetResourceAsync("Admin.Fairs.Fields.EndsOnUtc.MinimumLength");
                return string.Format(format, fairSettings.DefaultMinimumFairLengthInHours.ToString());
            });

        RuleFor(x => x.Published)
           .Must((fi, published) => !published || fi.EndsOnLocalDateTime.HasValue)
           .WithMessageAwait(localizationService.GetResourceAsync("Admin.Fairs.Fields.Published.EndsOnUtcRequired"));
    }
}