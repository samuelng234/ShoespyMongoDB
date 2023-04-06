namespace NikeProductPolling.Models.Polling
{
    public class NikeBrandAndSizeResponse
    {
        public IEnumerable<NikeBrandAndSizeProduct> HydratedProducts { get; set; }
    }

    public class NikeBrandAndSizeProduct
    {
        public bool IsOnSale { get; set; }
        public string CatalogId { get; set; }
        public bool IsNikeByYou { get; set; }
        public IEnumerable<string> Genders { get; set; }
        public string Brand { get; set; }
        public decimal CurrentPrice { get; set; }
        public string Country { get; set; }
        public string CloudProductId { get; set; }
        public IEnumerable<string> SubCategory { get; set; }
        public IEnumerable<NikeSize> SkuData { get; set; }
        public string Category { get; set; }
        public decimal FullPrice { get; set; }
        public string Color { get; set; }
    }

    public class NikeSize
    {
        public string Size { get; set; }
        public string Sku { get; set; }
    }
}
