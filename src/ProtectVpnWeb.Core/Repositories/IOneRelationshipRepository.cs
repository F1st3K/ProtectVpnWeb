using ProtectVpnWeb.Core.Entities.Interfaces;

namespace ProtectVpnWeb.Core.Repositories;

public interface IOneRelationshipRepository<in TSource, out TTarget>
    where TSource : IHasOneRelationship<TTarget>
    where TTarget : IEntity
{
    public TTarget GetRelatedEntity(TSource relation);
}