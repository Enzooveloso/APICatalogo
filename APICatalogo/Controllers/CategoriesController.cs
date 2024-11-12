using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{

    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }


    //Retorna todos as categorias junto com seus produtos associados
    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesProducts()
    {
        return _context.Categories.Include(p=>p.Products).ToList();
    }

    [HttpGet]
    //Retorna todos as categorias
    public ActionResult<IEnumerable<Category>> Get()
    {
        return _context.Categories.ToList();
    }


    [HttpGet("{id:int}", Name = "ObterCategoria")]
    //Retorna uma categoria específica com base no ID
    public ActionResult<Category> Get(int id)
    {
        var category = _context.Categories.FirstOrDefault(p => p.CategoryId == id);
        if (category is null)
        {
            return NotFound("Categoria não encontrado");
        }
        return Ok(category);
    }


    //Criar uma nova categoria na API
    [HttpPost]
    public ActionResult Post(Category category)
    {
        if (category is null)
        {
            return BadRequest();
        }
        _context.Categories.Add(category);
        _context.SaveChanges();
        return new CreatedAtRouteResult("ObterProduto",
            new { id = category.CategoryId }, category);
    }

    //Edição completa da categoria existentes
    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Category category)
    {
        if (id != category.CategoryId)
        {
            return BadRequest();
        }
        _context.Entry(category).State = EntityState.Modified;
        _context.SaveChanges();
        return Ok(category);
    }

    //Remoção completa da categoria existentes com base no Id
    [HttpDelete("{id:int}")]
    public ActionResult<Category> Delete(int id)
    {
        var category = _context.Categories.FirstOrDefault(p => p.CategoryId == id);

        if (category is null)
        {
            return NotFound("Produto não localizado");
        }

        _context.Categories.Remove(category);
        _context.SaveChanges();
        return Ok(category);
    }
}
