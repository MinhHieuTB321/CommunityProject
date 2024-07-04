using Domain.Entities;

namespace Application;
public interface IMongoRepository
{
    Task InsertAsync<T>(string table, T record) where T : BaseEntity;
    Task<List<T>> GetAllAsync<T>(string table);
    Task<T> GetByIdAsync<T>(string table, Guid id);
    Task UpsertAsync<T>(string table, Guid id, T record) where T : BaseEntity;
    Task<bool> DeleteAsync<T>(string table, Guid id);
}