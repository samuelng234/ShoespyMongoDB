using MongoDB.Driver;
using NikeProductPolling.Models.DBDocuments;
using NikeProductPolling.Models.Polling;
using NikeProductPolling.Repositories;

namespace NikeProductPolling.Services
{
    internal class ProductService
    {
        private ProductRepository repository;

        internal ProductService() 
        {
            repository = new ProductRepository();
        }

        internal async void AddNikeProducts(IEnumerable<NikeProduct> products)
        {
            var websiteId = await repository.UpsertWebsite(new Website()
            {
                Name = "Nike",
                CountryId = "NZ",
                Url = "https://www.nike.com/nz/",
                Active = true
            });

            var modifiedProducts = await UpsertProducts(products, websiteId);

            await UpsertColourways(products, modifiedProducts, websiteId);
            
            // disable inactive products
        }

        internal async void AddNikeProductBrandAndSizes(IEnumerable<NikeBrandAndSizeProduct> brandAndSizes)
        {
            var brands = await repository.GetBrands();

            foreach (var product in brandAndSizes)
            {

            }
        }

        private async Task<IEnumerable<Product>> UpsertProducts(IEnumerable<NikeProduct> products, string websiteId)
        {
            var bulkUpsert = new List<WriteModel<Product>>();
            var pids = new List<string>();

            foreach (var product in products)
            {
                pids.AddRange(product.Colorways.Select(x => x.Pid));

                FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.AnyIn(x => x.PIds, product.Colorways.Select(x => x.Pid));
                UpdateDefinition<Product> update = Builders<Product>.Update
                    .Set("PIds", product.Colorways.Select(x => x.Pid))
                    .Set("Name", product.Title)
                    .Set("Category", product.Subtitle)
                    .Set("Active", true)
                    .Set("WebsiteId", websiteId);

                var upsertOne = new UpdateOneModel<Product>(filterDefinition, update) { IsUpsert = true };
                bulkUpsert.Add(upsertOne);
            }

            await repository.UpsertProducts(bulkUpsert);
            return await repository.GetProducts(Builders<Product>.Filter.AnyIn(x => x.PIds, pids));
        }

        private async Task UpsertColourways(IEnumerable<NikeProduct> responseProducts, IEnumerable<Product> products, string websiteId)
        {
            var bulkProductUpsert = new List<WriteModel<Product>>();

            var pids = new List<string>();

            var bulkColourwaysUpsert = new List<WriteModel<ColourWay>>();
            foreach (var responseProduct in responseProducts)
            {
                pids.AddRange(responseProduct.Colorways.Select(x => x.Pid));

                foreach (var colourway in responseProduct.Colorways)
                {
                    FilterDefinition<ColourWay> filterDefinition = Builders<ColourWay>.Filter.Eq(x => x.PId, colourway.Pid);
                    UpdateDefinition<ColourWay> update = Builders<ColourWay>.Update
                        .Set("ProductId", products.Where(x => x.PIds.Contains(colourway.Pid)).Select(x => x.Id).FirstOrDefault())
                        .Set("PId", colourway.Pid)
                        .Set("Guid", colourway.CloudProductId)
                        .Set("Url", colourway.PdpUrl)
                        .Set("Description", colourway.ColorDescription)
                        .Set("InStock", colourway.InStock)
                        .Set("IsComingSoon", colourway.IsComingSoon)
                        .Set("IsNew", colourway.IsNew)
                        .Set("Active", true)
                        .Set("WebsiteId", websiteId);

                    var upsertOne = new UpdateOneModel<ColourWay>(filterDefinition, update) { IsUpsert = true };
                    bulkColourwaysUpsert.Add(upsertOne);
                }
            }

            await repository.UpsertColourways(bulkColourwaysUpsert);
            var colourways =  await repository.GetColourways(Builders<ColourWay>.Filter.In(x => x.PId, pids));

            var bulkPricesUpsert = new List<WriteModel<Price>>();
            foreach (var responseProduct in responseProducts)
            {
                foreach (var colourway in responseProduct.Colorways)
                {
                    var colourwayId = colourways.FirstOrDefault(x => x.PId == colourway.Pid)?.Id;
                    FilterDefinition<Price> filterDefinition = Builders<Price>.Filter.Eq(x => x.ColourwayId, colourwayId);
                    UpdateDefinition<Price> update = Builders<Price>.Update
                        .Set("ProductId", products.Where(x => x.PIds.Contains(colourway.Pid)).Select(x => x.Id).FirstOrDefault())
                        .Set("ColourwayId", colourwayId)
                        .Set("Currency", colourway.Price.Currency)
                        .Set("CurrentPrice", colourway.Price.CurrentPrice)
                        .Set("FullPrice", colourway.Price.FullPrice)
                        .Set("Discounted", colourway.Price.Discounted)
                        .Set("Active", true)
                        .Set("WebsiteId", websiteId);

                    var upsertOne = new UpdateOneModel<Price>(filterDefinition, update) { IsUpsert = true };
                    bulkPricesUpsert.Add(upsertOne);
                }
            }

            await repository.UpsertPrices(bulkPricesUpsert);
            var prices = await repository.GetPrices(Builders<Price>.Filter.In(x => x.ColourwayId, colourways.Select(x => x.Id)));

            foreach (var product in products)
            {
                FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(x => x.Id, product.Id);
                UpdateDefinition<Product> update = Builders<Product>.Update
                    .Set("ColourWayIds", colourways.Where(x => x.ProductId == product.Id).Select(x => x.Id))
                    .Set("PriceIds", prices.Where(x => x.ProductId == product.Id).Select(x => x.Id));

                var upsertOne = new UpdateOneModel<Product>(filterDefinition, update);
                bulkProductUpsert.Add(upsertOne);
            }

            await repository.UpsertProducts(bulkProductUpsert);
        }
    }
}
