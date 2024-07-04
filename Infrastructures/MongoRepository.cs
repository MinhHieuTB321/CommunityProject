using Application;
using Application.Services.Interfaces;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructures;

public class MongoRepository : IMongoRepository
{
    private readonly IMongoDatabase _db;
    private readonly IClaimsService _claims;
    public MongoRepository(AppSettings appSettings, IClaimsService claims)
    {
        var client = new MongoClient(appSettings.ConnectionStrings.MongoDbConnection);
        _db = client.GetDatabase(appSettings.DatabaseName);
        _claims = claims;
    }
    public async Task<bool> DeleteAsync<T>(string table, Guid id)
    {
        var collection = _db.GetCollection<T>(table);
        var filter = Builders<T>.Filter.Eq("Id", id);
        var result = await collection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    public async Task<List<T>> GetAllAsync<T>(string table)
    {
        var collection = _db.GetCollection<T>(table);
        return await collection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<T> GetByIdAsync<T>(string table, Guid id)
    {
        var collection = _db.GetCollection<T>(table);
        var filter = Builders<T>.Filter.Eq("Id", id);

        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task InsertAsync<T>(string table, T record) where T : BaseEntity
    {
        record.CreatedBy = _claims.GetCurrentUser;
        var collection = _db.GetCollection<T>(table);
        await collection.InsertOneAsync(record);
    }

    [Obsolete]
    public async Task UpsertAsync<T>(string table, Guid id, T record) where T : BaseEntity
    {
        record.ModificationDate = DateTime.Now;
        var collection = _db.GetCollection<T>(table);
        await collection.ReplaceOneAsync(
            new BsonDocument("_id", id),
            record,
            new UpdateOptions { IsUpsert = true }
            );
    }
}
