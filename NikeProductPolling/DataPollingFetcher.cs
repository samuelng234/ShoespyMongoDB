using Newtonsoft.Json;
using NikeProductPolling.Models.Polling;
using System;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace NikeProductPolling
{
    internal class DataPollingFetcher
    {
        internal HttpClient Client { get; set; }
        internal Dictionary<string, string> UrlParams { get; set; }

        private readonly string MEN_ATTRIBUTE_ID = "16633190-45e5-4830-a068-232ac7aea82c,0f64ecc7-d624-4e91-b171-b83a03dd8550";
        private readonly string WOMEN_ATTRIBUTE_ID = "16633190-45e5-4830-a068-232ac7aea82c,7baf216c-acc6-4452-9e07-39c2ca77ba32";

        private string _baseUrl = "https://api.nike.com/cic/browse/v2";
        private string _endpointUrl = "/product_feed/rollup_threads/v2?filter=marketplace(NZ)&filter=language(en-GB)&filter=employeePrice(true)&consumerChannelId=d9a5bc42-4b9c-4976-858a-f159cf99c647&count=60";
        

        internal DataPollingFetcher() 
        {
            UrlParams = new Dictionary<string, string>();

            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            Client = new HttpClient(handler);
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            Client.DefaultRequestHeaders.Add("Postman-Token", "2bbffa7c-660a-4c43-95ed-ee52b15cf9a5");
            Client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.31.0");
            Client.DefaultRequestHeaders.Add("Host", "api.nike.com");

            UrlParams.Add("queryid", "products");
            UrlParams.Add("anonymousId", "D512F0A3B5EADAD0942A4BCF1A76046F");
            UrlParams.Add("country", "nz");
            UrlParams.Add("language", "en-GB");
            UrlParams.Add("localizedRangeStr", "%7BlowestPrice%7D%E2%80%94%7BhighestPrice%7D");
        }

        internal async Task<List<NikeProduct>> GetProducts()
        {
            return await GetProductsTest();
            List<NikeProduct> products = new List<NikeProduct>();
            
            //products.AddRange(await GetProductsbyAttributeId(MEN_ATTRIBUTE_ID));
            //products.AddRange(await GetProductsbyAttributeId(WOMEN_ATTRIBUTE_ID));

            return products;
        }

        internal async Task<List<NikeBrandAndSizeProduct>> GetProductBrandAndSizes()
        {
            return await GetProductBrandAndSizesTest();
        }

        internal async Task<List<NikeProduct>> GetProductsTest()
        {
            try
            {
                List<NikeProduct> products = new List<NikeProduct>();
                NikeResponse? result = null;

                string productString = await File.ReadAllTextAsync("C:\\Projects\\NikeProductPolling\\Test data\\NikeProducts.json");
                result = JsonConvert.DeserializeObject<NikeResponse>(productString);

                if (result?.Data.Products.Products != null)
                {
                    products.AddRange(result.Data.Products.Products);
                }

                return products;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal async Task<List<NikeBrandAndSizeProduct>> GetProductBrandAndSizesTest()
        {
            try
            {
                List<NikeBrandAndSizeProduct> brandAndSizes = new List<NikeBrandAndSizeProduct>();
                NikeBrandAndSizeResponse? result = null;

                string productString = await File.ReadAllTextAsync("C:\\Projects\\NikeProductPolling\\Test data\\NikeBrandAndSize.json");
                result = JsonConvert.DeserializeObject<NikeBrandAndSizeResponse>(productString);

                if (result != null)
                {
                    brandAndSizes.AddRange(result.HydratedProducts);
                }

                return brandAndSizes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal async Task<List<NikeProduct>> GetProductsbyAttributeId(string attributeId)
        {
            try
            {
                List<NikeProduct> products = new List<NikeProduct>();
                NikeResponse? result = null;

                int anchor = 0;

                do
                {
                    StringBuilder parameterBuilder = new StringBuilder();

                    foreach (var keyPair in UrlParams)
                    {
                        parameterBuilder.Append("&");
                        parameterBuilder.Append(keyPair.Key);
                        parameterBuilder.Append("=");
                        parameterBuilder.Append(keyPair.Value);
                    }

                    parameterBuilder.Append("&endpoint=" + HttpUtility.UrlEncode(_endpointUrl + "&filter=attributeIds(" + attributeId + ")" + "&anchor=" + anchor));

                    if (parameterBuilder.Length < 1)
                    {
                        // throw exception and log
                        return null;
                    }

                    var queryUrl = _baseUrl + "?" + parameterBuilder.ToString().Substring(1);

                    var response = Client.GetAsync(queryUrl).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        response.Content.ReadAsStringAsync().Wait();
                        var compressedResult = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<NikeResponse>(compressedResult);

                        if (result?.Data.Products.Products != null)
                        {
                            products.AddRange(result.Data.Products.Products);
                        }
                    }

                    anchor += 60;

                    //anchor += int.Parse(EndpointParams.GetValueOrDefault("count"));
                } 
                while (result?.Data.Products.Products != null);

                return products;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
