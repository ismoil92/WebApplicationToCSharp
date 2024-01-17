using ClassLibrary.ContextAndRepository.Models;

namespace ClassLibrary.ContextAndRepository.Repository;

public interface IRepository<T> : IDisposable where T : class
{
    IEnumerable<T> GetAll();
    T Get(int id);
    void Create(T t);
    void Update(T t);
    void Delete(int id);
    Category FindFirstCategory(int id);
}