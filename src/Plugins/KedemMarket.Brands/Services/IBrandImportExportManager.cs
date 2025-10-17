namespace KedemMarket.Brands.Services;
public  interface IBrandImportExportManager
{
    public Task ImportFromJsonAsync(Stream stream);
}
