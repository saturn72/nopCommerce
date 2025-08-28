
namespace KedemMarket.Factories.Catalog;
public interface IProductApiFactory
{
    public Task<IEnumerable<ProductInfoApiModel>> ToProductInfoApiModelAsync(IEnumerable<Product> products);
    public Task<IEnumerable<ProductSlimApiModel>> ToProductSlimApiModelAsync(IEnumerable<Product> products);
    public Task<ShoppingCartApiModel> ToShoppingCartApiModelAsync(IEnumerable<ShoppingCartItem> cart);
}
