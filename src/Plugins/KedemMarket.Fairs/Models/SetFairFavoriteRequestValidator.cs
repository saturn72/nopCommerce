using FluentValidation;

namespace KedemMarket.Fairs.Models;

public class SetFairFavoriteRequestValidator : AbstractValidator<SetFairFavoriteRequest>
{
    public SetFairFavoriteRequestValidator()
    {
        RuleFor(x => x.FairId).GreaterThan(0).WithMessage("FairId is required.");
    }
}
