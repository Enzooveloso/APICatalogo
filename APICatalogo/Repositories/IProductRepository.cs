using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public interface IProductRepository : IRepository<Product>
{
    PagedList<Product> GetProducts(ProductsParameters productsParams);
    PagedList<Product> GetProductsFilterPrice(ProductFilterPrice productsfilterParams );
    IEnumerable<Product> GetProductsForCategories(int id);
}
