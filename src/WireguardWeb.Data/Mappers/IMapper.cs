namespace WireguardWeb.Data.Mappers;

public interface IMapper<TDomain, TEntity>
{
    public TDomain ToDomain(TEntity entity);
    public TEntity ToEntity(TDomain domain);
}