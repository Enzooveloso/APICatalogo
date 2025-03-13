using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        public ProductRepository(AppDbContext context) : base(context)
        { }

        public async Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParams)
        {
            var products = await GetAllAsync();

            var productsOrdered = products.OrderBy(p => p.ProductID).AsQueryable();

            var result = await productsOrdered.ToPagedListAsync(productsParams.PageNumber, productsParams.PageSize);

            return result;
        }

        public async Task<IPagedList<Product>> GetProductsFilterPriceAsync(ProductFilterPrice productsfilterParams)
        {
            var products = await GetAllAsync();

            if (productsfilterParams.Price.HasValue && !string.IsNullOrEmpty(productsfilterParams.PriceCritical))
            {
                if (productsfilterParams.PriceCritical.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price > productsfilterParams.Price.Value).OrderBy(p => p.Price);
                }
                else if (productsfilterParams.PriceCritical.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price < productsfilterParams.Price.Value).OrderBy(p => p.Price);
                }
                else if (productsfilterParams.PriceCritical.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price == productsfilterParams.Price.Value).OrderBy(p => p.Price);
                }
            }

            var productsFiltered = await products.ToPagedListAsync(productsfilterParams.PageNumber, productsfilterParams.PageSize);

            return productsFiltered;
        }

        public async Task<IEnumerable<Product>> GetProductsForCategoriesAsync(int id)
        {
            var products = await GetAllAsync();
            var productsCategories = products.Where(c => c.CategoryId == id);
            return productsCategories;
        }
    }
}
