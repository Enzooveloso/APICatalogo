using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParams);
    Task<IPagedList<Product>> GetProductsFilterPriceAsync(ProductFilterPrice productsfilterParams );
    Task<IEnumerable<Product>> GetProductsForCategoriesAsync(int id);
}
