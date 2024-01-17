using ClassLibrary.ContextAndRepository.Models;
using ClassLibrary.ContextAndRepository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationToCSharp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private IRepository<Product> _productRepository;

    public ProductController()
    {
        _productRepository = new ProductRepository();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync() => 
        await Task.Run(() => Ok(_productRepository.GetAll()));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductAsync([FromBody]int id) =>
        await Task.Run(() => Ok(_productRepository.Get(id)));

    [HttpPost]
    public async Task<ActionResult<Product>> AddProductAsync([FromBody] Product product, [FromQuery] int categoryID)
    {
        if (product == null && product!.ID < 0)
            return await Task.Run(() => BadRequest());
        else
        {
           var category = _productRepository.FindFirstCategory(categoryID);
            product.Category = category;
            _productRepository.Create(product);
            return await Task.Run(() => Ok(product));
        }
    }

    [HttpPut]
    public async Task<ActionResult<Product>> UpdateProductAsync([FromBody] Product product, [FromQuery] int categoryID)
    {
        if (product == null && product!.ID < 0)
            return await Task.Run(() => BadRequest());
        else
        {
            var category = _productRepository.FindFirstCategory(categoryID);
            product.Category = category;
            _productRepository.Update(product);
            return await Task.Run(() => Ok(product));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Product>> DeleteProductAsync([FromBody] int id)
    {
        if (id < 0)
            return await Task.Run(() => BadRequest());
        else
        {
            var product = _productRepository.Get(id);
            if (product == null)
                return await Task.Run(() => BadRequest());
            else
            {
                _productRepository.Delete(id);
                return await Task.Run(() => Ok(product));
            }
        }
    }
}