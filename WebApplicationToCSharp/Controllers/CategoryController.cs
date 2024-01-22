using ClassLibrary.ContextAndRepository.Models;
using ClassLibrary.ContextAndRepository.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace WebApplicationToCSharp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private IRepository<Category> repository;
    private IMemoryCache _cache;

    private string GetCsv(IEnumerable<Category> categories)
    {
        StringBuilder sb = new StringBuilder();
        foreach(var c in categories)
        {
            sb.AppendLine($"{c.Name}; {c.Description}; \n");
        }
        return sb.ToString();
    }
    public CategoryController(CategoryRepository categoryRepository, IMemoryCache cache)
    {
        repository = categoryRepository;
        _cache = cache;
    }

    [HttpGet(template:"GetCategoriesCSV")]
    public FileContentResult GetCategoriesCsv()
    {
        var content = "";

        if(_cache.TryGetValue("categories", out IEnumerable<Category>? categories))
        {
            content = GetCsv(categories!);
        }
        else
        {
            categories = repository.GetAll();
            _cache.Set("categories", categories.ToList());
            content = GetCsv(categories);
        }
        return File(new System.Text.UTF8Encoding().GetBytes(content), "text/csv", "report.csv");
    }

    [HttpGet(template:"GetCategoriesCSVUrl")]
    public ActionResult<string> GetCategoriesCsvUrl()
    {
        var content = "";

        if (_cache.TryGetValue("categories", out IEnumerable<Category>? categories))
        {
            content = GetCsv(categories!);
        }
        else
        {
            categories = repository.GetAll();
            _cache.Set("categories", categories.ToList());
            content = GetCsv(categories);
        }

        string fileName = null!;

        fileName = "categories" + DateTime.Now.ToBinary().ToString() + ".csv";
        System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName), content);

        return "https://" + Request.Host.ToString() + "/static/" + fileName;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetProductAsync()
    {
       if(_cache.TryGetValue("categories", out IEnumerable<Category>? categories))
        {
            return await Task.Run(() => Ok(categories));
        }
       else
        {
            categories = repository.GetAll();
            _cache.Set("products", categories.ToList(), TimeSpan.FromMinutes(30));
            return await Task.Run(() => Ok(categories));
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Category>> GetCategoryAsync([FromQuery] int id) =>
        await Task.Run(() => Ok(repository.Get(id)));

    [HttpPost]
    public async Task<ActionResult<Category>> AddCategoryAsync([FromBody] Category category)
    {
        if(category == null && category!.ID<0)
            return await Task.Run(() => BadRequest());
        else
        {
            repository.Create(category);
            _cache.Remove("categories");
            return await Task.Run(() => Ok(category));
        }
    }

    [HttpPut]
    public async Task<ActionResult<Category>> UpdateCategoryAsync([FromBody] Category category)
    {
        if (category == null)
            return await Task.Run(() => BadRequest());
        else
        {
            repository.Update(category);
            return await Task.Run(() => Ok(category));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Category>> DeleteCategoryAsync([FromQuery] int id)
    {
        if(id<0)
            return await Task.Run(() => BadRequest());
        else
        {
            var category = repository.Get(id);
            repository.Delete(id);
            return await Task.Run(() => Ok(category));
        }
    }
}