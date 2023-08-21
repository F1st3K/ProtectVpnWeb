using ProtectVpnWeb.Core.Entities.Interfaces;

namespace ProtectVpnWeb.Core.Repositories;

public interface IManyRelationshipsRepository<in TSource, out TTarget>
    where TSource : IHasManyRelationships<TTarget>
    where TTarget : IEntity
{
    public TTarget[] GetRelatedEntities(TSource relation);
}