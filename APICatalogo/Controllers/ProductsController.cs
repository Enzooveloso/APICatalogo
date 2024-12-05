using APICatalogo.Context;
using APICatalogo.DTO;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
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
    private readonly IMapper _mapper;
    public ProductsController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet("products/{id}")]
    public ActionResult <IEnumerable<ProductDTO>> GetProdutcsCategory(int id)
    {
        var products = _uof.ProductRepository.GetProductsForCategories(id);

        if(products is null)
            return NotFound();

        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

        return Ok(productsDto);
    }


    [HttpGet]
    //Retorna todos os produtos
    public ActionResult<IEnumerable<ProductDTO>> Get()
    {
        var products = _uof.ProductRepository.GetAll();
        if (products is null)
        {
            return NotFound("Produtos não encontrados...");
        }

        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

        return Ok(productsDto);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    //Retorna produto específico com base no ID
    public ActionResult<ProductDTO> Get(int id)
    {
        var product = _uof.ProductRepository.Get(p => p.ProductID == id);
        if (product is null)
        {
            return NotFound("Produto não encontrado");
        }

        var productDto = _mapper.Map<ProductDTO>(product);

        return Ok(product);
    }


    //Criar um novo produto na API
    [HttpPost]
    public ActionResult<ProductDTO> Post(ProductDTO productDto)
    {
        if (productDto is null)
        {
            return BadRequest();
        }

        var product = _mapper.Map<Product>(productDto);

        var newProduct = _uof.ProductRepository.Create(product);
        _uof.Commit();

        var newProductDto = _mapper.Map<ProductDTO>(newProduct);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = newProductDto.ProductID }, newProductDto);
    }

    //Edição completa dos produtos existentes
    [HttpPut("{id:int}")]
    public ActionResult<ProductDTO> Put(int id, ProductDTO productDto)
    {
        if (id != productDto.ProductID) return BadRequest();
      

        var product = _mapper.Map<Product>(productDto);

        var updatedProduct = _uof.ProductRepository.Update(product);
        _uof.Commit();

        var updatedProductDto = _mapper.Map<ProductDTO>(updatedProduct);

        return Ok(updatedProductDto);
    }

    //Remoção completa dos produtos existentes com base no Id
    [HttpDelete("{id:int}")]
    public ActionResult<ProductDTO> Delete(int id)
    {
        var product = _uof.ProductRepository.Get(p => p.ProductID == id);

        if (product is null)
        {
            return NotFound("Produto não encontrado");
        }

        var deletedProduct = _uof.ProductRepository.Delete(product);
        _uof.Commit();

        var deletedProductDto = _mapper.Map<ProductDTO>(deletedProduct);

        return Ok(deletedProductDto);
    }
}

