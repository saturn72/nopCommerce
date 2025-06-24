namespace KedemMarket.Factories.Pages;
public interface IEntityToModelFactory
{
    Task<object> GetModelEntityByTypeNameAndEntityId(string entityName, int entityId);
}
