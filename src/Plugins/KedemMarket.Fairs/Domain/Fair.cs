namespace KedemMarket.Fairs.Domain;
public class Fair : BaseEntity
{
    public bool IsVirtual { get; set; }
    public Address Address { get; set; }
    public string Name { get; set; }
    public int CustomerId { get; set; }
    public bool Deleted { get; set; }
    public string Description { get; set; }
    public int PictureId { get; internal set; }
    public bool Published { get; set; }
    public DateTime? StartsOnUtc { get; set; }
    public DateTime? EndsOnUtc { get; set; }
    public IList<int> AdminIds { get; set; }
    public IList<int> VendorIds { get; set; } = [];
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
}
