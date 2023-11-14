﻿using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;

namespace Nop.Plugin.Misc.KM.Orders.Migrations
{
    public class KmOrderInfoBuilder : NopEntityBuilder<KmOrder>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(KmOrder.Id))
                    .AsInt32()
                    .PrimaryKey()
                    .Identity()
                .WithColumn(nameof(KmOrder.CreatedOnUtc))
                    .AsDateTime2()
                    .Nullable()
                    .WithDefault(SystemMethods.CurrentUTCDateTime)

                .WithColumn(nameof(KmOrder.Data))
                    .AsString().NotNullable()

                .WithColumn(nameof(KmOrder.Status))
                    .AsString(128).NotNullable()

                .WithColumn(nameof(KmOrder.KmOrderId))
                .AsString(256).NotNullable()

                .WithColumn(nameof(KmOrder.KmUserId))
                    .AsString(256).NotNullable()

                .WithColumn(nameof(KmOrder.NopOrderId))
                    .AsInt32()
                    .NotNullable()
                .WithColumn(nameof(KmOrder.Errors))
                    .AsString()
                    .NotNullable();
        }
    }
}
