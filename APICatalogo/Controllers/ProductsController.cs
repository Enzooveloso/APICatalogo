using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    //Retorna todos os produtos
    public ActionResult<IEnumerable<Product>> Get()
    {
        var products = _context.Products.ToList();
        if (products is null)
        {
            return NotFound("Produtos não encontrados...");
        }
        return products;
    }

    [HttpGet("{id:int}", Name= "ObterProduto")]
    //Retorna produto específico com base no ID
    public ActionResult<Product> Get(int id) {
        var product = _context.Products.FirstOrDefault(p=>p.ProductID == id);
        if(product is null) {
            return NotFound("Produto não encontrado");
        }
        return product;
    }


    //Criar um novo produto na API
    [HttpPost]
    public ActionResult Post(Product product)
    {
        if(product is null)
        {
            return BadRequest();
        }
        _context.Products.Add(product);
        _context.SaveChanges();
        return  new CreatedAtRouteResult("ObterProduto", 
            new { id = product.ProductID }, product);
    }

    //Edição completa dos produtos existentes
    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Product product)
    {
        if(id != product.ProductID)
        {
            return BadRequest();
        }
        _context.Entry(product).State = EntityState.Modified;
        _context.SaveChanges();
        return Ok(product);
    }

    //Remoção completa dos produtos existentes com base no Id
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductID == id);
        
        if(  product is null)
        {
            return NotFound("Produto não localizado");
        }

        _context.Products.Remove(product);
        _context.SaveChanges();
        return Ok(product);
    }
}

