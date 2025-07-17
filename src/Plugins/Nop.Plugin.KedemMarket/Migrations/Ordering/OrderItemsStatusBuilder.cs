using FluentMigrator.Builders.Create.Table;
using KedemMarket.Domain.Ordering;
using Nop.Data.Mapping.Builders;

namespace KedemMarket.Migrations.Ordering;
public partial class OrderItemsStatusBuilder : NopEntityBuilder<OrderItemsStatus>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(OrderItemsStatus.Id)).AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn(nameof(OrderItemsStatus.OrderId)).AsInt32().NotNullable()
                .ForeignKey(nameof(Order), nameof(Order.Id))
                .Indexed("IX_OrderItemsStatus_OrderId")
            .WithColumn(nameof(OrderItemsStatus.Statuses)).AsString(int.MaxValue).NotNullable();
    }
}
