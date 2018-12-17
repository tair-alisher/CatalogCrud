namespace CatalogCrud.BLL.Interfaces
{
    public interface IServiceCreator
    {
        IUserService CreateUserService(string connection);
        ICatalogService CreateCatalogService(string connection);
    }
}
