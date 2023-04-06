using MongoDB.Driver;
using NikeProductPolling.Models.DBDocuments;

namespace NikeProductPolling.Repositories
{
    internal class ProductRepository
    {
        private MongoClient dbClient = new MongoClient("mongodb://127.0.0.1:27017");
        private IMongoDatabase db;

        public ProductRepository()
        {
            db = dbClient.GetDatabase("shoespydb");
        }

        internal async Task<Website> GetWebsite(FilterDefinition<Website> filter)
        {
            var collection = db.GetCollection<Website>("websites");

            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        internal async Task<string> UpsertWebsite(Website website)
        {
            var collection = db.GetCollection<Website>("websites");

            FilterDefinition<Website> filterDefinition = Builders<Website>.Filter.And(
                Builders<Website>.Filter.Eq(x => x.Name, "Nike"),
                Builders<Website>.Filter.Eq(x => x.CountryId, "NZ"));
            UpdateDefinition<Website> update = Builders<Website>.Update
                .Set("Name", website.Name)
                .Set("CountryId", website.CountryId)
                .Set("Url", website.Url)
                .Set("Active", true);

            var result = await collection.UpdateOneAsync(filterDefinition, update, new UpdateOptions { IsUpsert = true });
            return result.UpsertedId.ToString();
        }

        internal async Task<IEnumerable<Product>> GetProducts(FilterDefinition<Product> filter)
        {
            var collection = db.GetCollection<Product>("products");

            return await collection.Find(filter).ToListAsync();
        }

        internal async Task UpsertProducts(IEnumerable<WriteModel<Product>> bulkOps)
        {
            var collection = db.GetCollection<Product>("products");

            await collection.BulkWriteAsync(bulkOps);
        }

        internal async Task<IEnumerable<ColourWay>> GetColourways(FilterDefinition<ColourWay> filter)
        {
            var collection = db.GetCollection<ColourWay>("colourways");

            return await collection.Find(filter).ToListAsync();
        }

        internal async Task UpsertColourways(IEnumerable<WriteModel<ColourWay>> bulkOps)
        {
            var collection = db.GetCollection<ColourWay>("colourways");

            await collection.BulkWriteAsync(bulkOps);
        }

        internal async Task<IEnumerable<Price>> GetPrices(FilterDefinition<Price> filter)
        {
            var collection = db.GetCollection<Price>("prices");

            return await collection.Find(filter).ToListAsync();
        }

        internal async Task UpsertPrices(List<WriteModel<Price>> bulkOps)
        {
            var collection = db.GetCollection<Price>("prices");

            await collection.BulkWriteAsync(bulkOps);
        }

        internal async Task<IEnumerable<Brand>> GetBrands()
        {
            var collection = db.GetCollection<Brand>("brands");

            return await collection.Find(Builders<Brand>.Filter.Empty).ToListAsync();
        }
    }
}
