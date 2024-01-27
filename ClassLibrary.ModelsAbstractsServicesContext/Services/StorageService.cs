using AutoMapper;
using ClassLibrary.ModelsAbstractsServicesContext.Abstractions;
using ClassLibrary.ModelsAbstractsServicesContext.Context;
using ClassLibrary.ModelsAbstractsServicesContext.Models;
using ClassLibrary.ModelsAbstractsServicesContext.Models.DTO;
using Microsoft.Extensions.Caching.Memory;

namespace ClassLibrary.ModelsAbstractsServicesContext.Services;

public class StorageService : IStorageService<StorageDTO>
{
    private readonly StorageDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private bool _disposed;

    public StorageService(StorageDbContext context, IMapper mapper, IMemoryCache memoryCache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
                _context.Dispose();
        }
        this._disposed = true;
    }

    public int AddStorages(StorageDTO storageDTO)
    {
        using (_context)
        {
            var entity = _mapper.Map<Storage>(storageDTO);

            _context.Storages.Add(entity);
            _context.SaveChanges();

            _memoryCache.Remove("storages");

            return entity.Id;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IEnumerable<StorageDTO> GetStorages()
    {
        using (_context)
        {
            if (_memoryCache.TryGetValue("storages", out List<StorageDTO>? storages))
                return storages!;

            storages = _context.Storages.Select(x => _mapper.Map<StorageDTO>(x)).ToList();

            _memoryCache.Set("storages", TimeSpan.FromMinutes(30));

            return storages;
        }
    }
}