using ClassLibrary.ContextAndRepository.Context;
using ClassLibrary.ContextAndRepository.Models;

namespace ClassLibrary.ContextAndRepository.Repository;

public class CategoryRepository : IRepository<Category>
{
    private StorageContext db;
    private bool disposed = false;

    public CategoryRepository() => db = new StorageContext();

    public virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
                db.Dispose();
        }
        this.disposed = true;
    }

    public IEnumerable<Category> GetAll() => db.Categories.ToList();

    public Category Get(int id)
    {
        Category? category = db.Categories.FirstOrDefault();
        if (category != null)
            return category;
        return null!;
    }

    public void Create(Category category)
    {
        db.Categories.Add(category);
        db.SaveChanges();
    }

    public void Update(Category category)
    {
        db.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        db.SaveChanges();
    }

    public void Delete(int id)
    {
        Category? category = db.Categories.FirstOrDefault(x => x.ID == id);
        if (category != null)
        {
            db.Categories.Remove(category);
            db.SaveChanges();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Category FindFirstCategory(int id)
    {
        throw new NotImplementedException();
    }
}