using FluentValidation;
using Nop.Web.Framework.Validators;

namespace KedemMarket.Fairs.Admin.Models.FairVendor;

public class CreateOrUpdateFairVendorModelValidator : BaseNopValidator<CreateOrUpdateFairVendorModel>
{
    public CreateOrUpdateFairVendorModelValidator(ILocalizationService localizationService)
    {

        RuleFor(x => x.FairId)
            .GreaterThan(0)
                .WithMessageAwait(localizationService.GetResourceAsync("Admin.Fairs.Fields.Id.Invalid"));

        RuleFor(x => x.VendorId)
            .GreaterThan(0)
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Fairs.Fields.Id.Invalid"));
    }
}