using ClassLibrary.ContextAndRepository.Context;
using ClassLibrary.ContextAndRepository.Models;

namespace ClassLibrary.ContextAndRepository.Repository;

public class ProductRepository : IRepository<Product>
{

    private StorageContext db;
    private bool disposed = false;

    public ProductRepository()
    {
        db = new StorageContext();
    }

    public virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
                db.Dispose();
        }
        this.disposed = true;
    }

    public IEnumerable<Product> GetAll() => db.Products.ToList();


    public Product Get(int id)
    {
        var prod = db.Products.Find(id);
        if (prod != null)
            return prod;
        return null!;
    }


    public void Create(Product product)
    {
        db.Products.Add(product);
        db.SaveChanges();
    }
    public void Update(Product product)
    {
        db.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        db.SaveChanges();
    }

    public void Delete(int id)
    {
        Product? product = db.Products.Find(id);
        if (product != null)
        {
            db.Products.Remove(product);
            db.SaveChanges();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Category FindFirstCategory(int id) => db.Categories.FirstOrDefault(x => x.ID == id)!;
}