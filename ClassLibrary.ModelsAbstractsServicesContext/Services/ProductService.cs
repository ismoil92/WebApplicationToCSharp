using AutoMapper;
using ClassLibrary.ModelsAbstractsServicesContext.Abstractions;
using ClassLibrary.ModelsAbstractsServicesContext.Context;
using ClassLibrary.ModelsAbstractsServicesContext.Models;
using ClassLibrary.ModelsAbstractsServicesContext.Models.DTO;
using Microsoft.Extensions.Caching.Memory;

namespace ClassLibrary.ModelsAbstractsServicesContext.Services;

public class ProductService : IProductService<ProductDTO>
{

    private readonly StorageDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private bool disposed = false;

    public ProductService(StorageDbContext context, IMapper mapper, IMemoryCache cache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = cache;
    }

    public virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
                _context.Dispose();
        }
        this.disposed = true;
    }

    public int AddProducts(ProductDTO tproductDto)
    {
        using(_context)
        {
            var entity = _mapper.Map<Product>(tproductDto);

            _context.Products.Add(entity);
            _context.SaveChanges();
            _memoryCache.Remove("products");

            return entity.Id;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IEnumerable<ProductDTO> GetProducts()
    {
        using(_context)
        {
            if (_memoryCache.TryGetValue("products", out List<ProductDTO>? products))
                return products!;

            products = _context.Products.Select(x=>_mapper.Map<ProductDTO>(x)).ToList();

            _memoryCache.Set("products", TimeSpan.FromMinutes(30));

            return products;
        }
    }
}