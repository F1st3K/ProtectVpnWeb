using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Repositories.ConnectionRepository.dto;

namespace WireguardWeb.Core.Repositories.ConnectionRepository;

public interface IConnectionRepository
{
    public void Add(AddConnectionDto dto);
    public void Update(UpdateConnectionDto dto);
    public Connection GetById(string id);
    public Connection[] GetAllByUserId(string id);
    public Connection[] GetAll();
    public void Remove(string id);
}