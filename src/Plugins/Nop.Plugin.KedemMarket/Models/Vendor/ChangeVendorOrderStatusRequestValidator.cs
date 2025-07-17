using Nop.Web.Framework.Validators;

namespace KedemMarket.Models.Vendor;

public class ChangeVendorOrderStatusRequestValidator : AbstractValidator<ChangeVendorOrderStatusRequest>
{
    public ChangeVendorOrderStatusRequestValidator(IOrderService orderService)
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0).WithMessage("Order ID must be greater than 0.")
            .MustAwait(async (o, cancellation) =>
            {
                var order = await orderService.GetOrderByIdAsync(o.OrderId);
                return order != null;
            }).WithMessage("Order not found.");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Order status must not be empty.")
            .NotNull()
            .WithMessage("Order status must not be null.")
            .Must((r, v) => KmConsts.OrderStatuses.All.Contains(r.Status, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Invalid order status provided.");
    }
}
