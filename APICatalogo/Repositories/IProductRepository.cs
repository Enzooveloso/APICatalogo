using APICatalogo.Models;

namespace APICatalogo.Repositories;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetProductsForCategories(int id);
}
