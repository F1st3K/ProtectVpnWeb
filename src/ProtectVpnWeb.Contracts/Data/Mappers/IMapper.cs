namespace ProtectVpnWeb.Contracts.Data.Mappers;

public interface IMapper<TDomain, TData>
{
    public TDomain ToDomain(TData entity);
    public TData ToData(TDomain domain);
}