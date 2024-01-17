using ClassLibrary.ContextAndRepository.Models.Base;

namespace ClassLibrary.ContextAndRepository.Models;

public class Category : BaseModel
{
    public virtual List<Product>? Products { get; set; }
}