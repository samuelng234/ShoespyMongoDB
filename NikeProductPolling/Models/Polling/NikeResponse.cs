namespace NikeProductPolling.Models.Polling
{
    public class NikeResponse
    {
        public NikeResponseProductContainer Data { get; set; }
    }

    public class NikeResponseProductContainer
    {
        public NikeResponsePagenatedProductResponse Products { get; set; }
    }

    public class NikeResponsePagenatedProductResponse
    {
        public string Errors { get; set; }
        public IEnumerable<NikeProduct> Products { get; set; }
        public NikeResponsePages Pages { get; set; }
        public string ExternalResponses { get; set; }
        public string TraceId { get; set; }
    }

    public class NikeProduct
    {
        public string CardType { get; set; }
        public string CloudProductId { get; set; }
        public string ColorDescription { get; set; }
        public IEnumerable<NikeColorway> Colorways { get; set; }
        public bool Customizable { get; set; }
        public bool HasExtendedSizing { get; set; }
        public string Id { get; set; }
        public NikeImage Images { get; set; }
        public bool InStock { get; set; }
        public bool IsComingSoon { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsExcluded { get; set; }
        public bool IsGiftCard { get; set; }
        public bool IsJersey { get; set; }
        public bool IsLaunch { get; set; }
        public bool IsMemberExclusive { get; set; }
        public bool IsNBA { get; set; }
        public bool IsNFL { get; set; }
        public bool IsSustainable { get; set; }
        public string Label { get; set; }
        public NbyColorway NbyColorway { get; set; }
        public string Pid { get; set; }
        public string PrebuildId { get; set; }
        public NikePrice Price { get; set; }
        public string ProductInstanceId { get; set; }
        public string ProductType { get; set; }
        public string Properties { get; set; }
        public IEnumerable<string> SalesChannel { get; set; }
        public string Subtitle { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }

    public class NbyColorway
    {
        public string ColorDescription { get; set; }
        public NikeImage Images { get; set; }
        public string PdpUrl { get; set; }
        public NikePrice Price { get; set; }
    }

    public class NikeColorway : NbyColorway
    {
        public string CloudProductId { get; set; }
        public bool InStock { get; set; }
        public bool IsComingSoon { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsExcluded { get; set; }
        public bool IsLaunch { get; set; }
        public bool IsMemberExclusive { get; set; }
        public bool IsNew { get; set; }
        public string Label { get; set; }
        public string Pid { get; set; }
        public string PrebuildId { get; set; }
        public string ProductInstanceId { get; set; }
    }

    public class NikeImage
    {
        public string PortraitURL { get; set; }
        public string SquarishURL { get; set; }
    }

    public class NikePrice
    {
        public string Currency { get; set; }
        public decimal CurrentPrice { get; set; }
        public bool? Discounted { get; set; }
        public decimal? EmployeePrice { get; set; }
        public decimal FullPrice { get; set; }
        public decimal? MinimumAdvertisedPrice { get; set; }
    }

    public class NikeResponsePages
    {
        public string Prev { get; set; }
        public string Next { get; set; }
        public int TotalPages { get; set; }
        public int TotalResources { get; set; }
        public string SearchSummary { get; set; }
    }
}
