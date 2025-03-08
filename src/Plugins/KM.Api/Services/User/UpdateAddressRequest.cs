namespace KedemMarket.Api.Services.User;

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


