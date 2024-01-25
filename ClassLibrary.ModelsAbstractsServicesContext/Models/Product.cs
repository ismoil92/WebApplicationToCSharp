namespace ClassLibrary.ModelsAbstractsServicesContext.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public uint Cost { get; set; }
    public decimal Price { get; set; }
    public virtual Category? Category { get; set; }
    public virtual Storage? Storage { get; set; }
}