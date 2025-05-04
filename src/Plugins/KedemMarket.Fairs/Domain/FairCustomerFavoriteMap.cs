namespace KedemMarket.Fairs.Domain;

public class FairCustomerFavoriteMap: BaseEntity
{
    public int FairId { get; set; }
    public int CustomerId { get; set; }
}