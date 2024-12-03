using APICatalogo.Context;
using APICatalogo.Filter;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{

    private readonly IRepository<Category> _repository;
    private readonly ILogger<CategoriesController> _logger;


    public CategoriesController(ICategoryRepository repository, ILogger<CategoriesController> logger)
    {
        _repository = repository;
        _logger = logger;

    }


    [HttpGet]
    public ActionResult<IEnumerable<Category>> Get()
    {
        var categories = _repository.GetAll();
        return Ok(categories);
    }


    [HttpGet("{id:int}", Name = "ObterCategoria")]
    //Retorna uma categoria específica com base no ID
    public ActionResult<Category> Get(int id)
    {
        var category = _repository.Get(c => c.CategoryId == id);

        if (category is null)
        {
            _logger.LogWarning($"Categoria com id= {id} naoo encontrada");
            return NotFound($"Categoria com id= {id} não encontrada");
        }
        return Ok(category);
    }


    //Criar uma nova categoria na API
    [HttpPost]
    public ActionResult Post(Category category)
    {
        if (category is null)
        {
            _logger.LogWarning($"Dados Inválidos...");
            return BadRequest("Dados Inválidos");
        }
        var createCategory = _repository.Create(category);
        return new CreatedAtRouteResult("ObterCategoria",
            new { id = createCategory.CategoryId }, createCategory);
    }

    //Edição completa da categoria existentes
    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Category category)
    {
        if (id != category.CategoryId)
        {
            _logger.LogWarning($"Dados Invalidos");
            return BadRequest("Dados Invállidos");
        }

        _repository.Update(category);
        return Ok(category); ;
    }

    //Remoção completa da categoria existentes com base no Id
    [HttpDelete("{id:int}")]
    public ActionResult<Category> Delete(int id)
    {
        var category = _repository.Get(c => c.CategoryId == id );

        if (category == null)
        {
            _logger.LogWarning($"Category com id={id} nao encontrada");
            return NotFound($"Category com id={id} nao encontrada");
        }

        var deleteCategory = _repository.Delete(category); 
        return Ok(deleteCategory);
    }
}
