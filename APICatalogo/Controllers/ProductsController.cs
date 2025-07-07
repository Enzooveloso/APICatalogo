using APICatalogo.Context;
using APICatalogo.DTO;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
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
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProdutcsCategory(int id)
    {
        var products = await _uof.ProductRepository.GetProductsForCategoriesAsync(id);

        if (products is null)
            return NotFound();

        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

        return Ok(productsDto);
    }


    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery] ProductsParameters productsParameters)
    {
        var products = await _uof.ProductRepository.GetProductsAsync(productsParameters);
        return HasProduct(products);
    }


    private ActionResult<IEnumerable<ProductDTO>> HasProduct(IPagedList<Product> products)
    {
        var metadata = new
        {
            products.Count,
            products.PageSize,
            products.PageCount,
            products.TotalItemCount,
            products.HasNextPage,
            products.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

        return Ok(productsDto);
    }


    [HttpGet("filter/price/pagination")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsFilterPrice([FromQuery] ProductFilterPrice productFilterParameters)
    {
        var products = await _uof.ProductRepository.GetProductsFilterPriceAsync(productFilterParameters);

        return HasProduct(products);
    }

    /// <summary>
    /// Exibe relação produto por categoria
    /// </summary>
    /// <returns> Retorna todos os produtos</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
    {
        try
        {
            var products = await _uof.ProductRepository.GetAllAsync();
            
            if (products is null)
            {
                return NotFound();
            }

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }
        catch
        {
            return BadRequest("Bad Request");
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    //Retorna produto específico com base no ID
    public async Task<ActionResult<ProductDTO>> Get(int? id)
    {
        var product = await _uof.ProductRepository.GetAsync(p => p.ProductID == id);

        if (id == null || id <= 0)
        {
            return BadRequest("ID invalido");
        }

        if (product is null)
        {
            return NotFound("Produto não encontrado");
        }

        var productDto = _mapper.Map<ProductDTO>(product);

        return Ok(product);
    }


    //Criar um novo produto na API
    [HttpPost]
    public async Task<ActionResult<ProductDTO>> Post(ProductDTO productDto)
    {
        if (productDto is null)
        {
            return BadRequest();
        }

        var product = _mapper.Map<Product>(productDto);

        var newProduct = _uof.ProductRepository.Create(product);
        await _uof.CommitAsync();

        var newProductDto = _mapper.Map<ProductDTO>(newProduct);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = newProductDto.ProductID }, newProductDto);
    }


    [HttpPatch("{id}/ UpdatePartial")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProductDTOUpdateResponse>> Patch(int id, JsonPatchDocument<ProductDTOUpdateRequest> patchProductDTO)
    {
        if (patchProductDTO is null || id <= 0) return BadRequest();

        var product = await _uof.ProductRepository.GetAsync(p => p.ProductID == id);

        if (product is null) return NotFound();

        var productUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(product);

        patchProductDTO.ApplyTo(productUpdateRequest, ModelState);

        if (!ModelState.IsValid || TryValidateModel(productUpdateRequest)) return BadRequest(ModelState);

        _mapper.Map(productUpdateRequest, product);
        _uof.ProductRepository.Update(product);
        await _uof.CommitAsync();

        return Ok(_mapper.Map<ProductDTOUpdateResponse>(product));
    }


    //Edição completa dos produtos existentes
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProductDTO>> Put(int id, ProductDTO productDto)
    {
        if (id != productDto.ProductID) return BadRequest();


        var product = _mapper.Map<Product>(productDto);

        var updatedProduct = _uof.ProductRepository.Update(product);
        await _uof.CommitAsync();

        var updatedProductDto = _mapper.Map<ProductDTO>(updatedProduct);

        return Ok(updatedProductDto);
    }


    //Remoção completa dos produtos existentes com base no Id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProductDTO>> Delete(int id)
    {
        var product = await _uof.ProductRepository.GetAsync(p => p.ProductID == id);

        if (product is null)
        {
            return NotFound("Produto não encontrado");
        }

        var deletedProduct = _uof.ProductRepository.Delete(product);
        await _uof.CommitAsync();

        var deletedProductDto = _mapper.Map<ProductDTO>(deletedProduct);

        return Ok(deletedProductDto);
    }
}

