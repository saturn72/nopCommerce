namespace KedemMarket.Api.Services.User;
public interface IExternalUsersService
{
    Task<KmUserCustomerMap> GetUserIdCustomerMapByInternalCustomerId(int customerId);
    Task<KmUserCustomerMap> GetUserIdCustomerMapByExternalUserId(string userId);
    Task<IEnumerable<KmUserCustomerMap>> ProvisionUsersAsync(IEnumerable<string> userIds);
}


