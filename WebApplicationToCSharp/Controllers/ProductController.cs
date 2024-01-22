using ClassLibrary.ContextAndRepository.Models;
using ClassLibrary.ContextAndRepository.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace WebApplicationToCSharp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private IRepository<Product> _productRepository;
    private IMemoryCache _cache;


    private string GetCsv(IEnumerable<Product> products)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var p in products)
        {
            sb.AppendLine($"{p.Name}; {p.Description}; \n");
        }
        return sb.ToString();
    }
    public ProductController(ProductRepository productRepository, IMemoryCache cache)
    {
        _productRepository = productRepository;
        _cache = cache;
    }




    [HttpGet(template: "GetProductsCSV")]
    public FileContentResult GetProductsCsv()
    {
        var content = "";

        if (_cache.TryGetValue("products", out IEnumerable<Product>? products))
        {
            content = GetCsv(products!);
        }
        else
        {
            products = _productRepository.GetAll();
            _cache.Set("categories", products.ToList());
            content = GetCsv(products);
        }
        return File(new System.Text.UTF8Encoding().GetBytes(content), "text/csv", "report.csv");
    }


    [HttpGet(template: "GetProductsCSVUrl")]
    public ActionResult<string> GetProductsCsvUrl()
    {
        var content = "";

        if (_cache.TryGetValue("products", out IEnumerable<Product>? products))
        {
            content = GetCsv(products!);
        }
        else
        {
            products = _productRepository.GetAll();
            _cache.Set("products", products.ToList());
            content = GetCsv(products);
        }

        string fileName = null!;

        fileName = "products" + DateTime.Now.ToBinary().ToString() + ".csv";
        System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName), content);

        return "https://" + Request.Host.ToString() + "/static/" + fileName;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
    {
        if(_cache.TryGetValue("products", out IEnumerable<Product>? products))
        {
            return await Task.Run(() =>Ok(products));
        }
        else
        {
            products = _productRepository.GetAll();
            _cache.Set("products", products.ToList(), TimeSpan.FromMinutes(30));
            return await Task.Run(() => Ok(products));
        }
    }

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
            _cache.Remove("products");
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