namespace KedemMarket.Cms.Factories.Pages;
public interface ICmsPageModelFactory
{
    Task<object> GetCmsPageModelEntityByTypeNameAndEntityIdAsync(string entityName, int entityId);
}
