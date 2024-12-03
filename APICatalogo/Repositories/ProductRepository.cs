using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
 
        public ProductRepository(AppDbContext context) : base(context)
        {}

        public IEnumerable<Product> GetProductsForCategories(int id)
        {
            return GetAll().Where( c => c.CategoryId == id);
        }
    }
}
