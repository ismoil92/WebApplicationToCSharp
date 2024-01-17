using ClassLibrary.ContextAndRepository.Models.Base;
using Microsoft.Identity.Client.Extensions.Msal;

namespace ClassLibrary.ContextAndRepository.Models;

public class Product : BaseModel
{
    public int Cost { get; set; }
    public int CategoryID { get; set; } 
    public virtual Category? Category { get; set; }
    public virtual List<Storage>? Storages { get; set; }
}