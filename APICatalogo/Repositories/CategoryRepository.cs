using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;


namespace APICatalogo.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParams)
    {

        var categories = await GetAllAsync();
        var categoriesOrdered = categories.OrderBy(p => p.CategoryId).AsQueryable();

        //var result = PagedList<Category>.ToPagedList(categoriesOrdered, categoriesParams.PageNumber, categoriesParams.PageSize);
        var result = await categoriesOrdered.ToPagedListAsync(categoriesParams.PageNumber, categoriesParams.PageSize);
        return result;

    }

    public async Task<IPagedList<Category>> GetCategoriesFilterNameAsync(CategoriesFilterName categoriesParams)
    {
        var categories = await GetAllAsync();

        if (!string.IsNullOrEmpty(categoriesParams.Name))
        {
            categories = categories.Where(c => c.Name.Contains(categoriesParams.Name));
        }

        //var categoriesOrdered = PagedList<Category>.ToPagedList(categories.AsQueryable(), categoriesParams.PageNumber, categoriesParams.PageSize);
        var categoriesOrdered = await categories.ToPagedListAsync(categoriesParams.PageNumber, categoriesParams.PageSize);


        return categoriesOrdered;
    }
}
