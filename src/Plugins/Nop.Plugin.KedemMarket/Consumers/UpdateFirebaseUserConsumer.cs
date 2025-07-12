namespace KedemMarket.Consumers;
public class UpdateFirebaseUserConsumer :
    IConsumer<EntityDeletedEvent<CustomerCustomerRoleMapping>>,
    IConsumer<EntityUpdatedEvent<CustomerCustomerRoleMapping>>
{
    private readonly IExternalUsersService _externalUserService;
    private readonly ICustomerService _customerService;
    private readonly FirebaseAdapter _adapter;

    public UpdateFirebaseUserConsumer(
        IExternalUsersService externalUserService,
        FirebaseAdapter adapter,
        ICustomerService customerService)
    {
        _externalUserService = externalUserService;
        _adapter = adapter;
        _customerService = customerService;
    }
    public async Task HandleEventAsync(EntityDeletedEvent<CustomerCustomerRoleMapping> eventMessage)
    {
        await OnCustomerCustomerRoleMappingChangeHandlerAsync(eventMessage.Entity);
    }
    public async Task HandleEventAsync(EntityUpdatedEvent<CustomerCustomerRoleMapping> eventMessage)
    {
        await OnCustomerCustomerRoleMappingChangeHandlerAsync(eventMessage.Entity);
    }

    protected virtual async Task OnCustomerCustomerRoleMappingChangeHandlerAsync(CustomerCustomerRoleMapping map)
    {
        var userMap = await _externalUserService.GetUserIdCustomerMapByNopCustomerId(map.CustomerId);
        if (userMap == default)
            return;

        var roles = new List<string>(["registered"]);
        var customer = await _customerService.GetCustomerByIdAsync(map.CustomerId);
        if(customer == null)
            return;

        var customerRoles = (await _customerService.GetCustomerRolesAsync(customer)).Select(d => d.SystemName);
        if (customerRoles.Contains(NopCustomerDefaults.AdministratorsRoleName))
            roles.Add("admin");

        if (customerRoles.Contains(NopCustomerDefaults.VendorsRoleName))
            roles.Add("vendor");

        var claims = new Dictionary<string, object> {
            { "roles", roles},
            { "vendorId", customer.VendorId },
        };

        //outbox pattern here = add to recuring tasks queue to update claims in firebase
        await _adapter.Auth.SetCustomUserClaimsAsync(userMap.KmUserId, claims);
    }
}
