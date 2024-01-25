namespace ClassLibrary.ModelsAbstractsServicesContext.Abstractions;

public interface IStorageService<T> : IDisposable where T : class
{
    IEnumerable<T> GetStorages();
    int AddStorages(T t);
}