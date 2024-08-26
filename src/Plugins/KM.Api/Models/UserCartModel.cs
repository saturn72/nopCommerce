﻿using Nop.Core.Domain.Orders;
using System;
using System.Collections.Generic;

namespace KM.Api.Models
{
    public record UserCartModel
    {
        public IEnumerable<UserCartProductModel> Items { get; set; }
        public IEnumerable<string> CouponCodes { get; set; }
        public string UserId { get; set; }
        public int StoreId { get; set; }
        public IList<UserCartDiscountUsageModel> AppliedDiscounts { get; set; } = new List<UserCartDiscountUsageModel>();
        public IList<string> Errors { get; set; } = new List<string>();

        public record UserCartProductModel
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public DateTime CreatedOnUtc { get; set; }
            public int Quantity { get; set; }
            public string Sku { get; set; }
            public int VendorId { get; set; }
            public string VendorName { get; set; }
            public string ShortDescription { get; set; }
            public IEnumerable<object> AllowedQuantities { get; set; }
            public string RecurringInfo { get; set; }
            public DateTime? RentalStartDateUtc { get; init; }
            public DateTime? RentalEndDateUtc { get; init; }
        }
    }
}
