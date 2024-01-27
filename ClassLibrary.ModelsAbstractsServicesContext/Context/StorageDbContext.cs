using ClassLibrary.ModelsAbstractsServicesContext.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.ModelsAbstractsServicesContext.Context;

public class StorageDbContext : DbContext
{
    private readonly string _connectionString;
    public StorageDbContext()
    {

    }

    public StorageDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Storage> Storages { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Name).IsUnique();

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Price).IsRequired();

            entity.HasOne(x => x.Category)
                .WithMany(x => x.Products);

            entity.HasOne(x => x.Storage)
                .WithMany(x => x.Products);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Name).IsUnique();

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsRequired();
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Name).IsUnique();

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsRequired();
        });
    }
}