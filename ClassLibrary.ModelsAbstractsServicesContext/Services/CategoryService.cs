using AutoMapper;
using ClassLibrary.ModelsAbstractsServicesContext.Abstractions;
using ClassLibrary.ModelsAbstractsServicesContext.Context;
using ClassLibrary.ModelsAbstractsServicesContext.Models;
using ClassLibrary.ModelsAbstractsServicesContext.Models.DTO;
using Microsoft.Extensions.Caching.Memory;

namespace ClassLibrary.ModelsAbstractsServicesContext.Services;

public class CategoryService : ICategoryService<CategoryDTO>
{
    private readonly StorageDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    private bool disposed = false;

    public virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
                _context.Dispose();
        }
        this.disposed = true;
    }

    public CategoryService(StorageDbContext context, IMapper mapper, IMemoryCache memoryCache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public int AddCategories(CategoryDTO category)
    {
        using(_context)
        {
            var entity = _mapper.Map<Category>(category);

            _context.Categories.Add(entity);
            _context.SaveChanges();

            _memoryCache.Remove("categories");

            return entity.Id;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IEnumerable<CategoryDTO> GetCategories()
    {
        using(_context)
        {
            if (_memoryCache.TryGetValue("categories", out List<CategoryDTO>? categories))
                return categories!;

            categories = _context.Categories.Select(x=> _mapper.Map<CategoryDTO>(x)).ToList();

            _memoryCache.Set("categories", TimeSpan.FromMinutes(30));

            return categories;
        }
    }
}