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

    private readonly IUnitOfWork _uof;
    private readonly ILogger<CategoriesController> _logger;


    public CategoriesController(ILogger<CategoriesController> logger, IUnitOfWork uof)
    {
        _logger = logger;
        _uof = uof;
    }


    [HttpGet]
    public ActionResult<IEnumerable<Category>> Get()
    {
        var categories = _uof.CategoryRepository.GetAll();
        return Ok(categories);
    }


    [HttpGet("{id:int}", Name = "ObterCategoria")]
    //Retorna uma categoria específica com base no ID
    public ActionResult<Category> Get(int id)
    {
        var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);

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
        var createCategory = _uof.CategoryRepository.Create(category);
        _uof.Commit();

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

        _uof.CategoryRepository.Update(category);
        _uof.Commit();

        return Ok(category); ;
    }

    //Remoção completa da categoria existentes com base no Id
    [HttpDelete("{id:int}")]
    public ActionResult<Category> Delete(int id)
    {
        var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);

        if (category == null)
        {
            _logger.LogWarning($"Category com id={id} nao encontrada");
            return NotFound($"Category com id={id} nao encontrada");
        }

        var deleteCategory = _uof.CategoryRepository.Delete(category);
        _uof.Commit();
        return Ok(deleteCategory);
    }
}
