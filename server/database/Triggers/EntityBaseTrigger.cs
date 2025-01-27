using database.Entities;
using EntityFrameworkCore.Triggered;

namespace database.Triggers;

public class EntityBaseTrigger<T> : IBeforeSaveTrigger<EntityBase<T>>
{
    public Task BeforeSave(ITriggerContext<EntityBase<T>> context, CancellationToken cancellationToken)
    {
        context.Entity.UpdatedAt = DateTimeOffset.UtcNow;

        if (context.ChangeType == ChangeType.Added)
            context.Entity.CreatedAt = DateTimeOffset.UtcNow;

        return Task.CompletedTask;
    }
}