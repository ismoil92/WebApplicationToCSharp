using ClassLibrary.ModelsAbstractsServicesContext.Abstractions;
using ClassLibrary.ModelsAbstractsServicesContext.Models.DTO;

namespace ClassLibrary.ModelsAbstractsServicesContext.Mutation;

public class MutationClass
{
    public int AddProduct(ProductDTO product, [Service] IProductService<ProductDTO> service)
    {
        var id = service.AddProducts(product);
        return id;
    }
}