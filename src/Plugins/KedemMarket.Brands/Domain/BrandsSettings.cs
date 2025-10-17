namespace KedemMarket.Brands.Domain;
public class BrandsSettings : ISettings
{
    public bool AllowVendorsToImportBrands { get; set; } = true;
    public float MaxLogoWidth { get; set; } = 320;
    public float MaxLogoHeight { get; set; } = 160;
}
