using ClassLibrary.ModelsAbstractsServicesContext.Abstractions;
using ClassLibrary.ModelsAbstractsServicesContext.Models;
using ClassLibrary.ModelsAbstractsServicesContext.Models.DTO;
namespace ClassLibrary.ModelsAbstractsServicesContext.Query;

public class QueryClass
{
    public IEnumerable<ProductDTO> GetProducts([Service] IProductService<ProductDTO> service) => service.GetProducts();
    public IEnumerable<StorageDTO> GetStorages([Service] IStorageService<StorageDTO> service) => service.GetStorages();
    public IEnumerable<CategoryDTO> GetCategories([Service] ICategoryService<CategoryDTO> service) => service.GetCategories();
    public IEnumerable<Product> GetProductsByIDStorages([Service] IEnumerable<Storage> storages, int idStorage)
    {
        var storage = storages.FirstOrDefault(x=>x.Id== idStorage);
        return storage!.Products!;
    }
}