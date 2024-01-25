namespace ClassLibrary.ModelsAbstractsServicesContext.Abstractions;

public interface ICategoryService<T> : IDisposable where T : class
{
    IEnumerable<T> GetCategories();
    int AddCategories(T t);
}