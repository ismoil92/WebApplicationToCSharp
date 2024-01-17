using ClassLibrary.ContextAndRepository.Models;
using ClassLibrary.ContextAndRepository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationToCSharp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private IRepository<Category> repository;

    public CategoryController()
    {
        repository = new CategoryRepository();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetProductAsync() =>
        await Task.Run(() => Ok(repository.GetAll()));

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