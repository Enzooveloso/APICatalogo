namespace APICatalogo.Pagination
{
    public class ProductFilterPrice : QueryStringParameters
    {
        public decimal? Price { get; set; }
        public string? PriceCritical { get; set; }// max, min ou equal
    }
}
