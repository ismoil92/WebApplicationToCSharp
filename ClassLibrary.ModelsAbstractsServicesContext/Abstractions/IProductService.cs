namespace ClassLibrary.ModelsAbstractsServicesContext.Abstractions;

public interface IProductService<T> : IDisposable where T : class
{
    IEnumerable<T> GetProducts();

    int AddProducts(T t);
}