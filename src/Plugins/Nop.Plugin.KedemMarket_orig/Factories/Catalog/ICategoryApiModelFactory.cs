namespace KedemMarket.Factories.Catalog;
public interface ICategoryApiModelFactory
{
    public Task<CategoryApiModel> PrepareCategoryApiModelAsync(Category category);
}
