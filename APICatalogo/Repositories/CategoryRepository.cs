using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) :base(context) 
    {
    }

    public PagedList<Category> GetCategories(CategoriesParameters categoriesParams)
    {
        
            var categories = GetAll().OrderBy(p => p.CategoryId).AsQueryable();
            var categoriesOrdered = PagedList<Category>.ToPagedList(categories, categoriesParams.PageNumber, categoriesParams.PageSize);

            return categoriesOrdered;

    }

    public PagedList<Category> GetCategoriesFilterName(CategoriesFilterName categoriesParams)
    {
        var categories = GetAll().AsQueryable();

        if(!string.IsNullOrEmpty(categoriesParams.Name))
        {
            categories = categories.Where( c => c.Name.Contains(categoriesParams.Name));
        }

        var categoriesOrdered = PagedList<Category>.ToPagedList(categories, categoriesParams.PageNumber, categoriesParams.PageSize);

        return categoriesOrdered;
    }
}
