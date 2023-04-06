// See https://aka.ms/new-console-template for more information
using NikeProductPolling;
using NikeProductPolling.Services;

var dataFetcher = new DataPollingFetcher();
var products = await dataFetcher.GetProducts();
var productBrandAndSizes = await dataFetcher.GetProductBrandAndSizes();

var service = new ProductService();
service.AddNikeProducts(products);
service.AddNikeProductBrandAndSizes(productBrandAndSizes);

Console.ReadKey();
