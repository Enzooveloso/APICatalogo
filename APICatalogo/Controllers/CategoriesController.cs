using APICatalogo.Context;
using APICatalogo.DTO;
using APICatalogo.DTO.Mapping;
using APICatalogo.DTOs;
using APICatalogo.Filter;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Controllers;

[EnableCors("OrigensComAcessoPermitido")]
[Route("[controller]")]
[ApiController]
[EnableRateLimiting("fixedWindow")]
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
    [Authorize]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
    {
        var categories = await _uof.CategoryRepository.GetAllAsync();
        if (categories is null)
            return NotFound("Não existem categorias ...");

        var categoriesDto = categories.ToCategoryDTOList();

        return Ok(categoriesDto);
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery] CategoriesParameters categoriesParameters)
    {
        var categories = await _uof.CategoryRepository.GetCategoriesAsync(categoriesParameters);
        return HasCategory(categories);
    }

    private ActionResult<IEnumerable<CategoryDTO>> HasCategory(IPagedList<Category> categories)
    {
        var metadata = new
        {
            categories.Count,
            categories.PageSize,
            categories.PageCount,
            categories.TotalItemCount,
            categories.HasNextPage,
            categories.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriesDto = categories.ToCategoryDTOList();

        return Ok(categoriesDto);
    }

    [HttpGet("filter/name/pagination")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesFiltered([FromQuery] CategoriesFilterName categorieParameters) 
    {
        var categoriesFiltered = await _uof.CategoryRepository.GetCategoriesFilterNameAsync(categorieParameters);

        return HasCategory(categoriesFiltered);
    }

    [DisableRateLimiting]
    [HttpGet("{id:int}", Name = "ObterCategoria")]
    //Retorna uma categoria específica com base no ID
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get(int id)
    {
        var category = await _uof.CategoryRepository.GetAsync(c => c.CategoryId == id);

        if (category is null)
        {
            _logger.LogWarning($"Categoria com id= {id} nao encontrada");
            return NotFound($"Categoria com id= {id} não encontrada");
        }

        var categoryDto = category.ToCategoryDTO();
        

        return Ok(categoryDto);
    }


    //Criar uma nova categoria na API
    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO categoryDto)
    {
        if (categoryDto is null)
        {
            _logger.LogWarning($"Dados Inválidos...");
            return BadRequest("Dados Inválidos");
        }

        var category = categoryDto.ToCategory();

        var createCategory = _uof.CategoryRepository.Create(category);
        await _uof.CommitAsync();

        var newCategoryDto = createCategory.ToCategoryDTO();

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = createCategory.CategoryId }, createCategory);
    }


    //Edição completa da categoria existentes
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryDTO>> Put(int id, CategoryDTO categoryDto)
    {
        if (id != categoryDto.CategoryId)
        {
            _logger.LogWarning($"Dados Invalidos");
            return BadRequest("Dados Invállidos");
        }

        var category = categoryDto.ToCategory();

       var updatedCategory = _uof.CategoryRepository.Update(category);
        await _uof.CommitAsync();

        var updatedCategoryDto = updatedCategory.ToCategoryDTO();

        return Ok(updatedCategoryDto); ;
    }


    //Remoção completa da categoria existentes com base no Id
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<CategoryDTO>> Delete(int id)
    {
        var category = await _uof.CategoryRepository.GetAsync(c => c.CategoryId == id);

        if (category == null)
        {
            _logger.LogWarning($"Category com id={id} nao encontrada");
            return NotFound($"Category com id={id} nao encontrada");
        }

        var deletedCategory = _uof.CategoryRepository.Delete(category);
        await _uof.CommitAsync();

        var deletedCategoryDto = deletedCategory.ToCategoryDTO();

        return Ok(deletedCategoryDto);
    }
}
