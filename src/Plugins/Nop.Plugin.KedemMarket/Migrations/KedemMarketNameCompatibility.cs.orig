<<<<<<< HEAD
﻿namespace KedemMarket.Migrations;
=======
﻿using KedemMarket.Domain.Agent;
using KedemMarket.Domain.Ordering;

namespace KedemMarket.Migrations;
>>>>>>> dev/get-vendors-sales

public partial class KedemMarketNameCompatibility : INameCompatibility
{
    public Dictionary<Type, string> TableNames => new()
    {
        {typeof(EventData), "km_eventdata" },

<<<<<<< HEAD
=======
        {typeof(OrderItemsStatus), "km_orderitemsstatus" },
>>>>>>> dev/get-vendors-sales
        {typeof(KmOrder), "km_order" },
        {typeof(KmUserCustomerMap), "km_usercustomermap" },

        {typeof(NavbarInfo), "km_navbarinfo" },
        {typeof(NavbarElement), "km_navbarelement" },
        {typeof(NavbarElementVendor), "km_navbarelementvendor" },
<<<<<<< HEAD
=======
        {typeof(CustomerAgentSession), "km_customeragentsession" },
        {typeof(CustomerAgentSessionMessage), "km_customeragentsessionmessage" },
>>>>>>> dev/get-vendors-sales

    };

    public Dictionary<(Type, string), string> ColumnName => new();
}