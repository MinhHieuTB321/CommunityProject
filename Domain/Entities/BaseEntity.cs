using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;
public abstract class BaseEntity
{
    [BsonId]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public Guid? CreatedBy { get; set; } = Guid.Empty;
    public DateTime? ModificationDate { get; set; } = null;
    public Guid? ModifiedBy { get; set; } = default!;
    public bool IsDeleted { get; set; } = false;
}