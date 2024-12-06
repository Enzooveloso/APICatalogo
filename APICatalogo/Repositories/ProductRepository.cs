using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        public ProductRepository(AppDbContext context) : base(context)
        { }

        public PagedList<Product> GetProducts(ProductsParameters productsParams)
        {
            var products = GetAll().OrderBy(p => p.ProductID).AsQueryable();
            var productsOrdered = PagedList<Product>.ToPagedList(products, productsParams.PageNumber, productsParams.PageSize);

            return productsOrdered;
        }

        public PagedList<Product> GetProductsFilterPrice(ProductFilterPrice productsfilterParams)
        {
            var products = GetAll().AsQueryable();

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
            var productsFiltered = PagedList<Product>.ToPagedList(products, productsfilterParams.PageNumber, productsfilterParams.PageSize);

            return productsFiltered;
        }

        public IEnumerable<Product> GetProductsForCategories(int id)
        {
            return GetAll().Where(c => c.CategoryId == id);
        }
    }
}
