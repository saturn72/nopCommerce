namespace KedemMarket.Fairs.Domain;

public class FairAddressMap : BaseEntity
{
    public int FairId { get; set; }
    public int AddressId { get; set; }
    public int DisplayOrder { get; set; }
}
