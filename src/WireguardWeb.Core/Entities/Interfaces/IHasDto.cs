namespace WireguardWeb.Core.Entities.Interfaces;

public interface IHasDto<TDto>
{
    public TDto ToDto();
}