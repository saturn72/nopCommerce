namespace KedemMarket.Factories.ShoppingCart;
public interface IShoppingCartFactory
{
    public Task<CreateOrderRequest> ToCreateOrderRequest(CartTransactionApiModel model, List<string> errors);
    public Task<IList<ShoppingCartItem>> ToShoppingCartItems(IEnumerable<ShoppingCartItemApiModel> items, List<string> errors);
    public Task<CheckoutCartApiModel> PrepareCheckoutCartApiModelAsync(IList<ShoppingCartItem> cart);
}
