using ClassLibrary.ContextAndRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.ContextAndRepository.Context;

public class StorageContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Storage> Storages { get; set; }

    private string _connectionString = string.Empty;

    public StorageContext(string connectionString) => _connectionString = connectionString;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString)
            .UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");


            entity.HasKey(x => x.ID).HasName("ProductID");
            entity.HasIndex(x => x.Name).IsUnique();
            

            entity.Property(x => x.Name).HasColumnName("ProductName")
            .HasMaxLength(255).IsRequired();

            entity.Property(x => x.Description).HasColumnName("Desciption")
            .HasMaxLength(255).IsRequired();

            entity.Property(x => x.Cost).HasColumnName("Price").IsRequired();

            entity.HasOne(x => x.Category).WithMany(y => y.Products)
            .HasForeignKey(x => x.ID).HasConstraintName("CategoryToProduct");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");

            entity.HasKey(x => x.ID).HasName("CategoryID");
            entity.HasIndex(x => x.Name).IsUnique();

            entity.Property(x => x.Name).HasColumnName("CategoryName")
            .HasMaxLength(255).IsRequired();
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.ToTable("Storages");

            entity.HasKey(x => x.ID).HasName("StorageID");
            entity.HasIndex(x => x.Name).IsUnique();

            entity.Property(x => x.Name).HasColumnName("StorageName")
            .HasMaxLength(255).IsRequired();

            entity.Property(x => x.Count).HasColumnName("Count");

            entity.HasMany(x => x.Products)
            .WithMany(m => m.Storages)
            .UsingEntity(e => e.ToTable("StoragesAndProducts"));
        });
    }
}