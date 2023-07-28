using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Repositories.UserRepository.dto;

namespace WireguardWeb.Core.Repositories.UserRepository;

public interface IUserRepository
{
    public void Add(AddUserDto dto);
    public void Update(UpdateUserDto dto);
    public User GetById(string id);
    public User[] GetAllByUserId(string id);
    public User[] GetAll();
    public void Remove(string id);
}