using System.ComponentModel.DataAnnotations;

namespace database.Entities;

public abstract class EntityBase<TKey>
{
    [Key]
    public TKey Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}