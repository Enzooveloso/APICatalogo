using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    public ProductsController(IUnitOfWork uof)
    {
        _uof = uof;    
    }

    [HttpGet("products/{id}")]
    public ActionResult <IEnumerable<Product>> GetProdutcsCategory(int id)
    {
        var products = _uof.ProductRepository.GetProductsForCategories(id);

        if(products is null)
            return NotFound();

        return Ok(products);
    }


    [HttpGet]
    //Retorna todos os produtos
    public ActionResult<IEnumerable<Product>> Get()
    {
        var products = _uof.ProductRepository.GetAll();
        if (products is null)
        {
            return NotFound("Produtos não encontrados...");
        }
        return Ok(products);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    //Retorna produto específico com base no ID
    public ActionResult<Product> Get(int id)
    {
        var product = _uof.ProductRepository.Get(p => p.ProductID == id);
        if (product is null)
        {
            return NotFound("Produto não encontrado");
        }
        return Ok(product);
    }


    //Criar um novo produto na API
    [HttpPost]
    public ActionResult Post(Product product)
    {
        if (product is null)
        {
            return BadRequest();
        }
        
        var newProduct = _uof.ProductRepository.Create(product);
        _uof.Commit();

        return new CreatedAtRouteResult("ObterProduto",
            new { id = newProduct.ProductID }, newProduct);
    }

    //Edição completa dos produtos existentes
    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Product product)
    {
        if (id != product.ProductID)
        {
            return BadRequest();
        }

        var updatedProduct = _uof.ProductRepository.Update(product);
        _uof.Commit();

        return Ok(updatedProduct);
    }

    //Remoção completa dos produtos existentes com base no Id
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var product = _uof.ProductRepository.Get(p => p.ProductID == id);

        if (product is null)
        {
            return NotFound("Produto não encontrado");
        }

        var deletedProduct = _uof.ProductRepository.Delete(product);
        _uof.Commit();

        return Ok(deletedProduct);
    }
}

