namespace KedemMarket.Migrations;

public partial class KedemMarketNameCompatibility : INameCompatibility
{
    public Dictionary<Type, string> TableNames => new()
    {
        {typeof(EventData), "km_eventdata" },

        {typeof(KmOrder), "km_order" },
        {typeof(KmUserCustomerMap), "km_usercustomermap" },

        {typeof(NavbarInfo), "km_navbarinfo" },
        {typeof(NavbarElement), "km_navbarelement" },
        {typeof(NavbarElementVendor), "km_navbarelementvendor" },

    };

    public Dictionary<(Type, string), string> ColumnName => new();
}