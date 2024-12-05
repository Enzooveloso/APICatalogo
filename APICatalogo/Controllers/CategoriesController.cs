using APICatalogo.Context;
using APICatalogo.DTO.Mapping;
using APICatalogo.DTOs;
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
    public ActionResult<IEnumerable<CategoryDTO>> Get()
    {
        var categories = _uof.CategoryRepository.GetAll();
        if(categories is null)
            return NotFound("Não existem categorias ...");

        var categoriesDto = categories.ToCategoryDTOList();

        return Ok(categoriesDto);
    }


    [HttpGet("{id:int}", Name = "ObterCategoria")]
    //Retorna uma categoria específica com base no ID
    public ActionResult<CategoryDTO> Get(int id)
    {
        var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);

        if (category is null)
        {
            _logger.LogWarning($"Categoria com id= {id} naoo encontrada");
            return NotFound($"Categoria com id= {id} não encontrada");
        }

        var categoryDto = category.ToCategoryDTO();
        

        return Ok(categoryDto);
    }


    //Criar uma nova categoria na API
    [HttpPost]
    public ActionResult<CategoryDTO> Post(CategoryDTO categoryDto)
    {
        if (categoryDto is null)
        {
            _logger.LogWarning($"Dados Inválidos...");
            return BadRequest("Dados Inválidos");
        }

        var category = categoryDto.ToCategory();

        var createCategory = _uof.CategoryRepository.Create(category);
        _uof.Commit();

        var newCategoryDto = createCategory.ToCategoryDTO();

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = createCategory.CategoryId }, createCategory);
    }


    //Edição completa da categoria existentes
    [HttpPut("{id:int}")]
    public ActionResult<CategoryDTO> Put(int id, CategoryDTO categoryDto)
    {
        if (id != categoryDto.CategoryId)
        {
            _logger.LogWarning($"Dados Invalidos");
            return BadRequest("Dados Invállidos");
        }

        var category = categoryDto.ToCategory();

       var updatedCategory = _uof.CategoryRepository.Update(category);
        _uof.Commit();

        var updatedCategoryDto = updatedCategory.ToCategoryDTO();

        return Ok(updatedCategoryDto); ;
    }


    //Remoção completa da categoria existentes com base no Id
    [HttpDelete("{id:int}")]
    public ActionResult<CategoryDTO> Delete(int id)
    {
        var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);

        if (category == null)
        {
            _logger.LogWarning($"Category com id={id} nao encontrada");
            return NotFound($"Category com id={id} nao encontrada");
        }

        var deletedCategory = _uof.CategoryRepository.Delete(category);
        _uof.Commit();

        var deletedCategoryDto = deletedCategory.ToCategoryDTO();

        return Ok(deletedCategoryDto);
    }
}
