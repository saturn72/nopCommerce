using Nop.Core.Domain.Seo;

namespace KedemMarket.Brands.Domain;
public class Brand : BaseEntity, ISlugSupported
{
    public DateTime CreatedOnUtc { get; set; }
    public string Comment { get; set; }
    public int DisplayOrder { get; set; }
    public int LogoImageId { get; set; }
    public string Name { get; set; }
    public DateTime UpdatedOnUtc { get; set; }
}
