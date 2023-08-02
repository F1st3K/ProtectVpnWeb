namespace WireguardWeb.Data.Mappers;

public interface IMapper<TD, TE>
{
    public TD ToDomain(TE entity);
    public TE ToEntity(TD domain);
}