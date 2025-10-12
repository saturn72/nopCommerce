namespace KedemMarket.Services.User;
public interface IExternalUsersService
{
<<<<<<< HEAD
    Task<KmUserCustomerMap> GetUserIdCustomerMapByInternalCustomerId(int customerId);
=======
    Task<KmUserCustomerMap> GetUserIdCustomerMapByNopCustomerId(int customerId);
>>>>>>> dev/get-vendors-sales
    Task<KmUserCustomerMap> GetUserIdCustomerMapByExternalUserId(string userId);
    Task<IEnumerable<KmUserCustomerMap>> ProvisionUsersAsync(IEnumerable<string> userIds);
}
public record UpdateAddressRequest
{
    public UpdateAddressRequest(Customer customer, Address address)
    {
        Customer = customer;
        Address = address;
    }
    public Customer Customer { get; }
    public Address Address { get; }
}


